using FluentAssertions;
using Xunit;

namespace Light.Serialization.Tests
{
    public sealed class ComplexTypeDeserializationTests : BaseJsonDeserializerTest
    {
        [Fact(DisplayName = "The deserializer must be able to deserialize a JSON document containing a complex object.")]
        public void ComplexObject()
        {
            const string json = "{\"$id\":0,\"x\": 42}";

            var result = GetDeserializedJson<DummyClass>(json);

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