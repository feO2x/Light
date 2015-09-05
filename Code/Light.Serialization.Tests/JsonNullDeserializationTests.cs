using FluentAssertions;
using Light.Serialization.Json;
using System;
using System.IO;
using Xunit;

namespace Light.Serialization.Tests
{
    public sealed class JsonNullDeserializationTests
    {
        [Fact]
        public void NullValuesAreParsedCorrectly()
        {
            NullValuesAreParsedCorrectly<object>(); // Check that it works for classes
            NullValuesAreParsedCorrectly<IComparable>(); // Check that it works for interfaces
            NullValuesAreParsedCorrectly<Stream>(); // Check that it works for abstract base classes
        }

        private static void NullValuesAreParsedCorrectly<T>()
        {
            const string json = "null";
            var testTarget = new JsonDeserializerBuilder().Build();
            var result = testTarget.Deserialize<T>(json);
            result.Should().BeNull();
        }

        [Theory]
        [InlineData("nll")]
        [InlineData("nlul")]
        [InlineData("nul")]
        public void ExceptionIsThrownWhenNullIsMisspelled(string json)
        {
            var testTarget = new JsonDeserializerBuilder().Build();
            Action act = () => testTarget.Deserialize<object>(json);
            act.ShouldThrow<DeserializationException>().And.Message.Should().Be($"Cannot deserialize value {json} to null");
        }
    }
}
