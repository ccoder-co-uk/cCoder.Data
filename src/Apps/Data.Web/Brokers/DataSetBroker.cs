using System.Collections;
using System.Reflection;
using System.Text.Json;
using cCoder.Data;
using Data.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Data.Web.Brokers;

internal sealed class DataSetBroker(CoreDataContext context)
    : IDataSetBroker
{
    private static readonly MethodInfo SetMethod = typeof(DbContext)
        .GetMethods()
        .Single(method =>
            method.Name == nameof(DbContext.Set)
            && method.IsGenericMethod
            && method.GetParameters().Length == 0);

    public string GetCurrentSsoUserId() =>
        context.AuthInfo?.SSOUserId ?? string.Empty;

    public Task<DataEntitySet[]> SelectEntitySetsAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        DataEntitySet[] entitySets = GetEntitySets()
            .Select(item => ToEntitySet(item.Name, item.EntityType))
            .OrderBy(entitySet => entitySet.DisplayName)
            .ToArray();

        return Task.FromResult(entitySets);
    }

    public Task<DataRows> SelectRowsAsync(
        string entitySet,
        int skip,
        int take,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        (string setName, IEntityType entityType) = GetEntitySet(entitySet);
        IQueryable query = CreateQueryable(entityType.ClrType);

        if (skip > 0)
            query = ApplyQueryableIntMethod(nameof(Queryable.Skip), entityType.ClrType, query, skip);

        query = ApplyQueryableIntMethod(nameof(Queryable.Take), entityType.ClrType, query, take);

        Dictionary<string, object>[] rows = query
            .Cast<object>()
            .Select(entity => ToDictionary(entityType, entity))
            .ToArray();

        return Task.FromResult(new DataRows
        {
            EntitySet = setName,
            Skip = skip,
            Take = take,
            Rows = rows
        });
    }

    public async Task<Dictionary<string, object>> InsertRowAsync(
        string entitySet,
        Dictionary<string, JsonElement> values,
        CancellationToken cancellationToken)
    {
        (_, IEntityType entityType) = GetEntitySet(entitySet);
        object entity = Activator.CreateInstance(entityType.ClrType)
            ?? throw new InvalidOperationException($"Unable to create {entityType.ClrType.Name}.");

        foreach (IProperty property in GetWritableProperties(entityType, forCreate: true))
            TrySetProperty(entity, property, values);

        context.Add(entity);
        await context.SaveChangesAsync(cancellationToken);

        return ToDictionary(entityType, entity);
    }

    public async Task<Dictionary<string, object>> UpdateRowAsync(
        string entitySet,
        Dictionary<string, JsonElement> values,
        CancellationToken cancellationToken)
    {
        (_, IEntityType entityType) = GetEntitySet(entitySet);
        object entity = await FindEntityAsync(entityType, values, cancellationToken);

        foreach (IProperty property in GetWritableProperties(entityType, forCreate: false))
            TrySetProperty(entity, property, values);

        await context.SaveChangesAsync(cancellationToken);

        return ToDictionary(entityType, entity);
    }

    public async Task DeleteRowAsync(
        string entitySet,
        Dictionary<string, JsonElement> values,
        CancellationToken cancellationToken)
    {
        (_, IEntityType entityType) = GetEntitySet(entitySet);
        object entity = await FindEntityAsync(entityType, values, cancellationToken);

        context.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
    }

    private (string Name, IEntityType EntityType)[] GetEntitySets() =>
        context
            .GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(property => property.PropertyType.IsGenericType)
            .Where(property => property.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
            .Select(property => (
                property.Name,
                context.Model.FindEntityType(property.PropertyType.GetGenericArguments()[0])))
            .Where(item => item.Item2 is not null)
            .Select(item => (item.Name, item.Item2!))
            .ToArray();

    private (string Name, IEntityType EntityType) GetEntitySet(string entitySet)
    {
        (string Name, IEntityType EntityType)? match = GetEntitySets()
            .FirstOrDefault(item => string.Equals(
                item.Name,
                entitySet,
                StringComparison.OrdinalIgnoreCase));

        return match?.EntityType is null
            ? throw new InvalidOperationException($"Unknown entity set '{entitySet}'.")
            : match.Value;
    }

    private IQueryable CreateQueryable(Type clrType)
    {
        object dbSet = SetMethod
            .MakeGenericMethod(clrType)
            .Invoke(context, null)!;

        return (IQueryable)dbSet;
    }

    private static IQueryable ApplyQueryableIntMethod(
        string methodName,
        Type clrType,
        IQueryable query,
        int value)
    {
        MethodInfo method = typeof(Queryable)
            .GetMethods()
            .Single(method =>
                method.Name == methodName
                && method.GetParameters().Length == 2
                && method.GetParameters()[1].ParameterType == typeof(int));

        return (IQueryable)method
            .MakeGenericMethod(clrType)
            .Invoke(null, [query, value])!;
    }

    private async Task<object> FindEntityAsync(
        IEntityType entityType,
        Dictionary<string, JsonElement> values,
        CancellationToken cancellationToken)
    {
        IProperty[] keyProperties = entityType.FindPrimaryKey()?.Properties.ToArray() ?? [];

        if (keyProperties.Length == 0)
            throw new InvalidOperationException($"{entityType.ClrType.Name} does not define a primary key.");

        object[] keyValues = keyProperties
            .Select(property => GetPropertyValue(values, property))
            .ToArray();

        object entity = await context.FindAsync(
            entityType.ClrType,
            keyValues,
            cancellationToken);

        return entity
            ?? throw new InvalidOperationException($"{entityType.ClrType.Name} row was not found.");
    }

    private static DataEntitySet ToEntitySet(string name, IEntityType entityType) =>
        new()
        {
            Name = name,
            DisplayName = SplitName(name),
            ClrType = entityType.ClrType.FullName ?? entityType.ClrType.Name,
            Table = entityType.GetTableName() ?? name,
            KeyProperties = entityType.FindPrimaryKey()?.Properties
                .Select(property => property.Name)
                .ToArray() ?? [],
            Properties = entityType.GetProperties()
                .Where(property => !property.IsShadowProperty())
                .OrderByDescending(property => property.IsPrimaryKey())
                .ThenBy(property => property.Name)
                .Select(ToProperty)
                .ToArray()
        };

    private static DataProperty ToProperty(IProperty property)
    {
        Type type = Nullable.GetUnderlyingType(property.ClrType) ?? property.ClrType;

        return new()
        {
            Name = property.Name,
            Type = type.Name,
            IsKey = property.IsPrimaryKey(),
            IsNullable = property.IsNullable || Nullable.GetUnderlyingType(property.ClrType) is not null,
            CanCreate = !property.IsPrimaryKey() || property.ValueGenerated == ValueGenerated.Never,
            CanUpdate = !property.IsPrimaryKey() && property.GetAfterSaveBehavior() != PropertySaveBehavior.Throw,
            IsLongText = type == typeof(string) && (property.GetMaxLength() is null || property.GetMaxLength() > 255)
        };
    }

    private static IEnumerable<IProperty> GetWritableProperties(
        IEntityType entityType,
        bool forCreate) =>
            entityType
                .GetProperties()
                .Where(property => !property.IsShadowProperty())
                .Where(property => forCreate
                    ? !property.IsPrimaryKey() || property.ValueGenerated == ValueGenerated.Never
                    : !property.IsPrimaryKey() && property.GetAfterSaveBehavior() != PropertySaveBehavior.Throw);

    private static Dictionary<string, object> ToDictionary(IEntityType entityType, object entity) =>
        entityType
            .GetProperties()
            .Where(property => !property.IsShadowProperty())
            .OrderByDescending(property => property.IsPrimaryKey())
            .ThenBy(property => property.Name)
            .ToDictionary(
                property => property.Name,
                property => ToJsonFriendlyValue(property.PropertyInfo?.GetValue(entity)));

    private static object ToJsonFriendlyValue(object value) =>
        value switch
        {
            null => null,
            DateTime dateTime => dateTime.ToString("O"),
            DateTimeOffset dateTimeOffset => dateTimeOffset.ToString("O"),
            byte[] bytes => Convert.ToBase64String(bytes),
            IEnumerable enumerable when value is not string => enumerable.Cast<object>().ToArray(),
            _ => value
        };

    private static void TrySetProperty(
        object entity,
        IProperty property,
        Dictionary<string, JsonElement> values)
    {
        if (!TryGetJson(values, property.Name, out JsonElement element))
            return;

        object value = ConvertJsonElement(element, property.ClrType);
        property.PropertyInfo?.SetValue(entity, value);
    }

    private static object GetPropertyValue(
        Dictionary<string, JsonElement> values,
        IProperty property)
    {
        if (!TryGetJson(values, property.Name, out JsonElement element))
            throw new InvalidOperationException($"Key property '{property.Name}' is required.");

        return ConvertJsonElement(element, property.ClrType);
    }

    private static bool TryGetJson(
        Dictionary<string, JsonElement> values,
        string name,
        out JsonElement value)
    {
        foreach (KeyValuePair<string, JsonElement> item in values)
        {
            if (string.Equals(item.Key, name, StringComparison.OrdinalIgnoreCase))
            {
                value = item.Value;
                return true;
            }
        }

        value = default;
        return false;
    }

    private static object ConvertJsonElement(JsonElement element, Type targetType)
    {
        Type type = Nullable.GetUnderlyingType(targetType) ?? targetType;

        if (element.ValueKind == JsonValueKind.Null)
            return null;

        if (type == typeof(string))
            return element.ValueKind == JsonValueKind.String
                ? element.GetString()
                : element.GetRawText();

        if (type == typeof(Guid))
            return element.ValueKind == JsonValueKind.String
                ? Guid.Parse(element.GetString()!)
                : Guid.Parse(element.GetRawText());

        if (type == typeof(int))
            return element.GetInt32();

        if (type == typeof(long))
            return element.GetInt64();

        if (type == typeof(short))
            return element.GetInt16();

        if (type == typeof(bool))
            return element.GetBoolean();

        if (type == typeof(decimal))
            return element.GetDecimal();

        if (type == typeof(double))
            return element.GetDouble();

        if (type == typeof(float))
            return element.GetSingle();

        if (type == typeof(DateTime))
            return element.GetDateTime();

        if (type == typeof(DateTimeOffset))
            return element.GetDateTimeOffset();

        if (type == typeof(byte[]))
            return Convert.FromBase64String(element.GetString() ?? string.Empty);

        if (type.IsEnum)
            return Enum.Parse(type, element.GetString() ?? element.GetRawText(), ignoreCase: true);

        return JsonSerializer.Deserialize(element.GetRawText(), type);
    }

    private static string SplitName(string name) =>
        string.Concat(name.Select((character, index) =>
            index > 0 && char.IsUpper(character)
                ? " " + character
                : character.ToString()));
}
