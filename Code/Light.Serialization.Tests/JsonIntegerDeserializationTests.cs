using Xunit;

namespace Light.Serialization.Tests
{
    public sealed class JsonIntDeserializationTests : BaseDefaultJsonDeserializationTest
    {
        [Theory]
        [InlineData("42", 42)]
        [InlineData(int.MaxValue / 2, int.MaxValue / 2)]
        [InlineData("1247483647", 1247483647)]
        [InlineData(int.MaxValue, int.MaxValue)]
        [InlineData(int.MinValue, int.MinValue)]
        [InlineData("0", 0)]
        [InlineData(int.MinValue + int.MaxValue / 2, int.MinValue + int.MaxValue / 2)]
        public void IntValueCanBeDeserializedCorrectly(string json, int expected)
        {
            CompareDeserializedJsonToExpected(json, expected);
        }

        [Theory]
        [InlineData("2147483648")] //int max value ( 2147483647 ) +1
        [InlineData("-2147483658")]
        [InlineData("2247483647")]
        [InlineData("3147483647")]
        [InlineData("214748364700000000")]
        public void ExceptionIsThrownWhenOverflowingIntValueIsDeserialized(string json)
        {
            CheckDeserializerThrowsExceptionWithMessage<int>(json, $"Could not deserialize value {json} because it produces an overflow for type System.Int32.");
        }

        [Theory]
        [InlineData("42.0", 42)]
        [InlineData("1247483647.000", 1247483647)]
        [InlineData("2147483647.0000", int.MaxValue)]
        [InlineData("-2147483648.00000", int.MinValue)]
        [InlineData("0.0", 0)]
        public void NumbersWithTrailingZerosAfterDecimalPointCanBeDeserialized(string json, int expected)
        {
            CompareDeserializedJsonToExpected(json, expected);
        }

        [Theory]
        [InlineData("42.7")]
        [InlineData("1247483647.007")]
        [InlineData("2147483647.0353")]
        [InlineData("0.00000856")]
        [InlineData("-2147483648.000000001")]
        public void ExceptionIsThrownWhenNumbersWithNonZeroDigitsAfterDecimalPointIsDeserialized(string json)
        {
            CheckDeserializerThrowsExceptionWithMessage<int>(json, $"Could not deserialize value {json} because it is no integer, but a real number.");
        }
    }

    public sealed class JsonUIntDeserializationTests : BaseDefaultJsonDeserializationTest
    {
        [Theory]
        [InlineData("42", 42u)]
        [InlineData(uint.MaxValue / 2, uint.MaxValue / 2)]
        [InlineData("3394967295", 3394967295u)]
        [InlineData(uint.MaxValue, uint.MaxValue)]
        [InlineData(uint.MinValue, uint.MinValue)]
        public void IntValueCanBeDeserializedCorrectly(string json, uint expected)
        {
            CompareDeserializedJsonToExpected(json, expected);
        }

        [Theory]
        [InlineData("4294967296")] //uint max value ( 4294967295 ) +1
        [InlineData("-1")]
        [InlineData("4394967295")]
        [InlineData("5294967295")]
        [InlineData("429496729500000000")]
        public void ExceptionIsThrownWhenOverflowingIntValueIsDeserialized(string json)
        {
            CheckDeserializerThrowsExceptionWithMessage<uint>(json, $"Could not deserialize value {json} because it produces an overflow for type System.UInt32.");
        }

        [Theory]
        [InlineData("42.0", 42u)]
        [InlineData("3394967295.000", 3394967295u)]
        [InlineData("4294967295.0000", uint.MaxValue)]
        [InlineData("0.00000", uint.MinValue)]
        public void NumbersWithTrailingZerosAfterDecimalPointCanBeDeserialized(string json, uint expected)
        {
            CompareDeserializedJsonToExpected(json, expected);
        }

        [Theory]
        [InlineData("42.7")]
        [InlineData("3394967295.007")]
        [InlineData("4294967295.0353")]
        [InlineData("0.00000856")]
        [InlineData("0.000000001")]
        public void ExceptionIsThrownWhenNumbersWithNonZeroDigitsAfterDecimalPointIsDeserialized(string json)
        {
            CheckDeserializerThrowsExceptionWithMessage<int>(json, $"Could not deserialize value {json} because it is no integer, but a real number.");
        }
    }

    public sealed class JsonShortDeserializationTests : BaseDefaultJsonDeserializationTest
    {
        [Theory]
        [InlineData("42", 42)]
        [InlineData(short.MaxValue / 2, short.MaxValue / 2)]
        [InlineData("23767", 23767)]
        [InlineData(short.MaxValue, short.MaxValue)]
        [InlineData(short.MinValue, short.MinValue)]
        [InlineData("0", 0)]
        [InlineData(short.MinValue + short.MaxValue / 2, short.MinValue + short.MaxValue / 2)]
        public void IntValueCanBeDeserializedCorrectly(string json, short expected)
        {
            CompareDeserializedJsonToExpected(json, expected);
        }

        [Theory]
        [InlineData("32768")] //short max value ( 32767 ) +1
        [InlineData("-32778")]
        [InlineData("33767")]
        [InlineData("42767")]
        [InlineData("3276700000000")]
        public void ExceptionIsThrownWhenOverflowingIntValueIsDeserialized(string json)
        {
            CheckDeserializerThrowsExceptionWithMessage<short>(json, $"Could not deserialize value {json} because it produces an overflow for type System.Int16.");
        }

        [Theory]
        [InlineData("42.0", 42)]
        [InlineData("23767.000", 23767)]
        [InlineData("32767.0000", short.MaxValue)]
        [InlineData("-32768.00000", short.MinValue)]
        [InlineData("0.0", 0)]
        public void NumbersWithTrailingZerosAfterDecimalPointCanBeDeserialized(string json, short expected)
        {
            CompareDeserializedJsonToExpected(json, expected);
        }

        [Theory]
        [InlineData("42.7")]
        [InlineData("23767.007")]
        [InlineData("32767.0353")]
        [InlineData("0.00000856")]
        [InlineData("-32768.000000001")]
        public void ExceptionIsThrownWhenNumbersWithNonZeroDigitsAfterDecimalPointIsDeserialized(string json)
        {
            CheckDeserializerThrowsExceptionWithMessage<int>(json, $"Could not deserialize value {json} because it is no integer, but a real number.");
        }
    }

    public sealed class JsonUShortDeserializationTests : BaseDefaultJsonDeserializationTest
    {
        [Theory]
        [InlineData("42", 42)]
        [InlineData(ushort.MaxValue / 2, ushort.MaxValue / 2)]
        [InlineData("56535", 56535)]
        [InlineData(ushort.MaxValue, ushort.MaxValue)]
        [InlineData(ushort.MinValue, ushort.MinValue)]
        [InlineData("0", 0)]
        [InlineData(ushort.MinValue + ushort.MaxValue / 2, ushort.MinValue + ushort.MaxValue / 2)]
        public void IntValueCanBeDeserializedCorrectly(string json, ushort expected)
        {
            CompareDeserializedJsonToExpected(json, expected);
        }

        [Theory]
        [InlineData("65536")] //ushort max value ( 65535 ) +1
        [InlineData("-1")]
        [InlineData("66535")]
        [InlineData("75535")]
        [InlineData("6553500000000")]
        public void ExceptionIsThrownWhenOverflowingIntValueIsDeserialized(string json)
        {
            CheckDeserializerThrowsExceptionWithMessage<ushort>(json, $"Could not deserialize value {json} because it produces an overflow for type System.UInt16.");
        }

        [Theory]
        [InlineData("42.0", 42)]
        [InlineData("56535.000", 56535)]
        [InlineData("65535.0000", ushort.MaxValue)]
        [InlineData("0.00000", ushort.MinValue)]
        [InlineData("0.0", 0)]
        public void NumbersWithTrailingZerosAfterDecimalPointCanBeDeserialized(string json, ushort expected)
        {
            CompareDeserializedJsonToExpected(json, expected);
        }

        [Theory]
        [InlineData("42.7")]
        [InlineData("56535.007")]
        [InlineData("65535.0353")]
        [InlineData("0.00000856")]
        [InlineData("0.000000001")]
        public void ExceptionIsThrownWhenNumbersWithNonZeroDigitsAfterDecimalPointIsDeserialized(string json)
        {
            CheckDeserializerThrowsExceptionWithMessage<int>(json, $"Could not deserialize value {json} because it is no integer, but a real number.");
        }
    }

    public sealed class JsonByteDeserializationTests : BaseDefaultJsonDeserializationTest
    {
        [Theory]
        [InlineData("42", 42)]
        [InlineData(byte.MaxValue / 2, byte.MaxValue / 2)]
        [InlineData("165", 165)]
        [InlineData(byte.MaxValue, byte.MaxValue)]
        [InlineData(byte.MinValue, byte.MinValue)]
        [InlineData("0", 0)]
        [InlineData(byte.MinValue + byte.MaxValue / 2, byte.MinValue + byte.MaxValue / 2)]
        public void IntValueCanBeDeserializedCorrectly(string json, byte expected)
        {
            CompareDeserializedJsonToExpected(json, expected);
        }

        [Theory]
        [InlineData("256")] //byte max value ( 255 ) +1
        [InlineData("-1")]
        [InlineData("265")]
        [InlineData("355")]
        [InlineData("25500000000")]
        public void ExceptionIsThrownWhenOverflowingIntValueIsDeserialized(string json)
        {
            CheckDeserializerThrowsExceptionWithMessage<byte>(json, $"Could not deserialize value {json} because it produces an overflow for type System.Byte.");
        }

        [Theory]
        [InlineData("42.0", 42)]
        [InlineData("165.000", 165)]
        [InlineData("255.0000", byte.MaxValue)]
        [InlineData("0.00000", byte.MinValue)]
        [InlineData("0.0", 0)]
        public void NumbersWithTrailingZerosAfterDecimalPointCanBeDeserialized(string json, byte expected)
        {
            CompareDeserializedJsonToExpected(json, expected);
        }

        [Theory]
        [InlineData("42.7")]
        [InlineData("165.007")]
        [InlineData("255.0353")]
        [InlineData("0.00000856")]
        [InlineData("0.000000001")]
        public void ExceptionIsThrownWhenNumbersWithNonZeroDigitsAfterDecimalPointIsDeserialized(string json)
        {
            CheckDeserializerThrowsExceptionWithMessage<int>(json, $"Could not deserialize value {json} because it is no integer, but a real number.");
        }
    }

    public sealed class JsonSByteDeserializationTests : BaseDefaultJsonDeserializationTest
    {
        [Theory]
        [InlineData("42", 42)]
        [InlineData(sbyte.MaxValue / 2, sbyte.MaxValue / 2)]
        [InlineData("37", 37)]
        [InlineData(sbyte.MaxValue, sbyte.MaxValue)]
        [InlineData(sbyte.MinValue, sbyte.MinValue)]
        [InlineData("0", 0)]
        [InlineData(sbyte.MinValue + sbyte.MaxValue / 2, sbyte.MinValue + sbyte.MaxValue / 2)]
        public void IntValueCanBeDeserializedCorrectly(string json, sbyte expected)
        {
            CompareDeserializedJsonToExpected(json, expected);
        }

        [Theory]
        [InlineData("128")] //sbyte max value ( 127 ) +1
        [InlineData("-138")]
        [InlineData("137")]
        [InlineData("227")]
        [InlineData("12700000000")]
        public void ExceptionIsThrownWhenOverflowingIntValueIsDeserialized(string json)
        {
            CheckDeserializerThrowsExceptionWithMessage<sbyte>(json, $"Could not deserialize value {json} because it produces an overflow for type System.SByte.");
        }

        [Theory]
        [InlineData("42.0", 42)]
        [InlineData("37.000", 37)]
        [InlineData("127.0000", sbyte.MaxValue)]
        [InlineData("-128.00000", sbyte.MinValue)]
        [InlineData("0.0", 0)]
        public void NumbersWithTrailingZerosAfterDecimalPointCanBeDeserialized(string json, sbyte expected)
        {
            CompareDeserializedJsonToExpected(json, expected);
        }

        [Theory]
        [InlineData("42.7")]
        [InlineData("37.007")]
        [InlineData("127.0353")]
        [InlineData("0.00000856")]
        [InlineData("-128.000000001")]
        public void ExceptionIsThrownWhenNumbersWithNonZeroDigitsAfterDecimalPointIsDeserialized(string json)
        {
            CheckDeserializerThrowsExceptionWithMessage<int>(json, $"Could not deserialize value {json} because it is no integer, but a real number.");
        }
    }

    public sealed class JsonLongDeserializationTests : BaseDefaultJsonDeserializationTest
    {
        [Theory]
        [InlineData("42", 42)]
        [InlineData(long.MaxValue / 2, long.MaxValue / 2)]
        [InlineData("8323372036854775807", 8323372036854775807)]
        [InlineData(long.MaxValue, long.MaxValue)]
        [InlineData(long.MinValue, long.MinValue)]
        [InlineData("0", 0)]
        [InlineData(long.MinValue + long.MaxValue / 2, long.MinValue + long.MaxValue / 2)]
        public void IntValueCanBeDeserializedCorrectly(string json, long expected)
        {
            CompareDeserializedJsonToExpected(json, expected);
        }

        [Theory]
        [InlineData("9223372036854775808")] //long max value ( 9223372036854775807 ) +1
        [InlineData("-9223372036854775818")]
        [InlineData("9323372036854775807")]
        [InlineData("10223372036854775807")]
        [InlineData("922337203685477580700000000")]
        public void ExceptionIsThrownWhenOverflowingIntValueIsDeserialized(string json)
        {
            CheckDeserializerThrowsExceptionWithMessage<long>(json, $"Could not deserialize value {json} because it produces an overflow for type System.Int64.");
        }

        [Theory]
        [InlineData("42.0", 42)]
        [InlineData("8323372036854775807.000", 8323372036854775807)]
        [InlineData("9223372036854775807.0000", long.MaxValue)]
        [InlineData("-9223372036854775808.00000", long.MinValue)]
        [InlineData("0.0", 0)]
        public void NumbersWithTrailingZerosAfterDecimalPointCanBeDeserialized(string json, long expected)
        {
            CompareDeserializedJsonToExpected(json, expected);
        }

        [Theory]
        [InlineData("42.7")]
        [InlineData("8323372036854775807.007")]
        [InlineData("9223372036854775807.0353")]
        [InlineData("0.00000856")]
        [InlineData("-9223372036854775808.000000001")]
        public void ExceptionIsThrownWhenNumbersWithNonZeroDigitsAfterDecimalPointIsDeserialized(string json)
        {
            CheckDeserializerThrowsExceptionWithMessage<int>(json, $"Could not deserialize value {json} because it is no integer, but a real number.");
        }
    }

    public sealed class JsonULongDeserializationTests : BaseDefaultJsonDeserializationTest
    {
        [Theory]
        [InlineData("42", 42ul)]
        [InlineData(ulong.MaxValue / 2, ulong.MaxValue / 2)]
        [InlineData("8323372036854775807", 8323372036854775807ul)]
        [InlineData(ulong.MaxValue, ulong.MaxValue)]
        [InlineData("0", 0ul)]
        public void IntValueCanBeDeserializedCorrectly(string json, ulong expected)
        {
            CompareDeserializedJsonToExpected(json, expected);
        }

        [Theory]
        [InlineData("9223372036854775808")] //long max value ( 9223372036854775807 ) +1
        [InlineData("-9223372036854775818")]
        [InlineData("9323372036854775807")]
        [InlineData("10223372036854775807")]
        [InlineData("922337203685477580700000000")]
        public void ExceptionIsThrownWhenOverflowingIntValueIsDeserialized(string json)
        {
            CheckDeserializerThrowsExceptionWithMessage<long>(json, $"Could not deserialize value {json} because it produces an overflow for type System.Int64.");
        }

        [Theory]
        [InlineData("42.0", 42ul)]
        [InlineData("8323372036854775807.000", 8323372036854775807ul)]
        [InlineData("18446744073709551615.0000", ulong.MaxValue)]
        [InlineData("0.0", 0ul)]
        public void NumbersWithTrailingZerosAfterDecimalPointCanBeDeserialized(string json, ulong expected)
        {
            CompareDeserializedJsonToExpected(json, expected);
        }

        [Theory]
        [InlineData("42.7")]
        [InlineData("8323372036854775807.007")]
        [InlineData("9223372036854775807.0353")]
        [InlineData("0.00000856")]
        [InlineData("-9223372036854775808.000000001")]
        public void ExceptionIsThrownWhenNumbersWithNonZeroDigitsAfterDecimalPointIsDeserialized(string json)
        {
            CheckDeserializerThrowsExceptionWithMessage<int>(json, $"Could not deserialize value {json} because it is no integer, but a real number.");
        }
    }
}