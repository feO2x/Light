using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FluentAssertions;
using Xunit;


namespace Light.Serialization.Tests
{
    public class JsonDictionaryDeserializationPreservationTests : BaseJsonDeserializerTest
    {
        private static readonly DummyPerson _person = new DummyPerson {Name = "Walter White", Age = 52};

        public static readonly IEnumerable<object[]> CollectionTestData =
            new[]
            {
                new object[]
                {
                    "[{\"$id\":0,\"name\":\"Walter White\",\"age\":52},{\"$ref\":0}]",
                    new ObservableCollection<DummyPerson>
                    {
                        _person,
                        _person
                    }
                }
            };

        [Theory]
        [MemberData("CollectionTestData")]
        public void CollectionsAreDeserializedCorrectly(string json, IEnumerable expected)
        {
            var actual = GetDeserializedJson<List<DummyPerson>>(json);
            actual.ShouldAllBeEquivalentTo(expected);
        }
    }
}
