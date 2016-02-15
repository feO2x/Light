using FluentAssertions;
using Light.Serialization.Json;
using Xunit;

namespace Light.Serialization.Tests
{
    public sealed class ComplexTypeDeserializationTests
    {
        [Fact(DisplayName = "The deserializer must be able to deserialize a JSON document containing a complex object.")]
        public void ComplexObject()
        {
            const string json = "{\"x\": 42}";
            var deserializer = new JsonDeserializerBuilder().Build();

            var result = deserializer.Deserialize<DummyClass>(json);

            var expected = new DummyClass(42);
            result.ShouldBeEquivalentTo(expected);
        }

        public class DummyClass
        {
            public DummyClass(int x)
            {
                X = x;
            }

            public int X { get; }
        }
    }
}