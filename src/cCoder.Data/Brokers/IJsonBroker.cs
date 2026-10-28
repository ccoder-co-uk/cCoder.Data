// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Text.Json;

namespace cCoder.Data.Brokers;

internal interface IJsonBroker
{
    JsonSerializerOptions GetJsonSerializerOptions();
    JsonSerializerOptions GetODataJsonSerializerOptions();

    T Deserialize<T>(string value);

    T Deserialize<T>(
        string value,
        JsonSerializerOptions jsonSerializerOptions);

    string Serialize(object value);

    string SerializeUsingJsonSettings(object value);

    string SerializeUsingJsonSettings(object value, int maxDepth);

    string SerializeForOData(object value);

    string Serialize(
        object value,
        JsonSerializerOptions jsonSerializerOptions);
}