using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xunit;

namespace Light.Serialization.Tests
{
    public sealed class JsonCollectionSerializationTests : BaseJsonSerializerTest
    {
        public static readonly IEnumerable<object[]> CollectionTestData =
            new[]
            {
                new object[]
                {
                    new[] { 1, 2, 3 },
                    "[1,2,3]"
                },
                new object[]
                {
                    new List<string>
                    {
                        "Foo", null, "Bla"
                    },
                    "[\"Foo\",null,\"Bla\"]"
                },
                new object[]
                {
                    new ObservableCollection<DummyPerson>
                    {
                        new DummyPerson { Name = "Walter White", Age = 52 },
                        new DummyPerson { Name = "Jesse Pinkman", Age = 27 }
                    },
                    "[{\"$id\":0,\"name\":\"Walter White\",\"age\":52},{\"$id\":1,\"name\":\"Jesse Pinkman\",\"age\":27}]"
                }
            };

        [Theory]
        [MemberData("CollectionTestData")]
        public void CollectionsAreSerializedCorrectly(IEnumerable enumerable, string expected)
        {
            CompareJsonToExpected(enumerable, expected);
        }
    }
}