using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FluentAssertions;
using Xunit;


namespace Light.Serialization.Tests
{
    public class JsonDeserializationPreservationTests : BaseJsonDeserializerTest
    {
        public static readonly IEnumerable<object[]> CollectionTestData =
            new[]
            {
                new object[]
                {
                    "[{\"$id\":0,\"name\":\"Walter White\",\"age\":52},{\"$ref\":0}]",
                    new ObservableCollection<DummyPerson>
                    {
                        new DummyPerson {Name = "Walter White", Age = 52},
                        new DummyPerson {Name = "Walter White", Age = 52}
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
