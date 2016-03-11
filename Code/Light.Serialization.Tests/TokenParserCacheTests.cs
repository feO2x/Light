using System;
using System.Collections.Generic;
using FluentAssertions;
using Light.Serialization.Json;
using Light.Serialization.Json.Caching;
using Light.Serialization.Json.TokenParsers;
using Xunit;

namespace Light.Serialization.Tests
{
    public sealed class TokenParserCacheTests
    {
        [Fact(DisplayName = "Validate that blacklisted JsonToken and Type are found in the blacklist.")]
        public void ValidateBlacklistedJsonTokenAndType()
        {
            var jsonToken = new JsonToken(new char[1], 0, 0, JsonTokenType.BeginOfArray);
            var type = typeof(int);
            var jsonTokenTypeCombination = new JsonTokenTypeCombination(jsonToken.JsonType, type);
            var cache = new JsonTokenParserCache(new List<JsonTokenTypeCombination> { jsonTokenTypeCombination });

            var result = cache.CheckTokenTypeForBlacklist(jsonToken, type);

            result.ShouldBeEquivalentTo(true);
        }

        [Fact(DisplayName = "Validate that blacklisted JsonToken and Type are found in the blacklist even if the JsonTokens are equal.")]
        public void ValidateBlacklistedJsonTokenAndTypeWithEqualJsonToken()
        {
            //arrange
            var jsonToken = new JsonToken(new char[1], 0, 0, JsonTokenType.BeginOfArray);
            var typeInt = typeof(int);
            var jsonTokenTypeCombination = new JsonTokenTypeCombination(jsonToken.JsonType, typeInt);
            var cache = new JsonTokenParserCache(new List<JsonTokenTypeCombination> { jsonTokenTypeCombination });

            var typeLong = typeof (long);

            //act
            var result = cache.CheckTokenTypeForBlacklist(jsonToken, typeLong);
            
            //assert
            result.ShouldBeEquivalentTo(false);
        }

        [Fact(DisplayName = "Validate that blacklisted JsonToken and Type are found in the blacklist even if the Types are equal.")]
        public void ValidateBlacklistedJsonTokenAndTypeWithEqualTypes()
        {
            //arrange
            var jsonTokenForCombination = new JsonToken(new char[1], 0, 0, JsonTokenType.BeginOfArray);
            var typeInt = typeof(int);
            var jsonTokenTypeCombination = new JsonTokenTypeCombination(jsonTokenForCombination.JsonType, typeInt);
            var cache = new JsonTokenParserCache(new List<JsonTokenTypeCombination> { jsonTokenTypeCombination });

            var jsonTokenToCompare = new JsonToken(new char[2], 1, 2, JsonTokenType.EndOfArray);

            //act
            var result = cache.CheckTokenTypeForBlacklist(jsonTokenToCompare, typeInt);

            //assert
            result.ShouldBeEquivalentTo(false);
        }

        [Fact(DisplayName = "Validate that non-blacklisted JsonToken and Type are not found in the blacklist.")]
        public void ValidateNonBlacklistedJsonTokenAndType()
        {
            //arrange
            var jsonTokenForBlacklist = new JsonToken(new char[1], 0, 0, JsonTokenType.BeginOfArray);
            var typeInt = typeof(int);
            var jsonTokenTypeCombinationForBlacklist = new JsonTokenTypeCombination(jsonTokenForBlacklist.JsonType, typeInt);

            var typeChar = typeof(char);
            var jsonTokenToCompare = new JsonToken(new char[2], 1, 1, JsonTokenType.BeginOfObject);

            var cache = new JsonTokenParserCache(new List<JsonTokenTypeCombination> { jsonTokenTypeCombinationForBlacklist });

            //act
            var result = cache.CheckTokenTypeForBlacklist(jsonTokenToCompare, typeChar);

            //assert
            result.ShouldBeEquivalentTo(false);
        }

        [Fact(DisplayName = "Validate that non-blacklisted JsonToken and Type can be added to cache.")]
        public void NonBlacklistedJsonTokenAndTypeCanAddedToCache()
        {
            //arrange
            var jsonTokenForBlacklist = new JsonToken(new char[1], 0, 0, JsonTokenType.BeginOfArray);
            var typeInt = typeof(int);
            var jsonTokenTypeCombinationForBlacklist = new JsonTokenTypeCombination(jsonTokenForBlacklist.JsonType, typeInt);

            var typeChar = typeof(char);
            var jsonTokenToCompare = new JsonToken(new char[2], 1, 1, JsonTokenType.BeginOfObject);

            var cache = new JsonTokenParserCache(new List<JsonTokenTypeCombination> { jsonTokenTypeCombinationForBlacklist });

            //act
            var result = cache.TryAddTokenParserToCache(jsonTokenToCompare, typeChar, new UnsignedIntegerParser());

            //assert
            result.ShouldBeEquivalentTo(true);
        }

        [Fact(DisplayName = "Validate that blacklisted JsonToken and Type cannot be added to cache.")]
        public void ValidateBlacklistedJsonTokenAndTypeCannotAddedToCache()
        {
            //arrange
            var jsonTokenForBlacklist = new JsonToken(new char[1], 0, 0, JsonTokenType.BeginOfArray);
            var typeInt = typeof(int);
            var jsonTokenTypeCombinationForBlacklist = new JsonTokenTypeCombination(jsonTokenForBlacklist.JsonType, typeInt);

            var cache = new JsonTokenParserCache(new List<JsonTokenTypeCombination> { jsonTokenTypeCombinationForBlacklist });

            //act
            var result = cache.TryAddTokenParserToCache(jsonTokenForBlacklist, typeInt, new UnsignedIntegerParser());

            //assert
            result.ShouldBeEquivalentTo(false);
        }

        [Fact(DisplayName = "Validate that non-blacklisted JsonToken and Type are added to cache properly.")]
        public void NonBlacklistedJsonTokenAndTypeAddedToCacheProperly()
        {
            //arrange
            var jsonTokenForBlacklist = new JsonToken(new char[1], 0, 0, JsonTokenType.BeginOfArray);
            var typeInt = typeof(int);
            var jsonTokenTypeCombinationForBlacklist = new JsonTokenTypeCombination(jsonTokenForBlacklist.JsonType, typeInt);

            var typeChar = typeof(char);
            var jsonTokenToCompare = new JsonToken(new char[2], 1, 1, JsonTokenType.BeginOfObject);

            var jsonTokenParser = new SignedIntegerParser();
            IJsonTokenParser jsonTokenParserFromCache;

            var cache = new JsonTokenParserCache(new List<JsonTokenTypeCombination> { jsonTokenTypeCombinationForBlacklist });

            //act
            cache.TryAddTokenParserToCache(jsonTokenToCompare, typeChar, jsonTokenParser);
            cache.TryGetTokenParser(jsonTokenToCompare, typeChar, out jsonTokenParserFromCache);
            
            //assert
            jsonTokenParser.ShouldBeEquivalentTo(jsonTokenParser);
        }

        [Fact(DisplayName = "Validate that non-blacklisted JsonToken and Type are not added to cache multiple times.")]
        public void NonBlacklistedJsonTokenAndTypeNotAddedToCacheMultipleTimes()
        {
            //arrange
            var jsonTokenForBlacklist = new JsonToken(new char[1], 0, 0, JsonTokenType.BeginOfArray);
            var typeInt = typeof(int);
            var jsonTokenTypeCombinationForBlacklist = new JsonTokenTypeCombination(jsonTokenForBlacklist.JsonType, typeInt);

            var typeChar = typeof(char);
            var jsonTokenToCompare = new JsonToken(new char[2], 1, 1, JsonTokenType.BeginOfObject);

            var jsonTokenParser = new SignedIntegerParser();

            var cache = new JsonTokenParserCache(new List<JsonTokenTypeCombination> { jsonTokenTypeCombinationForBlacklist });

            //act
            cache.TryAddTokenParserToCache(jsonTokenToCompare, typeChar, jsonTokenParser);
            var result = cache.TryAddTokenParserToCache(jsonTokenToCompare, typeChar, jsonTokenParser);

            //assert
            result.ShouldBeEquivalentTo(false);
        }

    }
}
