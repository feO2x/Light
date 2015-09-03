using FluentAssertions;
using Light.Serialization.Json;
using Xunit;

namespace Light.Serialization.Tests
{
    public sealed class JsonCharacterDeserializationTests
    {
        [Theory]
        [InlineData("\"c\"", 'c')]
        [InlineData("\"4\"", '4')]
        [InlineData("\"Y\"", 'Y')]
        [InlineData("\" \"", ' ')]
        public void SimpleCharacterCanBeDeserializedCorrectly(string json, char expected)
        {
            var testTarget = new JsonDeserializerBuilder().Build();
            var actual = testTarget.Deserialize<char>(json);
            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData("\"\\\\\"", '\\')]
        [InlineData("\"\"\"", '"')]
        [InlineData("\"\b\"", '\b')]
        [InlineData("\"\f\"", '\f')]
        [InlineData("\"\n\"", '\n')]
        [InlineData("\"\r\"", '\r')]
        [InlineData("\"\t\"", '\t')]
        [InlineData("\"\\/\"", '/')]
        public void EscapedCharactersCanBeDeserializedCorrectly(string json, char expected)
        {
            var testTarget = new JsonDeserializerBuilder().Build();
            var actual = testTarget.Deserialize<char>(json);
            actual.Should().Be(expected);
        }
    }
}
