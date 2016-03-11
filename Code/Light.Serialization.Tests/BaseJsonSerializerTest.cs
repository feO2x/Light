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
    public abstract class BaseJsonSerializerTest
    {
        protected JsonSerializerBuilder JsonSerializerBuilder;

        protected BaseJsonSerializerTest()
        {
            JsonSerializerBuilder = new JsonSerializerBuilder();
        }

        protected void CompareJsonToExpected<T>(T value, string expected)
        {
            var json = GetSerializedJson(value);

            json.Should().Be(expected);
        }

        protected string GetSerializedJson<T>(T value)
        {
            var jsonSerializer = JsonSerializerBuilder.Build();

            return jsonSerializer.Serialize(value);
        }

        protected void AddRule<T>(Action<Rule<T>> rule)
        {
            JsonSerializerBuilder.WithRuleFor(rule);
        }

        protected void ReplaceTimeZoneInfoInDateTimeFormatter(TimeZoneInfo timeZoneInfo)
        {
            // TODO: this is shitty design, we should refactor the Serializer and Deserializer Builder so that it is not that hard to exchange objects
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