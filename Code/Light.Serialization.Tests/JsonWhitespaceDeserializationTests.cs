using FluentAssertions;
using Light.Serialization.Json;
using Xunit;

namespace Light.Serialization.Tests
{
    public sealed class JsonWhitespaceDeserializationTests
    {
        [Theory]
        [InlineData(" 42", 42)]
        [InlineData("  false", false)]
        [InlineData("   true", true)]
        [InlineData(" 0.8863", 0.8863)]
        [InlineData("      \"Hello\"", "Hello")]
        [InlineData("   null", null)]
        public void SpacesBeforeSingleValueIsIgnoredCorrectly<T>(string json, T expected)
        {
            var testTarget = new JsonDeserializerBuilder().Build();
            var actual = testTarget.Deserialize<T>(json);
            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData("-42 ", -42)]
        [InlineData("true  ", true)]
        [InlineData("false    ", false)]
        [InlineData("3255.25 ", 3255.25)]
        [InlineData("\"Hi there\"  ", "Hi there")]
        [InlineData("null ", null)]
        public void SpacesAfterSingleValueAreIgnoredCorrectly<T>(string json, T expected)
        {
            var testTarget = new JsonDeserializerBuilder().Build();
            var actual = testTarget.Deserialize<T>(json);
            actual.Should().Be(expected);
        }
    }
}
