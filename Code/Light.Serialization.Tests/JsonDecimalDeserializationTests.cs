using FluentAssertions;
using Light.Serialization.Json;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.Serialization.Tests
{
    public sealed class JsonDecimalDeserializationTests : BaseDefaultJsonDeserializationTest
    {
        [Theory(DisplayName = "JSON strings that represent floating point numbers are correctly parsed to decimal.")]
        [MemberData("DecimalTestData")]
        public void DecimalValuesCanBeDeserializedCorrectly(string json, decimal expected, decimal tolerance)
        {
            var testTarget = new JsonDeserializerBuilder().Build();
            var actual = testTarget.Deserialize<decimal>(json);
            actual.Should().BeApproximately((decimal)expected, tolerance);
        }

        public static readonly TestData DecimalTestData = new[]
                                                  {
                                                              new object[] { "42.0", 42m, 1 },
                                                              new object[] { "42.01", 42.01m, 0.01 },
                                                              new object[] { "42.0001", 42.0001m, 0.0001 },
                                                              new object[] { "79228162514264337593543950335.0", decimal.MaxValue, 0.1},
                                                              new object[] { "-79228162514264337593543950335.0", decimal.MinValue, 0.1 },
                                                              new object[] { "-42.00200", - 42.00200m, 0.00001 }
                                                          };
        [Theory(DisplayName = "JSON strings that contain malformed floating point numbers or numbers that are to large for decimal result in an expection.")]
        [InlineData("42.0.")]
        [InlineData("42.0m")]
        [InlineData("3k.0")]
        [InlineData("3.141e1")]
        [InlineData("79228162514264337593543950336.0")]
        [InlineData("-79228162514264337593543950336.0")]

        public void ExceptionIsThrownWhenNumberCannotBeParsed(string json)
        {
            CheckDeserializerThrowsExceptionWithMessageContaining<decimal>(json, $"Cannot deserialize value {json} to");
        }
    }
}
