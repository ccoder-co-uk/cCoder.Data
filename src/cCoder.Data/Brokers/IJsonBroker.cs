// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using Newtonsoft.Json;

namespace cCoder.Data.Brokers;

internal interface IJsonBroker
{
    JsonSerializerSettings GetJsonSerializerSettings();
    JsonSerializerSettings GetODataJsonSerializerSettings();

    T Deserialize<T>(string value);

    T Deserialize<T>(
        string value,
        JsonSerializerSettings jsonSerializerSettings);

    string Serialize(object value);

    string SerializeUsingJsonSettings(object value);

    string SerializeUsingJsonSettings(object value, int maxDepth);

    string SerializeForOData(object value);

    string Serialize(
        object value,
        JsonSerializerSettings jsonSerializerSettings);
}