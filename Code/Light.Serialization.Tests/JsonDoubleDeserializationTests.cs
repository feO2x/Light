using System;
using FluentAssertions;
using Light.Serialization.Json;
using Xunit;

namespace Light.Serialization.Tests
{
    public sealed class JsonDoubleDeserializationTests
    {
        [Theory]
        [InlineData("42.0", 42.0, 0.1)]
        [InlineData("0.75", 0.75, 0.01)]
        [InlineData("-146.311", -146.311, 0.001)]
        [InlineData("-1.7976931348623157", -1.7976931348623157, 0.0000000000000001)]
        [InlineData("32", 32.0, 0.1)]
        [InlineData("1.625e10", 1.625E10, 0.1)]
        [InlineData("3.141E-3", 3.141E-3, 0.1E-7)]
        public void DoubleValuesCanBeDeserializedCorrectly(string json, double expected, double tolerance)
        {
            var testTarget = new JsonDeserializerBuilder().Build();
            var actual = testTarget.Deserialize<double>(json);
            actual.Should().BeApproximately(expected, tolerance);
        }

        [Theory]
        [InlineData("42.0.")]
        [InlineData("3k.0")]
        [InlineData("3.141E1f")]
        public void ExceptionIsThrownWhenNumberCannotBeParsed(string json)
        {
            var testTarget = new JsonDeserializerBuilder().Build();
            Action act = () => testTarget.Deserialize<double>(json);
            act.ShouldThrow<DeserializationException>().And.Message.Should().Contain(($"Cannot deserialize value {json} to"));
        }
    }
}
