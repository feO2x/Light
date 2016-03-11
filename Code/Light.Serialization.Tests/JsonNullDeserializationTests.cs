using System;
using System.IO;
using FluentAssertions;
using Light.Serialization.Json;
using Xunit;

namespace Light.Serialization.Tests
{
    public sealed class JsonNullDeserializationTests : BaseJsonDeserializerTest
    {
        [Theory]
        [InlineData(typeof (object))] // Normal class
        [InlineData(typeof (IComparable))] // Interface
        [InlineData(typeof (Stream))] // Abstract base class
        public void NullValuesAreParsedCorrectly(Type requestedType)
        {
            const string json = "null";
            var result = GetDeserializedJson(json, requestedType);
            result.Should().BeNull();
        }

        [Theory]
        [InlineData("nll")]
        [InlineData("nlul")]
        [InlineData("nul")]
        public void ExceptionIsThrownWhenNullIsMisspelled(string json)
        {
            CheckDeserializerThrowsExceptionWithMessage<object>(json, $"Cannot deserialize value {json} to {JsonSymbols.Null}.");
        }
    }
}