using System;
using System.Collections.Generic;
using Light.GuardClauses;
using Light.Serialization.Json.TokenParsers;

namespace Light.Serialization.Json.Caching
{
    public static class JsonTokenCacheExtensions
    {
        public static Dictionary<JsonTokenTypeCombination, IJsonTokenParser> AddDefaultTokenParsersToCache(this Dictionary<JsonTokenTypeCombination, IJsonTokenParser> cache)
        {
            cache.MustNotBeNull(nameof(cache));

            cache.Add(new JsonTokenTypeCombination(JsonTokenType.IntegerNumber, typeof(object)), new SignedIntegerParser());
            cache.Add(new JsonTokenTypeCombination(JsonTokenType.IntegerNumber, typeof(ValueType)), new SignedIntegerParser());
            cache.Add(new JsonTokenTypeCombination(JsonTokenType.IntegerNumber, typeof(int)), new SignedIntegerParser());
            cache.Add(new JsonTokenTypeCombination(JsonTokenType.FloatingPointNumber, typeof(int)), new SignedIntegerParser());
            cache.Add(new JsonTokenTypeCombination(JsonTokenType.IntegerNumber, typeof(uint)), new UnsignedIntegerParser());
            cache.Add(new JsonTokenTypeCombination(JsonTokenType.FloatingPointNumber, typeof(uint)), new UnsignedIntegerParser());
            cache.Add(new JsonTokenTypeCombination(JsonTokenType.IntegerNumber, typeof(long)), new SignedIntegerParser());
            cache.Add(new JsonTokenTypeCombination(JsonTokenType.FloatingPointNumber, typeof(long)), new SignedIntegerParser());
            cache.Add(new JsonTokenTypeCombination(JsonTokenType.IntegerNumber, typeof(ulong)), new UnsignedIntegerParser());
            cache.Add(new JsonTokenTypeCombination(JsonTokenType.FloatingPointNumber, typeof(ulong)), new UnsignedIntegerParser());
            cache.Add(new JsonTokenTypeCombination(JsonTokenType.IntegerNumber, typeof(sbyte)), new SignedIntegerParser());
            cache.Add(new JsonTokenTypeCombination(JsonTokenType.FloatingPointNumber, typeof(sbyte)), new SignedIntegerParser());
            cache.Add(new JsonTokenTypeCombination(JsonTokenType.IntegerNumber, typeof(byte)), new UnsignedIntegerParser());
            cache.Add(new JsonTokenTypeCombination(JsonTokenType.FloatingPointNumber, typeof(byte)), new UnsignedIntegerParser());
            cache.Add(new JsonTokenTypeCombination(JsonTokenType.IntegerNumber, typeof(short)), new SignedIntegerParser());
            cache.Add(new JsonTokenTypeCombination(JsonTokenType.FloatingPointNumber, typeof(short)), new SignedIntegerParser());
            cache.Add(new JsonTokenTypeCombination(JsonTokenType.IntegerNumber, typeof(ushort)), new UnsignedIntegerParser());
            cache.Add(new JsonTokenTypeCombination(JsonTokenType.FloatingPointNumber, typeof(ushort)), new UnsignedIntegerParser());
            cache.Add(new JsonTokenTypeCombination(JsonTokenType.IntegerNumber, typeof(decimal)), new DecimalParser());
            cache.Add(new JsonTokenTypeCombination(JsonTokenType.FloatingPointNumber, typeof(decimal)), new DecimalParser());
            cache.Add(new JsonTokenTypeCombination(JsonTokenType.IntegerNumber, typeof(double)), new DoubleParser());
            cache.Add(new JsonTokenTypeCombination(JsonTokenType.FloatingPointNumber, typeof(double)), new DoubleParser());
            cache.Add(new JsonTokenTypeCombination(JsonTokenType.IntegerNumber, typeof(float)), new FloatParser());
            cache.Add(new JsonTokenTypeCombination(JsonTokenType.FloatingPointNumber, typeof(float)), new FloatParser());
            cache.Add(new JsonTokenTypeCombination(JsonTokenType.True, typeof(bool)), new BooleanParser());
            cache.Add(new JsonTokenTypeCombination(JsonTokenType.False, typeof(bool)), new BooleanParser());
            cache.Add(new JsonTokenTypeCombination(JsonTokenType.String, typeof(char)), new CharacterParser());
            cache.Add(new JsonTokenTypeCombination(JsonTokenType.String, typeof(DateTime)), new DateTimeParser());
            cache.Add(new JsonTokenTypeCombination(JsonTokenType.String, typeof(string)), new StringParser());

            return cache;
        }
    }
}
