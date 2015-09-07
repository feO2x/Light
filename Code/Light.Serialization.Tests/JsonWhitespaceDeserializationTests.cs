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

        
    }
}
