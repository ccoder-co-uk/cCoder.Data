// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace cCoder.Data.Brokers;

internal sealed class JsonBroker : IJsonBroker
{
    public JsonSerializerSettings GetJsonSerializerSettings() =>
        new()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            TypeNameHandling = TypeNameHandling.Objects,
            Formatting = Formatting.None,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            NullValueHandling = NullValueHandling.Ignore,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            ContractResolver = new DefaultContractResolver
            {
                IgnoreSerializableAttribute = true,
            },
        };

    public JsonSerializerSettings GetODataJsonSerializerSettings() =>
        new()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            TypeNameHandling = TypeNameHandling.None,
            Formatting = Formatting.None,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            NullValueHandling = NullValueHandling.Ignore,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            ContractResolver = new DefaultContractResolver
            {
                IgnoreSerializableAttribute = true,
            },
            MaxDepth = 4,
        };

    public T Deserialize<T>(
        string value) =>
        Deserialize<T>(
            value: value,
            jsonSerializerSettings: GetJsonSerializerSettings());

    public T Deserialize<T>(
        string value,
        JsonSerializerSettings jsonSerializerSettings) =>
        JsonConvert.DeserializeObject<T>(
            value: value,
            settings: jsonSerializerSettings);

    public string Serialize(object value) =>
        JsonConvert.SerializeObject(value: value);

    public string SerializeUsingJsonSettings(object value) =>
        Serialize(
            value: value,
            jsonSerializerSettings: GetJsonSerializerSettings());

    public string SerializeUsingJsonSettings(object value, int maxDepth)
    {
        JsonSerializerSettings jsonSerializerSettings =
            GetJsonSerializerSettings();

        jsonSerializerSettings.MaxDepth = maxDepth;

        return Serialize(
            value: value,
            jsonSerializerSettings: jsonSerializerSettings);
    }

    public string SerializeForOData(object value) =>
        Serialize(
            value: value,
            jsonSerializerSettings: GetODataJsonSerializerSettings());

    public string Serialize(
        object value,
        JsonSerializerSettings jsonSerializerSettings) =>
        JsonConvert.SerializeObject(
            value: value,
            formatting: Formatting.None,
            settings: jsonSerializerSettings);
}