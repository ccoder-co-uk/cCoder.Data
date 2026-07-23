// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Collections;
using System.Reflection;
using System.Text.Json;
using cCoder.Data;
using Data.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Data.Web.Dependencies;

internal sealed class DataSetBroker(CoreDataContext context)
    : IDataSetBroker
{
    private static readonly MethodInfo SetMethod = typeof(DbContext)
        .GetMethods()
        .Single(predicate:method =>
            method.Name == nameof(DbContext.Set)
            && method.IsGenericMethod
            && method.GetParameters().Length == 0);

    public string GetCurrentSsoUserId() =>
        context.AuthInfo?.SSOUserId ?? string.Empty;

    public Task<DataEntitySet[]> SelectEntitySetsAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        DataEntitySet[] entitySets = GetEntitySets()
            .Select(selector:item => ToEntitySet(item.Name, item.EntityType))
            .OrderBy(keySelector:entitySet => entitySet.DisplayName)
            .ToArray();

        return Task.FromResult(result:entitySets);
    }

    public Task<DataRows> SelectRowsAsync(
        string entitySet,
        int skip,
        int take,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        (string setName, IEntityType entityType) = GetEntitySet(entitySet:entitySet);
        IQueryable query = CreateQueryable(clrType:entityType.ClrType);

        if (skip > 0)
            query = ApplyQueryableIntMethod(methodName:nameof(Queryable.Skip), clrType:entityType.ClrType, query:query, value:skip);

        query = ApplyQueryableIntMethod(methodName:nameof(Queryable.Take), clrType:entityType.ClrType, query:query, value:take);

        Dictionary<string, object>[] rows = query
            .Cast<object>()
            .Select(selector:entity => ToDictionary(entityType:entityType, entity:entity))
            .ToArray();

        return Task.FromResult(result:new DataRows
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
        (_, IEntityType entityType) = GetEntitySet(entitySet:entitySet);

        object entity = Activator.CreateInstance(type:entityType.ClrType)
            ?? throw new InvalidOperationException($"Unable to create {entityType.ClrType.Name}.");

        foreach (IProperty property in GetWritableProperties(entityType:entityType, forCreate: true))
            TrySetProperty(entity:entity, property:property, values:values);

        context.Add(entity:entity);
        await context.SaveChangesAsync(cancellationToken:cancellationToken);

        return ToDictionary(entityType:entityType, entity:entity);
    }

    public async Task<Dictionary<string, object>> UpdateRowAsync(
        string entitySet,
        Dictionary<string, JsonElement> values,
        CancellationToken cancellationToken)
    {
        (_, IEntityType entityType) = GetEntitySet(entitySet:entitySet);
        object entity = await FindEntityAsync(entityType:entityType, values:values, cancellationToken:cancellationToken);

        foreach (IProperty property in GetWritableProperties(entityType:entityType, forCreate: false))
            TrySetProperty(entity:entity, property:property, values:values);

        await context.SaveChangesAsync(cancellationToken:cancellationToken);

        return ToDictionary(entityType:entityType, entity:entity);
    }

    public async Task DeleteRowAsync(
        string entitySet,
        Dictionary<string, JsonElement> values,
        CancellationToken cancellationToken)
    {
        (_, IEntityType entityType) = GetEntitySet(entitySet:entitySet);
        object entity = await FindEntityAsync(entityType:entityType, values:values, cancellationToken:cancellationToken);

        context.Remove(entity:entity);
        await context.SaveChangesAsync(cancellationToken:cancellationToken);
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
            .Where(predicate:item => item.Item2 is not null)
            .Select(selector:item => (item.Name, item.Item2!))
            .ToArray();

    private (string Name, IEntityType EntityType) GetEntitySet(string entitySet)
    {
        (string Name, IEntityType EntityType)? match = GetEntitySets()
            .FirstOrDefault(predicate:item => string.Equals(
a:                item.Name,
b:                entitySet,
comparisonType:                StringComparison.OrdinalIgnoreCase));

        return match?.EntityType is null
            ? throw new InvalidOperationException($"Unknown entity set '{entitySet}'.")
            : match.Value;
    }

    private IQueryable CreateQueryable(Type clrType)
    {
        object dbSet = SetMethod
            .MakeGenericMethod(typeArguments:clrType)
            .Invoke(obj:context, parameters:null)!;

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
            .Single(predicate:method =>
                method.Name == methodName
                && method.GetParameters().Length == 2
                && method.GetParameters()[1].ParameterType == typeof(int));

        return (IQueryable)method
            .MakeGenericMethod(typeArguments:clrType)
            .Invoke(obj:null, parameters:[query, value])!;
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
            .Select(selector:property => GetPropertyValue(values:values, property:property))
            .ToArray();

        object entity = await context.FindAsync(
entityType:            entityType.ClrType,
keyValues:            keyValues,
cancellationToken:            cancellationToken);

        return entity
            ?? throw new InvalidOperationException($"{entityType.ClrType.Name} row was not found.");
    }

    private static DataEntitySet ToEntitySet(string name, IEntityType entityType) =>
        new()
        {
            Name = name,
            DisplayName = SplitName(name:name),
            ClrType = entityType.ClrType.FullName ?? entityType.ClrType.Name,
            Table = entityType.GetTableName() ?? name,
            KeyProperties = entityType.FindPrimaryKey()?.Properties
                .Select(selector:property => property.Name)
                .ToArray() ?? [],
            Properties = entityType.GetProperties()
                .Where(property => !property.IsShadowProperty())
                .OrderByDescending(property => property.IsPrimaryKey())
                .ThenBy(keySelector:property => property.Name)
                .Select(selector:ToProperty)
                .ToArray()
        };

    private static DataProperty ToProperty(IProperty property)
    {
        Type type = Nullable.GetUnderlyingType(nullableType:property.ClrType) ?? property.ClrType;

        return new()
        {
            Name = property.Name,
            Type = type.Name,
            IsKey = property.IsPrimaryKey(),
            IsNullable = property.IsNullable || Nullable.GetUnderlyingType(nullableType:property.ClrType) is not null,
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
                .Where(predicate:property => !property.IsShadowProperty())
                .Where(predicate:property => forCreate
                    ? !property.IsPrimaryKey() || property.ValueGenerated == ValueGenerated.Never
                    : !property.IsPrimaryKey() && property.GetAfterSaveBehavior() != PropertySaveBehavior.Throw);

    private static Dictionary<string, object> ToDictionary(IEntityType entityType, object entity) =>
        entityType
            .GetProperties()
            .Where(property => !property.IsShadowProperty())
            .OrderByDescending(property => property.IsPrimaryKey())
            .ThenBy(keySelector:property => property.Name)
            .ToDictionary(
keySelector:                property => property.Name,
elementSelector:                property => ToJsonFriendlyValue(value:property.PropertyInfo?.GetValue(entity)));

    private static object ToJsonFriendlyValue(object value) =>
        value switch
        {
            null => null,
            DateTime dateTime => dateTime.ToString(format:"O"),
            DateTimeOffset dateTimeOffset => dateTimeOffset.ToString(format:"O"),
            byte[] bytes => Convert.ToBase64String(inArray:bytes),
            IEnumerable enumerable when value is not string => enumerable.Cast<object>().ToArray(),
            _ => value
        };

    private static void TrySetProperty(
        object entity,
        IProperty property,
        Dictionary<string, JsonElement> values)
    {
        if (!TryGetJson(values:values, name:property.Name, value:out JsonElement element))
            return;

        object value = ConvertJsonElement(element:element, targetType:property.ClrType);
        property.PropertyInfo?.SetValue(obj:entity, value:value);
    }

    private static object GetPropertyValue(
        Dictionary<string, JsonElement> values,
        IProperty property)
    {
        if (!TryGetJson(values:values, name:property.Name, value:out JsonElement element))
            throw new InvalidOperationException($"Key property '{property.Name}' is required.");

        return ConvertJsonElement(element:element, targetType:property.ClrType);
    }

    private static bool TryGetJson(
        Dictionary<string, JsonElement> values,
        string name,
        out JsonElement value)
    {
        foreach (KeyValuePair<string, JsonElement> item in values)
        {
            if (string.Equals(a:item.Key, b:name, comparisonType:StringComparison.OrdinalIgnoreCase))
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
        Type type = Nullable.GetUnderlyingType(nullableType:targetType) ?? targetType;

        if (element.ValueKind == JsonValueKind.Null)
            return null;

        if (type == typeof(string))
            return element.ValueKind == JsonValueKind.String
                ? element.GetString()
                : element.GetRawText();

        if (type == typeof(Guid))
            return element.ValueKind == JsonValueKind.String
                ? Guid.Parse(input:element.GetString()!)
                : Guid.Parse(input:element.GetRawText());

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
            return Convert.FromBase64String(s:element.GetString() ?? string.Empty);

        if (type.IsEnum)
            return Enum.Parse(enumType:type, value:element.GetString() ?? element.GetRawText(), ignoreCase: true);

        return JsonSerializer.Deserialize(json:element.GetRawText(), returnType:type);
    }

    private static string SplitName(string name) =>
        string.Concat(values:name.Select(selector:(character, index) =>
            index > 0 && char.IsUpper(character)
                ? " " + character
                : character.ToString()));
}