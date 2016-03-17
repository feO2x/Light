using System;
using FluentAssertions;
using Light.Serialization.Json;
using Light.Serialization.Json.TokenParsers;
using Light.Serialization.Json.TypeNaming;

namespace Light.Serialization.Tests
{
    public abstract class BaseJsonDeserializerTest
    {
        protected JsonDeserializerBuilder Builder;

        protected BaseJsonDeserializerTest()
        {
            Builder = new JsonDeserializerBuilder();
        }

        public void CompareDeserializedJsonToExpected<T>(string json, T expected)
        {
            var actual = GetDeserializedJson<T>(json);

            actual.Should().Be(expected);
        }

        public T GetDeserializedJson<T>(string json)
        {
            var testTarget = Builder.Build();
            return testTarget.Deserialize<T>(json);
        }

        public object GetDeserializedJson(string json, Type requestedType)
        {
            var testTarget = Builder.Build();
            return testTarget.Deserialize(json, requestedType);
        }

        public void ConfigureDefaultDomainFriendlyNaming(Action<NameToMappingTransformer.IScanningOptions> configureAdditionalTypes = null)
        {
            Builder = new DefaultTokenParsersBuilder().EnableDefaultDomainFriendlyNaming(configureAdditionalTypes).InjectParsersIntoSerializerBuilder();
        }

        public void CheckDeserializerThrowsExceptionWithMessageContaining<T>(string json, string partOfExceptionMessage)
        {
            var testTarget = Builder.Build();

            Action act = () => testTarget.Deserialize<T>(json);

            act.ShouldThrow<DeserializationException>().And.Message.Should().Contain(partOfExceptionMessage);
        }

        public void CheckDeserializerThrowsExceptionWithMessage<T>(string json, string exceptionMessage)
        {
            var testTarget = Builder.Build();

            Action act = () => testTarget.Deserialize<T>(json);

            act.ShouldThrow<DeserializationException>().And.Message.Should().Be(exceptionMessage);
        }
    }
}