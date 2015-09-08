using System;
using FluentAssertions;
using Light.Serialization.Json;
using Xunit;

namespace Light.Serialization.Tests
{
    public sealed class JsonBooleanDeserializationTests
    {
        [Theory]
        [InlineData("true", true)]
        [InlineData("false", false)]
        public void BooleanValuesCanBeDeserializedCorrectly(string json, bool expected)
        {
            var testTarget = new JsonDeserializerBuilder().Build();
            var actual = testTarget.Deserialize<bool>(json);
            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData("treu")]
        [InlineData("teu")]
        [InlineData("tru")]
        [InlineData("tu")]
        [InlineData("tr")]
        [InlineData("fasle")]
        [InlineData("flse")]
        [InlineData("fse")]
        [InlineData("fa")]
        public void ExceptionIsThrownWhenFalseOrTrueAreNotSpelledCorrectly(string json)
        {
            var testTarget = new JsonDeserializerBuilder().Build();
            Action act = () => testTarget.Deserialize<bool>(json);
            act.ShouldThrow<DeserializationException>().And.Message.Should().Contain($"Cannot deserialize value {json} to");
        }
    }
}
