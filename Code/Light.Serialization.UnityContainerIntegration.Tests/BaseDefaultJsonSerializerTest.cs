using FluentAssertions;
using Light.Serialization.Json;
using Light.Serialization.UnityContainerIntegration;
using Microsoft.Practices.Unity;

// ReSharper disable once CheckNamespace
namespace Light.Serialization.Tests
{
    public abstract class BaseDefaultJsonSerializerTest
    {
        protected ISerializer JsonSerializer;

        protected BaseDefaultJsonSerializerTest()
        {
            JsonSerializer = new UnityContainer().RegisterDefaultSerializationTypes().Resolve<ISerializer>();
        }

        protected void CompareJsonToExpected<T>(T value, string expected)
        {
            var json = JsonSerializer.Serialize(value);

            json.Should().Be(expected);
        }
    }
}