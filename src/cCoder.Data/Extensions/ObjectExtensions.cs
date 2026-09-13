// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Brokers;

namespace cCoder.Data.Extensions;

public static class ObjectExtensions
{
    private static readonly IJsonBroker jsonBroker = new JsonBroker();

    internal static T FromJson<T>(string value) =>
        jsonBroker.Deserialize<T>(value: value);

    internal static string ToJsonUsingDefaultSettings(object value) =>
        jsonBroker.Serialize(value: value);

    public static string ToJson(this object value) =>
        jsonBroker.SerializeUsingJsonSettings(value: value);

    public static string ToJson(this object value, int depth) =>
        jsonBroker.SerializeUsingJsonSettings(
            value: value,
            maxDepth: depth);

    public static string ToJsonForOdata(this object value) =>
        jsonBroker.SerializeForOData(value: value);
}