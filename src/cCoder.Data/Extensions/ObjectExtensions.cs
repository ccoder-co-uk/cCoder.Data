using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace cCoder.Data.Extensions;

public static class ObjectExtensions
{
    public static JsonSerializerSettings GetJSONSettings() => new()
    {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        TypeNameHandling = TypeNameHandling.Objects,
        Formatting = Formatting.None,
        DateFormatHandling = DateFormatHandling.IsoDateFormat,
        NullValueHandling = NullValueHandling.Ignore,
        DateTimeZoneHandling = DateTimeZoneHandling.Utc,
        ContractResolver = new DefaultContractResolver { IgnoreSerializableAttribute = true },
    };

    public static JsonSerializerSettings GetODataJsonSettings() => new()
    {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        TypeNameHandling = TypeNameHandling.None,
        Formatting = Formatting.None,
        DateFormatHandling = DateFormatHandling.IsoDateFormat,
        NullValueHandling = NullValueHandling.Ignore,
        DateTimeZoneHandling = DateTimeZoneHandling.Utc,
        ContractResolver = new DefaultContractResolver { IgnoreSerializableAttribute = true },
        MaxDepth = 4,
    };

    public static string ToJson(this object value) =>
        JsonConvert.SerializeObject(value, Formatting.None, GetJSONSettings());

    public static string ToJson(this object value, int depth)
    {
        JsonSerializerSettings settings = GetJSONSettings();
        settings.MaxDepth = depth;
        return JsonConvert.SerializeObject(value, Formatting.None, settings);
    }

    public static string ToJson(this object value, JsonSerializerSettings settings) =>
        JsonConvert.SerializeObject(value, Formatting.None, settings);

    public static string ToJsonForOdata(this object value) =>
        JsonConvert.SerializeObject(value, Formatting.None, GetODataJsonSettings());
}
