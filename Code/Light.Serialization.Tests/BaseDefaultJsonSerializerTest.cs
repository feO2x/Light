using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Light.Serialization.Json;
using Light.Serialization.Json.Caching;
using Light.Serialization.Json.ComplexTypeDecomposition;
using Light.Serialization.Json.PrimitiveTypeFormatters;
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

        protected void ReplaceTimeZoneInfoInDateTimeFormatter(TimeZoneInfo timeZoneInfo)
        {
            var primitiveFormatters =
                new List<IPrimitiveTypeFormatter>().AddDefaultPrimitiveTypeFormatters(new DefaultCharacterEscaper());
            primitiveFormatters.OfType<DateTimeFormatter>().Single().TimeZoneInfo = timeZoneInfo;

            var writerInstructors = new List<IJsonWriterInstructor>().AddDefaultWriterInstructors(
                primitiveFormatters.ToDictionary(f => f.TargetType),
                new ValueProvidersCacheDecorator(new PublicPropertiesAndFieldsAnalyzer(),
                    new Dictionary<Type, IList<IValueProvider>>()));

            JsonSerializerBuilder.WithWriterInstructors(writerInstructors);
        }
    }
}