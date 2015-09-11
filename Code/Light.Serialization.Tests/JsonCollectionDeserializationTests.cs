using System.Collections.Generic;
using FluentAssertions;
using Light.Serialization.Json;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.Serialization.Tests
{
    public sealed class JsonCollectionDeserializationTests
    {
        [Theory]
        [MemberData(nameof(IntegerCollections))]
        public void IntegerCollectionsCanBeDeserialized(string json, int[] expected)
        {
            var testTarget = new JsonDeserializerBuilder().Build();
            var actual = testTarget.Deserialize<IList<int>>(json);
            actual.ShouldAllBeEquivalentTo(expected);
        }

        public static readonly TestData IntegerCollections =
            new[]
            {
                new object[] { "[1,2,3]", new[] { 1, 2, 3 } },
                new object[] { "[9,83]", new[] { 9, 83 } }
            };
    }
}