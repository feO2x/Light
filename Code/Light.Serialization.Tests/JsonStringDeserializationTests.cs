using FluentAssertions;
using Light.Serialization.Json;
using Xunit;

namespace Light.Serialization.Tests
{
    public sealed class JsonStringDeserializationTests
    {
        [Theory]
        [InlineData("\"Hello\"", "Hello")]
        [InlineData("\"World\"", "World")]
        [InlineData("\"2\"", "2")]
        [InlineData("\"\"", "")]
        public void SimpleStringCanBeDeserializedCorrectly(string json, string expected)
        {
            var testTarget = new JsonDeserializerBuilder().Build();
            var actual = testTarget.Deserialize<string>(json);
            actual.Should().Be(expected);
        }

        // TODO: add more complicated cases for strings
    }
}
