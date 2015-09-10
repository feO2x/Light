using Xunit;

namespace Light.Serialization.Tests
{
    public sealed class JsonBooleanDeserializationTests : BaseDefaultJsonDeserializationTest
    {
        [Theory]
        [InlineData("true", true)]
        [InlineData("false", false)]
        public void BooleanValuesCanBeDeserializedCorrectly(string json, bool expected)
        {
            CompareDeserializedJsonToExpected(json, expected);
        }

        [Theory]
        [InlineData("treu")]
        [InlineData("teu")]
        [InlineData("tru")]
        [InlineData("tu")]
        [InlineData("tr")]
        [InlineData("fasle")]
        [InlineData("flse")]
        [InlineData("fse")]
        [InlineData("fa")]
        public void ExceptionIsThrownWhenFalseOrTrueAreNotSpelledCorrectly(string json)
        {
            CheckDeserializerThrowsExceptionWithMessageContaining<bool>(json, $"Cannot deserialize value {json} to");
        }
    }
}
