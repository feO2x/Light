using FluentAssertions;
using Light.Serialization.Json;
using Xunit;

namespace Light.Serialization.Tests
{
    public sealed class JsonPrimitiveTypesDeserializationTests
    {
        [Theory]
        [InlineData("42", 42)]
        [InlineData("0", 0)]
        [InlineData("-42", -42)]
        public void IntValueCanBeDeserializedCorrectly(string json, int value)
        {
            var testTarget = new JsonDeserializerBuilder().Build();

            var result = testTarget.Deserialize<int>(json);
            result.Should().Be(value);
        }
    }
}
