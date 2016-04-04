using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FluentAssertions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.Serialization.Tests
{
    public class JsonDeserializationTestsWithoutPreservation : BaseJsonDeserializerTest
    {
        public static readonly IEnumerable<object[]> CollectionTestData =
            new[]
            {
                new object[]
                {
                    "[{\"name\":\"Walter White\",\"age\":52},{\"name\":\"Walter White\",\"age\":52}]",
                    new ObservableCollection<DummyPerson>
                    {
                        new DummyPerson {Name = "Walter White", Age = 52},
                        new DummyPerson {Name = "Walter White", Age = 52}
                    }
                }
            };

        [Theory(DisplayName = "The deserializer must be able to deserialize dictionaries containing strings without using object preservation.")]
        [MemberData("CollectionTestData")]
        public void CollectionsAreDeserializedCorrectly(string json, IEnumerable expected)
        {
            var actual = GetDeserializedJson<List<DummyPerson>>(json);
            actual.ShouldAllBeEquivalentTo(expected);
        }

        [Theory(DisplayName = "The deserializer must be able to deserialize dictionaries containing strings without using object preservation.")]
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
    }
}
