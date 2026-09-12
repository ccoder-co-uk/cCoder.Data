// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Brokers;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Xunit;

namespace cCoder.Data.Tests.Brokers;

public sealed partial class JsonBrokerTests
{
    [Fact]
    public void ShouldCreateDefaultSerializerSettings()
    {
        // Given
        JsonBroker jsonBroker = new();

        // When
        JsonSerializerSettings jsonSerializerSettings =
            jsonBroker.GetJsonSerializerSettings();

        // Then
        jsonSerializerSettings.ReferenceLoopHandling.Should()
            .Be(expected: ReferenceLoopHandling.Ignore);

        jsonSerializerSettings.TypeNameHandling.Should()
            .Be(expected: TypeNameHandling.Objects);

        jsonSerializerSettings.NullValueHandling.Should()
            .Be(expected: NullValueHandling.Ignore);

        jsonSerializerSettings.DateTimeZoneHandling.Should()
            .Be(expected: DateTimeZoneHandling.Utc);

        jsonSerializerSettings.ContractResolver.Should()
            .BeOfType<DefaultContractResolver>()
            .Which.IgnoreSerializableAttribute.Should()
            .BeTrue();
    }

    [Fact]
    public void ShouldCreateODataSerializerSettings()
    {
        // Given
        JsonBroker jsonBroker = new();

        // When
        JsonSerializerSettings jsonSerializerSettings =
            jsonBroker.GetODataJsonSerializerSettings();

        // Then
        jsonSerializerSettings.TypeNameHandling.Should()
            .Be(expected: TypeNameHandling.None);

        jsonSerializerSettings.MaxDepth.Should()
            .Be(expected: 4);
    }

    [Fact]
    public void ShouldSerializeUsingProvidedSettings()
    {
        // Given
        JsonBroker jsonBroker = new();

        JsonSerializerSettings jsonSerializerSettings =
            jsonBroker.GetODataJsonSerializerSettings();

        object value = new
        {
            Name = "Data",
            Empty = (string)null,
        };

        // When
        string actualJson = jsonBroker.Serialize(
            value: value,
            jsonSerializerSettings: jsonSerializerSettings);

        // Then
        actualJson.Should()
            .Be(expected: "{\"Name\":\"Data\"}");
    }

    [Fact]
    public void ShouldSerializeUsingDefaultSettings()
    {
        // Given
        JsonBroker jsonBroker = new();

        object value = new
        {
            Name = "Data",
            Empty = (string)null,
        };

        // When
        string actualJson = jsonBroker.Serialize(value: value);

        // Then
        actualJson.Should()
            .Be(expected: "{\"Name\":\"Data\",\"Empty\":null}");
    }

    [Fact]
    public void ShouldDeserializeUsingProvidedSettings()
    {
        // Given
        JsonBroker jsonBroker = new();

        JsonSerializerSettings jsonSerializerSettings =
            jsonBroker.GetJsonSerializerSettings();

        string value = "{\"Name\":\"Data\"}";

        // When
        Dictionary<string, string> actualValue =
            jsonBroker.Deserialize<Dictionary<string, string>>(
                value: value,
                jsonSerializerSettings: jsonSerializerSettings);

        // Then
        actualValue.Should()
            .Contain(
                key: "Name",
                value: "Data");
    }
}