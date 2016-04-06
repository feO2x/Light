using System;
using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Light.Serialization.Tests
{
    public sealed class JsonSerializationMaxIndentTests : BaseJsonSerializerTest
    {
        public static readonly IEnumerable<object[]> Testdata =
            new[]
            {
                new object[]
                {
                    new Dictionary<string, DummyPerson>
                    {
                        {"Walter", new DummyPerson {Name = "Walter White", Age = 52}},
                        {"Jesse", new DummyPerson {Name = "Jesse Pinkman", Age = 27}}
                    },
                    "{\r\n  \"walter\": {\r\n    \"name\": \"Walter White\",\r\n    \"age\": 52\r\n  },\r\n  \"jesse\": {\r\n    \"name\": \"Jesse Pinkman\",\r\n    \"age\": 27\r\n  }\r\n}"
                }
            };

        [Theory(DisplayName = "Collection and dictionary serialization output can be switched to human readable json.")]
        [MemberData("Testdata")]
        public void CollectionsAndDictionariesAreSerializedToHumanReadableJsonCorrectly(IEnumerable enumerable,
            string expected)
        {
            var jsonSerializer = JsonSerializerBuilder.WithMaxIndent(1).Build();

            Action act = () => jsonSerializer.Serialize(enumerable);

            act.ShouldThrow<SerializationException>("Serializing Light.Serialization.Tests.DummyPerson would produce the indent of 2 which exceeds the maximal indent of 1.");
        }
    }
}