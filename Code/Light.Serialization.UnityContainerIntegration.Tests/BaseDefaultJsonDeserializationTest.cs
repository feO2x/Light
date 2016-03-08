using System;
using Domain;
using FluentAssertions;
using Light.Serialization.Json;
using Light.Serialization.UnityContainerIntegration;
using Microsoft.Practices.Unity;

// ReSharper disable once CheckNamespace
namespace Light.Serialization.Tests
{
    public abstract class BaseDefaultJsonDeserializationTest
    {
        public static void CompareDeserializedJsonToExpected<T>(string json, T expected)
        {
            var diContainer = new UnityContainer().RegisterDefaultDeserializationTypes();
            var testTarget = diContainer.Resolve<IDeserializer>();
            var actual = testTarget.Deserialize<T>(json);
            actual.Should().Be(expected);
        }

        public static T GetDeserializedJson<T>(string json)
        {
            var diContainer = new UnityContainer().RegisterDefaultDeserializationTypes();
            var testTarget = diContainer.Resolve<IDeserializer>();
            return testTarget.Deserialize<T>(json);
        }

        public static object GetDeserializedJson(string json, Type requestedType)
        {
            var diContainer = new UnityContainer().RegisterDefaultDeserializationTypes();
            var testTarget = diContainer.Resolve<IDeserializer>();
            return testTarget.Deserialize(json, requestedType);
        }

        public static void CheckDeserializerThrowsExceptionWithMessageContaining<T>(string json, string partOfExceptionMessage)
        {
            var diContainer = new UnityContainer().RegisterDefaultDeserializationTypes();
            var testTarget = diContainer.Resolve<IDeserializer>();
            Action act = () => testTarget.Deserialize<T>(json);
            act.ShouldThrow<DeserializationException>().And.Message.Should().Contain(partOfExceptionMessage);
        }

        public static void CheckDeserializerThrowsExceptionWithMessage<T>(string json, string exceptionMessage)
        {
            var diContainer = new UnityContainer().RegisterDefaultDeserializationTypes();
            var testTarget = diContainer.Resolve<IDeserializer>();
            Action act = () => testTarget.Deserialize<T>(json);
            act.ShouldThrow<DeserializationException>().And.Message.Should().Be(exceptionMessage);
        }
    }
}
