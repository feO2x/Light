using Light.Core;
using System;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.Serialization.Tests
{
    public sealed class JsonPrimitiveTypesSerializationTests : BaseDefaultJsonSerializerTest
    {
        [Theory]
        [InlineData(42)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        [InlineData(42L)]
        [InlineData(long.MaxValue)]
        [InlineData(long.MinValue)]
        [InlineData(short.MaxValue)]
        [InlineData(short.MinValue)]
        [InlineData(sbyte.MaxValue)]
        [InlineData(sbyte.MinValue)]
        [InlineData(uint.MaxValue)]
        [InlineData(uint.MinValue)]
        [InlineData(ulong.MaxValue)]
        [InlineData(ulong.MinValue)]
        [InlineData(ushort.MaxValue)]
        [InlineData(ushort.MinValue)]
        [InlineData(byte.MaxValue)]
        [InlineData(byte.MinValue)]
        public void IntegerNumbersMustBeSerializedCorrectly<T>(T number)
        {
            CompareJsonToExpected(number, number.ToString());
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void BooleansMustBeSerializedCorrectly(bool value)
        {
            CompareJsonToExpected(value, value.ToString().ToLower());
        }

        [Fact]
        public void GuidsMustBeSerializedCorrectly()
        {
            var guid = Guid.NewGuid();

            CompareJsonToExpected(guid, guid.ToString().SurroundWithQuotationMarks());
        }

        [Theory]
        [InlineData(42.0, "42.0")]
        [InlineData(42.01, "42.01")]
        [InlineData(42.0001, "42.0001")]
        [InlineData(double.MaxValue, "1.7976931348623157E+308")]
        [InlineData(double.MinValue, "-1.7976931348623157E+308")]
        [InlineData(double.Epsilon, "4.94065645841247E-324")]
        [InlineData(double.NegativeInfinity, "\"-Infinity\"")]
        [InlineData(double.PositiveInfinity, "\"Infinity\"")]
        [InlineData(double.NaN, "\"NaN\"")]
        [InlineData(-42.00200000, "-42.002")]
        public void DoubleValuesMustBeSerializedCorrectly(double value, string expected)
        {
            CompareJsonToExpected(value, expected);
        }

        [Theory]
        [InlineData(42.0f, "42.0")]
        [InlineData(42.01f, "42.01")]
        [InlineData(42.0001f, "42.0001")]
        [InlineData(float.MaxValue, "3.40282347E+38")]
        [InlineData(float.MinValue, "-3.40282347E+38")]
        [InlineData(float.Epsilon, "1.401298E-45")]
        [InlineData(float.NegativeInfinity, "\"-Infinity\"")]
        [InlineData(float.PositiveInfinity, "\"Infinity\"")]
        [InlineData(float.NaN, "\"NaN\"")]
        [InlineData(-42.00200f, "-42.002")]
        public void FloatsMustBeSerializedCorrectly(float value, string expected)
        {
            CompareJsonToExpected(value, expected);
        }

        [Theory]
        [MemberData("DecimalTestData")]
        public void DecimalsMustBeSerializedCorrectly(decimal value, string expected)
        {
            CompareJsonToExpected(value, expected);
        }

        public static readonly TestData DecimalTestData = new[]
                                                          {
                                                              new object[] { 42m, "42.0" },
                                                              new object[] { 42.01m, "42.01" },
                                                              new object[] { 42.0001m, "42.0001" },
                                                              new object[] { decimal.MaxValue, "79228162514264337593543950335.0" },
                                                              new object[] { decimal.MinValue, "-79228162514264337593543950335.0" },
                                                              new object[] { -42.00200m, "-42.00200" }
                                                          };

        [Theory]
        [InlineData('a', "\"a\"")]
        [InlineData('b', "\"b\"")]
        [InlineData(char.MinValue, "\"\\u0000\"")]
        [InlineData('\t', @"""\t""")]
        [InlineData('\n', @"""\n""")]
        [InlineData('\r', @"""\r""")]
        [InlineData('\f', @"""\f""")]
        [InlineData('\b', @"""\b""")]
        [InlineData('\\', @"""\\""")]
        [InlineData('\u0085', @"""\u0085""")]  // Next Line Character
        [InlineData('\u2028', @"""\u2028""")]  // Line Separator Character
        [InlineData('\u2029', @"""\u2029""")]  // Paragraph Separator
        public void ChararctersMustBeSerializedCorrectly(char value, string expected)
        {
            CompareJsonToExpected(value, expected);
        }

        [Theory]
        [InlineData("a", "\"a\"")]
        [InlineData("Foo", "\"Foo\"")]
        [InlineData("It is\r\na good day, \tmy \"sunshine\"", @"""It is\r\na good day, \tmy \""sunshine\""""")]
        [InlineData("Here is the\u0085next line", @"""Here is the\u0085next line""")]
        public void StringsMustBeSerializedCorrectly(string @string, string expected)
        {
            CompareJsonToExpected(@string, expected);
        }

        [Fact]
        public void EnumerationsMustBeSerializedCorrectly()
        {
            const ConsoleColor enumValue = ConsoleColor.Black;

            CompareJsonToExpected(enumValue, enumValue.ToString().SurroundWithQuotationMarks());
        }
    }
}