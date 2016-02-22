using System;
using Xunit;
using System.Linq;

namespace Light.Serialization.Tests
{


    public sealed class JsonIntParserDeserializationTests : BaseDefaultJsonDeserializationTest
    {
		[Theory]
        [InlineData("42", 42)]
		[InlineData(int.MaxValue/2, int.MaxValue/2)]
        [InlineData("1247483647", 1247483647)]
        [InlineData(int.MaxValue, int.MaxValue)]
        [InlineData(int.MinValue, int.MinValue)]
		[InlineData("0", 0)]
        [InlineData(int.MinValue + int.MaxValue/2, int.MinValue + int.MaxValue/2)]
				
		public void IntValueCanBeDeserializedCorrectly(string json, int expected)
        {
            CompareDeserializedJsonToExpected(json, expected);
        }

		[Theory]
		[InlineData("2147483648")] //int max value ( 2147483647 ) +1
        [InlineData("-2147483658")]
        //[InlineData("-2247483648")]
        [InlineData("3147483647")]
        //[InlineData("-375000000000")]

        public void ExceptionIsThrownWhenOverflowingIntValueIsDeserialized(string json)
        {
            CheckDeserializerThrowsExceptionWithMessage<int>(json, $"Could not deserialize value {json} because it produces an overflow for type int.");
		}

	}
	
    public sealed class JsonUIntParserDeserializationTests : BaseDefaultJsonDeserializationTest
    {
		[Theory]
        [InlineData("42", 42)]
		[InlineData(uint.MaxValue/2, uint.MaxValue/2)]
        [InlineData("3394967295", 3394967295)]
        [InlineData(uint.MaxValue, uint.MaxValue)]
        [InlineData(uint.MinValue, uint.MinValue)]
				
		public void IntValueCanBeDeserializedCorrectly(string json, uint expected)
        {
            CompareDeserializedJsonToExpected(json, expected);
        }

		[Theory]
		//[InlineData("4294967296")] //uint max value ( 4294967295 ) +1
        [InlineData("-1")]
        //[InlineData("-2247483648")]
        //[InlineData("5294967295")]
        //[InlineData("-375000000000")]

        public void ExceptionIsThrownWhenOverflowingIntValueIsDeserialized(string json)
        {
            CheckDeserializerThrowsExceptionWithMessage<uint>(json, $"Could not deserialize value {json} because it produces an overflow for type uint.");
		}

	}
	
    public sealed class JsonShortParserDeserializationTests : BaseDefaultJsonDeserializationTest
    {
		[Theory]
        [InlineData("42", 42)]
		[InlineData(short.MaxValue/2, short.MaxValue/2)]
        [InlineData("23767", 23767)]
        [InlineData(short.MaxValue, short.MaxValue)]
        [InlineData(short.MinValue, short.MinValue)]
		[InlineData("0", 0)]
        [InlineData(short.MinValue + short.MaxValue/2, short.MinValue + short.MaxValue/2)]
				
		public void IntValueCanBeDeserializedCorrectly(string json, short expected)
        {
            CompareDeserializedJsonToExpected(json, expected);
        }

		[Theory]
		[InlineData("32768")] //short max value ( 32767 ) +1
        [InlineData("-32778")]
        //[InlineData("-2247483648")]
        [InlineData("42767")]
        //[InlineData("-375000000000")]

        public void ExceptionIsThrownWhenOverflowingIntValueIsDeserialized(string json)
        {
            CheckDeserializerThrowsExceptionWithMessage<short>(json, $"Could not deserialize value {json} because it produces an overflow for type short.");
		}

	}
	
    public sealed class JsonUShortParserDeserializationTests : BaseDefaultJsonDeserializationTest
    {
		[Theory]
        [InlineData("42", 42)]
		[InlineData(ushort.MaxValue/2, ushort.MaxValue/2)]
        [InlineData("56535", 56535)]
        [InlineData(ushort.MaxValue, ushort.MaxValue)]
        [InlineData(ushort.MinValue, ushort.MinValue)]
		[InlineData("0", 0)]
        [InlineData(ushort.MinValue + ushort.MaxValue/2, ushort.MinValue + ushort.MaxValue/2)]
				
		public void IntValueCanBeDeserializedCorrectly(string json, ushort expected)
        {
            CompareDeserializedJsonToExpected(json, expected);
        }

		[Theory]
		[InlineData("65536")] //ushort max value ( 65535 ) +1
        [InlineData("-1")]
        //[InlineData("-2247483648")]
        [InlineData("75535")]
        //[InlineData("-375000000000")]

        public void ExceptionIsThrownWhenOverflowingIntValueIsDeserialized(string json)
        {
            CheckDeserializerThrowsExceptionWithMessage<ushort>(json, $"Could not deserialize value {json} because it produces an overflow for type ushort.");
		}

	}
	
    public sealed class JsonByteParserDeserializationTests : BaseDefaultJsonDeserializationTest
    {
		[Theory]
        [InlineData("42", 42)]
		[InlineData(byte.MaxValue/2, byte.MaxValue/2)]
        [InlineData("165", 165)]
        [InlineData(byte.MaxValue, byte.MaxValue)]
        [InlineData(byte.MinValue, byte.MinValue)]
		[InlineData("0", 0)]
        [InlineData(byte.MinValue + byte.MaxValue/2, byte.MinValue + byte.MaxValue/2)]
				
		public void IntValueCanBeDeserializedCorrectly(string json, byte expected)
        {
            CompareDeserializedJsonToExpected(json, expected);
        }

		[Theory]
		[InlineData("256")] //byte max value ( 255 ) +1
        [InlineData("-1")]
        //[InlineData("-2247483648")]
        [InlineData("355")]
        //[InlineData("-375000000000")]

        public void ExceptionIsThrownWhenOverflowingIntValueIsDeserialized(string json)
        {
            CheckDeserializerThrowsExceptionWithMessage<byte>(json, $"Could not deserialize value {json} because it produces an overflow for type byte.");
		}

	}
	
    public sealed class JsonSByteParserDeserializationTests : BaseDefaultJsonDeserializationTest
    {
		[Theory]
        [InlineData("42", 42)]
		[InlineData(sbyte.MaxValue/2, sbyte.MaxValue/2)]
        [InlineData("37", 37)]
        [InlineData(sbyte.MaxValue, sbyte.MaxValue)]
        [InlineData(sbyte.MinValue, sbyte.MinValue)]
		[InlineData("0", 0)]
        [InlineData(sbyte.MinValue + sbyte.MaxValue/2, sbyte.MinValue + sbyte.MaxValue/2)]
				
		public void IntValueCanBeDeserializedCorrectly(string json, sbyte expected)
        {
            CompareDeserializedJsonToExpected(json, expected);
        }

		[Theory]
		[InlineData("128")] //sbyte max value ( 127 ) +1
        [InlineData("-138")]
        //[InlineData("-2247483648")]
        [InlineData("227")]
        //[InlineData("-375000000000")]

        public void ExceptionIsThrownWhenOverflowingIntValueIsDeserialized(string json)
        {
            CheckDeserializerThrowsExceptionWithMessage<sbyte>(json, $"Could not deserialize value {json} because it produces an overflow for type sbyte.");
		}

	}
	
    public sealed class JsonLongParserDeserializationTests : BaseDefaultJsonDeserializationTest
    {
		[Theory]
        [InlineData("42", 42)]
		[InlineData(long.MaxValue/2, long.MaxValue/2)]
        [InlineData("8323372036854775807", 8323372036854775807)]
        [InlineData(long.MaxValue, long.MaxValue)]
        [InlineData(long.MinValue, long.MinValue)]
		[InlineData("0", 0)]
        [InlineData(long.MinValue + long.MaxValue/2, long.MinValue + long.MaxValue/2)]
				
		public void IntValueCanBeDeserializedCorrectly(string json, long expected)
        {
            CompareDeserializedJsonToExpected(json, expected);
        }

		[Theory]
		[InlineData("9223372036854775808")] //long max value ( 9223372036854775807 ) +1
        [InlineData("-9223372036854775818")]
        //[InlineData("-2247483648")]
        [InlineData("10223372036854775807")]
        //[InlineData("-375000000000")]

        public void ExceptionIsThrownWhenOverflowingIntValueIsDeserialized(string json)
        {
            CheckDeserializerThrowsExceptionWithMessage<long>(json, $"Could not deserialize value {json} because it produces an overflow for type long.");
		}

	}
	}

