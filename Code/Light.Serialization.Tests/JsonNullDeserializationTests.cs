using FluentAssertions;
using Light.Serialization.Json;
using System;
using System.IO;
using Xunit;

namespace Light.Serialization.Tests
{
    public sealed class JsonNullDeserializationTests : BaseDefaultJsonDeserializationTest
    {
        [Theory]
        [InlineData(typeof(object))]    // Normal class
        [InlineData(typeof(IComparable))]   // Interface
        [InlineData(typeof(Stream))]    // Abstract base class
        public void NullValuesAreParsedCorrectly(Type requestedType)
        {
            const string json = "null";
            var testTarget = new JsonDeserializerBuilder().Build();
            var result = testTarget.Deserialize(json, requestedType);
            result.Should().BeNull();
        }

        [Theory]
        [InlineData("nll")]
        [InlineData("nlul")]
        [InlineData("nul")]
        public void ExceptionIsThrownWhenNullIsMisspelled(string json)
        {
            CheckDeserializerThrowsExceptionWithMessage<object>(json, $"Cannot deserialize value {json} to {DefaultJsonSymbols.Null}.");
        }
    }
}
