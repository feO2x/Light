using System;
using FluentAssertions;
using Light.Serialization.Json;

namespace Light.Serialization.Tests
{
    public abstract class BaseDefaultJsonDeserializationTest
    {
        public static void CompareDeserializedJsonToExpected<T>(string json, T expected)
        {
            var testTarget = new JsonDeserializerBuilder().Build();
            var actual = testTarget.Deserialize<T>(json);
            actual.Should().Be(expected);
        }

        public static void CheckDeserializerThrowsExceptionWithMessageContaining<T>(string json, string partOfExceptionMessage)
        {
            var testTarget = new JsonDeserializerBuilder().Build();
            Action act = () => testTarget.Deserialize<T>(json);
            act.ShouldThrow<DeserializationException>().And.Message.Should().Contain(partOfExceptionMessage);
        }

        public static void CheckDeserializerThrowsExceptionWithMessage<T>(string json, string exceptionMessage)
        {
            var testTarget = new JsonDeserializerBuilder().Build();
            Action act = () => testTarget.Deserialize<T>(json);
            act.ShouldThrow<DeserializationException>().And.Message.Should().Be(exceptionMessage);
        }
    }
}