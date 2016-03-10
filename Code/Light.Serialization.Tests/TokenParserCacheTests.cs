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
        [Fact(DisplayName = "Validate that blacklisted JsonToken and Type combinations are found in the blacklist.")]
        public void ValidateBlacklistedCombinations()
        {
            var jsonToken = new JsonToken(new char[1], 0, 0, JsonTokenType.BeginOfArray);
            var type = typeof (int);
            var jsonTokenTypeCombination = new JsonTokenTypeCombination(jsonToken, type);
            var cache = new JsonTokenParserCache(new List<JsonTokenTypeCombination> {jsonTokenTypeCombination });

            var result = cache.CheckTokenTypeCombinationForBlacklist(jsonTokenTypeCombination);

            result.ShouldBeEquivalentTo(true);
        }

        [Fact(DisplayName = "Validate that blacklisted JsonToken and Type are found in the blacklist.")]
        public void ValidateBlacklistedJsonTokenAndType()
        {
            var jsonToken = new JsonToken(new char[1], 0, 0, JsonTokenType.BeginOfArray);
            var type = typeof(int);
            var jsonTokenTypeCombination = new JsonTokenTypeCombination(jsonToken, type);
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
            var jsonTokenTypeCombination = new JsonTokenTypeCombination(jsonToken, typeInt);
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
            var jsonTokenTypeCombination = new JsonTokenTypeCombination(jsonTokenForCombination, typeInt);
            var cache = new JsonTokenParserCache(new List<JsonTokenTypeCombination> { jsonTokenTypeCombination });

            var jsonTokenToCompare = new JsonToken(new char[2], 1, 2, JsonTokenType.EndOfArray);

            //act
            var result = cache.CheckTokenTypeForBlacklist(jsonTokenToCompare, typeInt);

            //assert
            result.ShouldBeEquivalentTo(false);
        }

        [Fact(DisplayName = "Validate that non-blacklisted JsonToken and Type combinations are not found in the blacklist.")]
        public void ValidateNonBlacklistedCombinations()
        {
            //arrange
            var jsonTokenForBlacklist = new JsonToken(new char[1], 0, 0, JsonTokenType.BeginOfArray);
            var typeInt = typeof(int);
            var jsonTokenTypeCombinationForBlacklist = new JsonTokenTypeCombination(jsonTokenForBlacklist, typeInt);

            var typeChar = typeof(char);
            var jsonTokenToCompare = new JsonToken(new char[2], 1, 1, JsonTokenType.BeginOfObject);
            var jsonTokenTypeCombinationToCompare = new JsonTokenTypeCombination(jsonTokenToCompare, typeChar);

            var cache = new JsonTokenParserCache(new List<JsonTokenTypeCombination> { jsonTokenTypeCombinationForBlacklist });

            //act
            var result = cache.CheckTokenTypeCombinationForBlacklist(jsonTokenTypeCombinationToCompare);

            //assert
            result.ShouldBeEquivalentTo(false);
        }

        [Fact(DisplayName = "Validate that non-blacklisted JsonToken and Type are not found in the blacklist.")]
        public void ValidateNonBlacklistedJsonTokenAndType()
        {
            //arrange
            var jsonTokenForBlacklist = new JsonToken(new char[1], 0, 0, JsonTokenType.BeginOfArray);
            var typeInt = typeof(int);
            var jsonTokenTypeCombinationForBlacklist = new JsonTokenTypeCombination(jsonTokenForBlacklist, typeInt);

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
            var jsonTokenTypeCombinationForBlacklist = new JsonTokenTypeCombination(jsonTokenForBlacklist, typeInt);

            var typeChar = typeof(char);
            var jsonTokenToCompare = new JsonToken(new char[2], 1, 1, JsonTokenType.BeginOfObject);
            var jsonTokenTypeCombinationForCache = new JsonTokenTypeCombination(jsonTokenToCompare, typeChar);

            var cache = new JsonTokenParserCache(new List<JsonTokenTypeCombination> { jsonTokenTypeCombinationForBlacklist });

            //act
            var result = cache.TryAddTokenParserToCache(jsonTokenTypeCombinationForCache, new UnsignedIntegerParser());

            //assert
            result.ShouldBeEquivalentTo(true);
        }

        [Fact(DisplayName = "Validate that blacklisted JsonToken and Type cannot be added to cache.")]
        public void ValidateBlacklistedJsonTokenAndTypeCannotAddedToCache()
        {
            //arrange
            var jsonTokenForBlacklist = new JsonToken(new char[1], 0, 0, JsonTokenType.BeginOfArray);
            var typeInt = typeof(int);
            var jsonTokenTypeCombinationForBlacklist = new JsonTokenTypeCombination(jsonTokenForBlacklist, typeInt);

            var cache = new JsonTokenParserCache(new List<JsonTokenTypeCombination> { jsonTokenTypeCombinationForBlacklist });

            //act
            var result = cache.TryAddTokenParserToCache(jsonTokenTypeCombinationForBlacklist, new UnsignedIntegerParser());

            //assert
            result.ShouldBeEquivalentTo(false);
        }

        [Fact(DisplayName = "Validate that non-blacklisted JsonToken and Type are added to cache properly.")]
        public void NonBlacklistedJsonTokenAndTypeAddedToCacheProperly()
        {
            //arrange
            var jsonTokenForBlacklist = new JsonToken(new char[1], 0, 0, JsonTokenType.BeginOfArray);
            var typeInt = typeof(int);
            var jsonTokenTypeCombinationForBlacklist = new JsonTokenTypeCombination(jsonTokenForBlacklist, typeInt);

            var typeChar = typeof(char);
            var jsonTokenToCompare = new JsonToken(new char[2], 1, 1, JsonTokenType.BeginOfObject);
            var jsonTokenTypeCombinationForCache = new JsonTokenTypeCombination(jsonTokenToCompare, typeChar);

            var jsonTokenParser = new SignedIntegerParser();
            IJsonTokenParser jsonTokenParserFromCache;

            var cache = new JsonTokenParserCache(new List<JsonTokenTypeCombination> { jsonTokenTypeCombinationForBlacklist });

            //act
            cache.TryAddTokenParserToCache(jsonTokenTypeCombinationForCache, jsonTokenParser);
            cache.TryGetTokenParser(jsonTokenTypeCombinationForCache, out jsonTokenParserFromCache);
            
            //assert
            jsonTokenParser.ShouldBeEquivalentTo(jsonTokenParser);
        }

        [Fact(DisplayName = "Validate that non-blacklisted JsonToken and Type are not added to cache multiple times.")]
        public void NonBlacklistedJsonTokenAndTypeNotAddedToCacheMultipleTimes()
        {
            //arrange
            var jsonTokenForBlacklist = new JsonToken(new char[1], 0, 0, JsonTokenType.BeginOfArray);
            var typeInt = typeof(int);
            var jsonTokenTypeCombinationForBlacklist = new JsonTokenTypeCombination(jsonTokenForBlacklist, typeInt);

            var typeChar = typeof(char);
            var jsonTokenToCompare = new JsonToken(new char[2], 1, 1, JsonTokenType.BeginOfObject);
            var jsonTokenTypeCombinationForCache = new JsonTokenTypeCombination(jsonTokenToCompare, typeChar);

            var jsonTokenParser = new SignedIntegerParser();

            var cache = new JsonTokenParserCache(new List<JsonTokenTypeCombination> { jsonTokenTypeCombinationForBlacklist });

            //act
            cache.TryAddTokenParserToCache(jsonTokenTypeCombinationForCache, jsonTokenParser);
            var result = cache.TryAddTokenParserToCache(jsonTokenTypeCombinationForCache, jsonTokenParser);

            //assert
            result.ShouldBeEquivalentTo(false);
        }

    }
}
