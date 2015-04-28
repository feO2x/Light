﻿using System;
using FluentAssertions;
using Light.Core;
using Light.Serialization.Json;
using Xunit;

namespace Light.Serialization.Tests
{
    public sealed class JsonSerializationTests
    {
        public readonly ISerializer JsonSerializer;

        public JsonSerializationTests()
        {
            JsonSerializer = new JsonSerializerBuilder().Build();
        }

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

        private void CompareJsonToExpected<T>(T value, string expected)
        {
            var json = JsonSerializer.Serialize(value);

            json.Should().Be(expected);
        }
    }
}
