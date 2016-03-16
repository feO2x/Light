using System.Collections.Generic;
using FluentAssertions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.Serialization.Tests
{
    public sealed class JsonDictionaryDeserializationTests : BaseJsonDeserializerTest
    {
        [Theory(DisplayName = "The deserializer must be able to deserialize dictionaries containing strings.")]
        [MemberData(nameof(DeserializeStringDictionariesData))]
        public void DeserializeStringDictionaries(string json, Dictionary<string, string> expected)
        {
            var actual = GetDeserializedJson<Dictionary<string, string>>(json);

            actual.ShouldAllBeEquivalentTo(expected);
        }

        public static readonly TestData DeserializeStringDictionariesData =
            new[]
            {
                new object[] { "{\"Hello\":\"World\"}", new Dictionary<string, string> { ["Hello"] = "World" } },
                new object[] { "{\"1\":\"Hey\",\"2\":\"Ho!\",\"3\":\"What?\"}", new Dictionary<string, string> { ["1"] = "Hey", ["2"] = "Ho!", ["3"] = "What?" } }
            };


        [Theory(DisplayName = "The deserializer must be able to deserialize dictiories when the requested type is a dictionary abstraction.")]
        [MemberData(nameof(UseAbstractionsData))]
        public void UseAbstractions(string json, IDictionary<string, string> expected)
        {
            ConfigureDefaultDomainFriendlyNaming();
            var actual = GetDeserializedJson<IDictionary<string, string>>(json);

            actual.ShouldAllBeEquivalentTo(expected);
        }

        public static readonly TestData UseAbstractionsData =
            new[]
            {
                new object[]
                {
                    "{\"$type\":{\"name\":\"genericMap\",\"typeArguments\":[\"string\",\"string\"]}, \"1\": \"Hello\", \"2\": \"World\"}",
                    new Dictionary<string, string> { ["1"] = "Hello", ["2"] = "World" }
                },
                new object[]
                {
                    "{\"$type\":{\"name\":\"genericMap\",\"typeArguments\":[\"string\",\"string\"]}, \"This\": \"That\", \"Here\": \"There\"}",
                    new Dictionary<string, string> { ["This"] = "That", ["Here"] = "There" }
                }
            };

        [Theory(DisplayName = "The deserializer must be able to deserialize dictionaries when the key is a primitive non-string type.")]
        [MemberData(nameof(NumericKeysData))]
        public void NumericKeys<T>(string json, IDictionary<T, object> expected, T sampleValueForTypeResolving)
        {
            ConfigureDefaultDomainFriendlyNaming();

            var actual = GetDeserializedJson<IDictionary<T, object>>(json);

            actual.ShouldAllBeEquivalentTo(expected);
        }

        public static readonly TestData NumericKeysData =
            new[]
            {
                new object[]
                {
                    "{\"$type\":{\"name\":\"genericMap\",\"typeArguments\":[\"int32\",\"object\"]}, \"1\": \"Hello\", \"2\": \"World\"}",
                    new Dictionary<int, object> {[1] = "Hello", [2] = "World"}, 1 
                },
                // TODO: more tests for other integer types, double, float, characters, and enum values
            };
    }
}