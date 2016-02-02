using System;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.Serialization.Tests
{
    public sealed class JsonDateTimeSerializationTests : BaseDefaultJsonSerializerTest
    {
        public static readonly TestData Utc1TestData =
            new[]
            {
                new object[] { new DateTime(2016, 2, 2, 0, 0, 0, DateTimeKind.Utc), "\"2016-02-02T00:00:00Z\"" }
            };

        //https://en.wikipedia.org/wiki/ISO_8601
        [Theory(DisplayName = "UTC date times are serialized according to the ISO 8601 specification")]
        [MemberData(nameof(Utc1TestData))]
        public void Utc1(DateTime dateTime, string expectedJson)
        {
            CompareJsonToExpected(dateTime, expectedJson);
        }
    }
}