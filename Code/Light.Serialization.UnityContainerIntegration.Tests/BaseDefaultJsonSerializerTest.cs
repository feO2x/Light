using System;
using System.Collections.Generic;
using FluentAssertions;
using Light.Serialization.Json;
using Light.Serialization.Json.SerializationRules;
using Light.Serialization.UnityContainerIntegration;
using Microsoft.Practices.Unity;

// ReSharper disable once CheckNamespace
namespace Light.Serialization.Tests
{
    public abstract class BaseDefaultJsonSerializerTest
    {
        protected IUnityContainer Container;

        protected BaseDefaultJsonSerializerTest()
        {
            Container = new UnityContainer().RegisterDefaultSerializationTypes();
        }

        protected void CompareJsonToExpected<T>(T value, string expected)
        {
            var jsonSerializer = Container.RegisterDefaultSerializationTypes().Resolve<ISerializer>();
            var json = jsonSerializer.Serialize(value);

            json.Should().Be(expected);
        }

        protected void AddRule<T>(Action<Rule<T>> rule)
        {
            Container.WithSerializationRuleFor(rule);
        }

        protected void AddWriterInstructors(IList<IJsonWriterInstructor> writerInstructors)
        {
            //todo: implement WithWriterInstructors on IUnityContainer
        }
    }
}