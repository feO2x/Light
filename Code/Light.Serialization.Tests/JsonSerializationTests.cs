﻿using FluentAssertions;
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
        public void IntegerNumbersAreSerializedCorrectly<T>(T number)
        {
            CompareJsonToExpected(number, number.ToString());
        }

        private void CompareJsonToExpected<T>(T value, string expected)
        {
            var json = JsonSerializer.Serialize(value);

            json.Should().Be(expected);
        }
    }
}
