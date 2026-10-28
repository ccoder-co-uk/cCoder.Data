// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Text.Json;
using System.Text.Json.Serialization;

namespace cCoder.Data.Brokers;

internal sealed class JsonBroker : IJsonBroker
{
    public JsonSerializerOptions GetJsonSerializerOptions() =>
        new()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true
        };

    public JsonSerializerOptions GetODataJsonSerializerOptions() =>
        new()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true,
            MaxDepth = 4
        };

    public T Deserialize<T>(
        string value) =>
        Deserialize<T>(
            value: value,
            jsonSerializerOptions: GetJsonSerializerOptions());

    public T Deserialize<T>(
        string value,
        JsonSerializerOptions jsonSerializerOptions) =>
        JsonSerializer.Deserialize<T>(
            json: value,
            options: jsonSerializerOptions);

    public string Serialize(object value) =>
        JsonSerializer.Serialize(value: value);

    public string SerializeUsingJsonSettings(object value) =>
        Serialize(
            value: value,
            jsonSerializerOptions: GetJsonSerializerOptions());

    public string SerializeUsingJsonSettings(object value, int maxDepth)
    {
        JsonSerializerOptions jsonSerializerOptions =
            GetJsonSerializerOptions();

        jsonSerializerOptions.MaxDepth = maxDepth;

        return Serialize(
            value: value,
            jsonSerializerOptions: jsonSerializerOptions);
    }

    public string SerializeForOData(object value) =>
        Serialize(
            value: value,
            jsonSerializerOptions: GetODataJsonSerializerOptions());

    public string Serialize(
        object value,
        JsonSerializerOptions jsonSerializerOptions) =>
        JsonSerializer.Serialize(
            value: value,
            options: jsonSerializerOptions);
}