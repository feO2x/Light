using FluentAssertions;
using Xunit;

namespace Light.Serialization.Tests
{
    public sealed class JsonFloatDeserializationTests : BaseJsonDeserializerTest
    {
        [Theory (DisplayName = "JSON strings that represent floating point numbers are correctly parsed to float.")]
        [InlineData("3.402823e38", 3.402823e38f, 0.000001f)]
        [InlineData("2.402823e38", 2.402823e38f, 0.000001f)]
        [InlineData("2.0e38", 2.0e38f, 0.1f)]
        [InlineData("0.0", 0.0f, 0.1f)]
        [InlineData("-2.402823e38", -2.402823e38f, 0.000001f)]
        [InlineData("-3.402823e38", -3.402823e38f, 0.000001f)]
        [InlineData("-2.0e38", -2.0e38, 0.1f)]
        public void FloatValuesCanBeDeserializedCorrectly(string json, float expected, float tolerance)
        {
            var actual = GetDeserializedJson<float>(json);
            actual.Should().BeApproximately(expected, tolerance);
        }

        [Theory (DisplayName = "JSON strings that contain malformed floating point numbers or numbers that are to large for float result in an expection.")]
        [InlineData("42.0.")]
        [InlineData("3k.0")]
        [InlineData("3.141E1f")]
        [InlineData("-3.502823e38")]
        [InlineData("-3.402823e39")]
        [InlineData("-3.402824e38")]
        [InlineData("3.502823e38")]
        [InlineData("3.402825e38")]
        [InlineData("3.402823e39")]
        [InlineData("3.502823e38e")]


        public void ExceptionIsThrownWhenNumberCannotBeParsed(string json)
        {
            CheckDeserializerThrowsExceptionWithMessageContaining<float>(json, $"Cannot deserialize value {json} to");
        }

    }
}
