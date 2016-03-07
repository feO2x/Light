using FluentAssertions;
using Light.Serialization.Json;

namespace Light.Serialization.Tests
{
    public abstract class BaseDefaultJsonSerializerTest
    {
        protected ISerializer JsonSerializer;

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