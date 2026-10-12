// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Dynamic;
using cCoder.Data.Models.CMS;
using cCoder.Data.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Xunit;

namespace cCoder.Data.Tests;

public sealed partial class AppConfigWorkflowRoundTripTests
{
    [Fact]
    public void ShouldPreserveAppConfigThroughTypedWorkflowContextRoundTrip()
    {
        // Given
        App app = new() { Id = 42, ConfigJson = "{\"Region\":\"EU\",\"Enabled\":true}" };
        Dictionary<string, object> variables = new() { ["App"] = app };

        JsonSerializerSettings settings = new()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            TypeNameHandling = TypeNameHandling.Objects,
            NullValueHandling = NullValueHandling.Ignore,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            ContractResolver = new DefaultContractResolver { IgnoreSerializableAttribute = true }
        };

        string json = JsonConvert.SerializeObject(value: variables, settings: settings);

        // When
        Dictionary<string, object> restored = JsonConvert.DeserializeObject<Dictionary<string, object>>(
            value: json,
            settings: settings);

        // Then
        App restoredApp = Assert.IsType<App>(@object: restored["App"]);
        IDictionary<string, object> restoredConfig = (IDictionary<string, object>)restoredApp.Config;
        Assert.Equal(expected: "EU", actual: restoredConfig["Region"]);
        Assert.Equal(expected: true, actual: restoredConfig["Enabled"]);
    }

    [Fact]
    public void ShouldSerializeExpandoConfigWithoutDynamicMethodDispatch()
    {
        // Given
        IDictionary<string, object> config = new ExpandoObject();
        config["Region"] = "EU";

        // When
        App app = new() { Config = config };

        // Then
        Assert.Equal(expected: ObjectExtensions.ToJson(value: config), actual: app.ConfigJson);
    }
}