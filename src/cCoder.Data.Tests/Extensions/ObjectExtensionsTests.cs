// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Extensions;
using FluentAssertions;
using System.Reflection;
using Xunit;

namespace cCoder.Data.Tests.Extensions;

public sealed partial class ObjectExtensionsTests
{
    [Fact]
    public void PublicMethods_WhenInspected_ShouldNotExposeJsonSerializerSettings()
    {
        // Given

        const string externalTypeName =
            "Newtonsoft.Json.JsonSerializerSettings";

        // When

        MethodInfo[] publicMethods = typeof(ObjectExtensions)
            .GetMethods(bindingAttr:
                BindingFlags.Public
                | BindingFlags.Static
                | BindingFlags.DeclaredOnly);

        // Then

        publicMethods
            .Should()
            .OnlyContain(predicate: method =>
            method.ReturnType.FullName != externalTypeName
            && method
                .GetParameters()
                .All(predicate: parameter =>
                parameter.ParameterType.FullName != externalTypeName));
    }

    [Fact]
    public void ToJson_WhenCalled_ShouldUseBrokeredDefaultSettings()
    {
        // Given

        object value = new
        {
            Name = "Data",
            Empty = (string)null
        };

        // When

        string actualJson = value.ToJson();

        // Then

        actualJson
            .Should()
            .Contain(expected: "\"Name\":\"Data\"");

        actualJson
            .Should()
            .NotContain(unexpected: "\"Empty\"");

        actualJson
            .Should()
            .Contain(expected: "\"$type\"");
    }

    [Fact]
    public void ToJsonForOdata_WhenCalled_ShouldUseBrokeredODataSettings()
    {
        // Given

        object value = new
        {
            Name = "Data",
            Empty = (string)null
        };

        // When

        string actualJson = value.ToJsonForOdata();

        // Then

        actualJson
            .Should()
            .Be(expected: "{\"Name\":\"Data\"}");
    }
}