using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace cCoder.Data.Extensions;

public static class TypeExtensions
{
    public static bool IsJoinType(this Type type)
    {
        TableAttribute table = type.GetCustomAttribute<TableAttribute>();
        return table != null
            && type.GetProperties().Length == 4
            && type.GetProperties()
                .Where(p => p.PropertyType.IsValueType || p.PropertyType == typeof(string))
                .All(p => p.GetCustomAttribute<ForeignKeyAttribute>() != null);
    }

    public static PropertyInfo GetIdProperty(this Type type)
    {
        if (!type.IsJoinType())
        {
            PropertyInfo idProperty = type.GetProperty("ID")
                ?? type.GetProperty("Id")
                ?? type.GetProperty(type.Name + "Id")
                ?? type.GetProperty(type.Name + "ID")
                ?? type.GetProperties().FirstOrDefault(p => p.GetCustomAttributes(typeof(KeyAttribute), false).Any());

            if (idProperty != null)
                return idProperty;
        }
        else
        {
            return new CompositePropertyInfo(type);
        }

        return null;
    }
}
