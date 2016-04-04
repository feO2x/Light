﻿using System;
using FluentAssertions;
using Light.Serialization.Json;
using Light.Serialization.Json.Caching;
using Light.Serialization.Json.ComplexTypeDecomposition;
using Light.Serialization.Json.LowLevelWriting;
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

        protected void CompareJsonToExpectedWithoutPreservation<T>(T value, string expected)
        {
            var json = GetSerializedJsonWithoutPreservation(value);

            json.Should().Be(expected);
        }

        protected void CompareHumanReadableJsonToExpected<T>(T value, string expected)
        {
            var json = GetSerializedHumanReadableJson(value);

            json.Should().Be(expected);
        }

        protected string GetSerializedJson<T>(T value)
        {
            var jsonSerializer = JsonSerializerBuilder.Build();

            return jsonSerializer.Serialize(value);
        }

        private object GetSerializedJsonWithoutPreservation(object value)
        {
            var jsonSerializer = JsonSerializerBuilder.WithoutPreservation().Build();

            return jsonSerializer.Serialize(value);
        }

        protected string GetSerializedHumanReadableJson<T>(T value)
        {
            var writerFactory = new JsonWriterFactory {JsonWhitespaceFormatter = new IndentingWhitespaceFormatter()};
            var jsonSerializer = JsonSerializerBuilder.WithWriterFactory(writerFactory).Build();

            return jsonSerializer.Serialize(value);
        }

        protected void AddRule<T>(Action<Rule<T>> rule)
        {
            JsonSerializerBuilder.WithRuleFor(rule);
        }

        protected void ReplaceTimeZoneInfoInDateTimeFormatter(TimeZoneInfo timeZoneInfo)
        {
            JsonSerializerBuilder.ConfigureFormatterOfPrimitiveTypeInstructor<DateTimeFormatter>(f => f.TimeZoneInfo = timeZoneInfo);
        }
    }
}