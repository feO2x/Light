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
                }
            };
    }
}