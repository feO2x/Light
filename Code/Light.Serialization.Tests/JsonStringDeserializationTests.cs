using Xunit;

namespace Light.Serialization.Tests
{
    public sealed class JsonStringDeserializationTests : BaseDefaultJsonDeserializationTest
    {
        [Theory]
        [InlineData("\"Hello\"", "Hello")]
        [InlineData("\"World\"", "World")]
        [InlineData("\"2\"", "2")]
        [InlineData("\"\"", "")]
        public void SimpleStringCanBeDeserializedCorrectly(string json, string expected)
        {
            CompareDeserializedJsonToExpected(json, expected);
        }

        // TODO: add more complicated cases for strings
    }
}
