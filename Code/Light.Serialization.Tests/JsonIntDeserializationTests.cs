using Xunit;

namespace Light.Serialization.Tests
{
    public sealed class JsonIntDeserializationTests : BaseDefaultJsonDeserializationTest
    {
        [Theory]
        [InlineData("42", 42)]
        [InlineData("2147483647", 2147483647)]
        [InlineData("0", 0)]
        [InlineData("-420", -420)]
        public void IntValueCanBeDeserializedCorrectly(string json, int expected)
        {
            CompareDeserializedJsonToExpected(json, expected);
        }

        [Theory]
        [InlineData("2147483648")]
        [InlineData("-2147483649")]
        [InlineData("185000000000")]
        [InlineData("-375000000000")]
        public void ExceptionIsThrownWhenOverflowingIntValueIsDeserialized(string json)
        {
            CheckDeserializerThrowsExceptionWithMessage<int>(json, $"Could not deserialize value {json} because it produces an overflow for type int.");
        }

        [Theory]
        [InlineData("42.0", 42)]
        [InlineData("3.00", 3)]
        [InlineData("555.0000", 555)]
        [InlineData("0.000000", 0)]
        public void NumbersWithTrailingZerosAfterDecimalPointCanBeDeserialized(string json, int expected)
        {
            CompareDeserializedJsonToExpected(json, expected);
        }

        [Theory]
        [InlineData("42.7")]
        [InlineData("103.007")]
        [InlineData("50.0353")]
        [InlineData("0.00000856")]
        public void ExceptionIsThrownWhenNumbersWithNonZeroDigitsAfterDecimalPointIsDeserialized(string json)
        {
            CheckDeserializerThrowsExceptionWithMessage<int>(json, $"Could not deserialize value {json} because it is no integer, but a real number");
        }
    }
}
