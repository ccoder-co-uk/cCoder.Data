// ---------------------------------------------------------------
// Copyright (c) Paul.Ward@ccoder.co.uk
// ---------------------------------------------------------------

using cCoder.Data.Extensions;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Xunit;

namespace cCoder.Data.Tests.Extensions;

public sealed partial class TypeExtensionsTests
{
    [Fact]
    public void GetIdProperty_WhenTypeHasConventionalId_ReturnsIdProperty()
    {
        // Given
        Type modelType = typeof(ModelWithId);

        // When
        PropertyInfo actualProperty = modelType.GetIdProperty();

        // Then
        actualProperty.Should()
            .BeSameAs(expected: modelType.GetProperty(name: nameof(ModelWithId.Id)));
    }

    [Fact]
    public void GetIdProperty_WhenTypeHasKeyAttribute_ReturnsAttributedProperty()
    {
        // Given
        Type modelType = typeof(ModelWithKey);

        // When
        PropertyInfo actualProperty = modelType.GetIdProperty();

        // Then
        actualProperty.Should()
            .BeSameAs(expected: modelType.GetProperty(name: nameof(ModelWithKey.Identifier)));
    }

    [Fact]
    public void GetIdProperty_WhenTypeIsJoinType_ReturnsCompositeProperty()
    {
        // Given
        Type modelType = typeof(JoinModel);

        // When
        PropertyInfo actualProperty = modelType.GetIdProperty();

        // Then
        actualProperty.Should()
            .BeOfType<CompositePropertyInfo>();
    }

    [Table(name: "JoinModels")]
    private sealed class JoinModel
    {
        [ForeignKey(name: "First")]
        public int FirstId { get; set; }

        [ForeignKey(name: "Second")]
        public int SecondId { get; set; }

        [ForeignKey(name: "Third")]
        public int ThirdId { get; set; }

        [ForeignKey(name: "Fourth")]
        public int FourthId { get; set; }
    }

    private sealed class ModelWithId
    {
        public Guid Id { get; set; }
    }

    private sealed class ModelWithKey
    {
        [Key]
        public string Identifier { get; set; }
    }
}