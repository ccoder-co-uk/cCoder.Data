// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using Newtonsoft.Json;
using cCoder.Data.Brokers;

namespace cCoder.Data.Extensions;

public static class ObjectExtensions
{
    private static readonly IJsonBroker jsonBroker = new JsonBroker();

    public static JsonSerializerSettings GetJSONSettings() =>
        jsonBroker.GetJsonSerializerSettings();

    public static JsonSerializerSettings GetODataJsonSettings() =>
        jsonBroker.GetODataJsonSerializerSettings();

    internal static T FromJson<T>(string value) =>
        jsonBroker.Deserialize<T>(
            value: value,
            jsonSerializerSettings: GetJSONSettings());

    internal static string ToJsonUsingDefaultSettings(object value) =>
        jsonBroker.Serialize(value: value);

    public static string ToJson(this object value) =>
        jsonBroker.Serialize(
            value: value,
            jsonSerializerSettings: GetJSONSettings());

    public static string ToJson(this object value, int depth)
    {
        JsonSerializerSettings settings = GetJSONSettings();
        settings.MaxDepth = depth;

        return jsonBroker.Serialize(
            value: value,
            jsonSerializerSettings: settings);
    }

    public static string ToJson(
        this object value,
        JsonSerializerSettings jsonSerializerSettings) =>
        jsonBroker.Serialize(
            value: value,
            jsonSerializerSettings: jsonSerializerSettings);

    public static string ToJsonForOdata(this object value) =>
        jsonBroker.Serialize(
            value: value,
            jsonSerializerSettings: GetODataJsonSettings());
}