using System;
using System.Collections.Generic;
using FluentAssertions;
using Light.Serialization.Json;
using Light.Serialization.Json.SerializationRules;

namespace Light.Serialization.Tests
{
    public abstract class BaseDefaultJsonSerializerTest
    {
        protected JsonSerializerBuilder JsonSerializerBuilder;

        protected BaseDefaultJsonSerializerTest()
        {
            JsonSerializerBuilder = new JsonSerializerBuilder();
        }

        protected void CompareJsonToExpected<T>(T value, string expected)
        {
            var jsonSerializer = JsonSerializerBuilder.Build();
            var json = jsonSerializer.Serialize(value);

            json.Should().Be(expected);
        }

        protected void AddRule<T>(Action<Rule<T>> rule)
        {
            JsonSerializerBuilder.WithRuleFor(rule);
        }

        protected void AddWriterInstructors(IList<IJsonWriterInstructor> writerInstructors)
        {
            JsonSerializerBuilder.WithWriterInstructors(writerInstructors);
        }
    }
}