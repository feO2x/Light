using FluentAssertions;
using Light.Serialization.Json;
using Light.Serialization.Json.LowLevelWriting;

namespace Light.Serialization.Tests
{
    public abstract class BaseDefaultJsonSerializerTest
    {
        protected readonly ISerializer JsonSerializer;

        protected BaseDefaultJsonSerializerTest()
        {
            JsonSerializer = new JsonSerializerBuilder().Build();
        }

        protected void CompareJsonToExpected<T>(T value, string expected)
        {
            var json = JsonSerializer.Serialize(value);

            json.Should().Be(expected);
        }
    }
}