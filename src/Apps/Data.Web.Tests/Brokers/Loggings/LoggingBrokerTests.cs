// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using Data.Web.Brokers.Loggings;
using FluentAssertions;
using Xunit;

namespace Data.Web.Tests.Brokers.Loggings;

public sealed partial class LoggingBrokerTests
{
    [Fact]
    public void LoggingBroker_WhenUtilityMarkerIsResolved_UsesRuntimeContractsAssembly()
    {
        // Given
        Type loggingBrokerType = typeof(LoggingBroker);

        // When
        Type utilityBrokerInterface = loggingBrokerType
            .GetInterfaces()
            .Single(predicate: interfaceType =>
                interfaceType.Name == "IUtilityBroker");

        string markerAssemblyName = utilityBrokerInterface.Assembly
            .GetName()
            .Name;

        // Then
        markerAssemblyName.Should()
            .Be(expected: "cCoder.CodeAnalysis.Contracts");
    }

    [Fact]
    public void DataWeb_WhenRuntimeOutputIsBuilt_ContainsContractsWithoutAnalyzerRuntime()
    {
        // Given
        string runtimeOutputDirectory = Path.GetDirectoryName(
            path: typeof(LoggingBroker).Assembly.Location);

        string contractsAssemblyPath = Path.Combine(
            path1: runtimeOutputDirectory,
            path2: "cCoder.CodeAnalysis.Contracts.dll");

        string analyzerAssemblyPath = Path.Combine(
            path1: runtimeOutputDirectory,
            path2: "cCoder.CodeAnalysis.dll");

        // When
        bool contractsAssemblyExists = File.Exists(path: contractsAssemblyPath);
        bool analyzerAssemblyExists = File.Exists(path: analyzerAssemblyPath);

        // Then
        contractsAssemblyExists.Should()
            .BeTrue();

        analyzerAssemblyExists.Should()
            .BeFalse();
    }
}