using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Light.Serialization.Tests
{
    public class JsonSerializationWithoutPreservationTests : BaseJsonSerializerTest
    {
            public static readonly IEnumerable<object[]> CollectionTestData =
                new[]
                {
                    new object[]
                    {
                        new ObservableCollection<DummyPerson>
                        {
                            new DummyPerson { Name = "Walter White", Age = 52 },
                            new DummyPerson { Name = "Jesse Pinkman", Age = 27 }
                        },
                        "[{\"name\":\"Walter White\",\"age\":52},{\"name\":\"Jesse Pinkman\",\"age\":27}]"
                    }
                };

        [Theory(DisplayName = "The deserializer must be able to deserialize complex objects in a collection without preservation.")]
        [MemberData("CollectionTestData")]
        public void CollectionsAreSerializedCorrectly(IEnumerable enumerable, string expected)
        {
            CompareJsonToExpectedWithoutPreservation(enumerable, expected);
        }

        [Theory]
        [MemberData("DictionaryTestData")]
        public void DictionariesMustBeSerializedCorrectly(object dictionary, string expected)
        {
            CompareJsonToExpectedWithoutPreservation(dictionary, expected);
        }

        public static readonly IEnumerable<object[]> DictionaryTestData =
            new[]
            {
                new object[]
                {
                    new Dictionary<int, int> { { 1, 87 }, { 2, 88 }, { 3, 89 } },
                    "{\"1\":87,\"2\":88,\"3\":89}"
                },
                new object[]
                {
                    new Dictionary<string, DummyPerson>
                    {
                        { "Walter", new DummyPerson { Name = "Walter White", Age = 52 } },
                        { "Jesse", new DummyPerson { Name = "Jesse Pinkman", Age = 27 } }
                    },
                    "{\"walter\":{\"name\":\"Walter White\",\"age\":52},\"jesse\":{\"name\":\"Jesse Pinkman\",\"age\":27}}"
                },
                new object[]
                {
                    new Dictionary<DummyPerson, string>
                    {
                        { new DummyPerson { Name = "Foo", Age = 34 }, "Bla" },
                        { new DummyPerson { Name = "Blubb", Age = 55 }, "Xoi" }
                    },
                    string.Format("{{\"{0}\":\"Bla\",\"{0}\":\"Xoi\"}}",
                                  typeof (DummyPerson).FullName)
                }
            };
    }
}