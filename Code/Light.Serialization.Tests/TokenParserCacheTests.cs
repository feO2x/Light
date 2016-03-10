﻿using System;
using System.Collections.Generic;
using FluentAssertions;
using Light.Serialization.Json;
using Light.Serialization.Json.Caching;
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

            var result = cache.CheckJsonTokenTypeCombinationForBlacklist(jsonTokenTypeCombination);

            result.ShouldBeEquivalentTo(true);
        }

        [Fact(DisplayName = "Validate that blacklisted JsonToken and Type are found in the blacklist.")]
        public void ValidateBlacklistedJsonTokenAndType()
        {
            var jsonToken = new JsonToken(new char[1], 0, 0, JsonTokenType.BeginOfArray);
            var type = typeof(int);
            var jsonTokenTypeCombination = new JsonTokenTypeCombination(jsonToken, type);
            var cache = new JsonTokenParserCache(new List<JsonTokenTypeCombination> { jsonTokenTypeCombination });

            var result = cache.CheckJsonTokenTypeForBlacklist(jsonToken, type);

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
            var result = cache.CheckJsonTokenTypeForBlacklist(jsonToken, typeLong);
            
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
            var result = cache.CheckJsonTokenTypeForBlacklist(jsonTokenToCompare, typeInt);

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
            var result = cache.CheckJsonTokenTypeCombinationForBlacklist(jsonTokenTypeCombinationToCompare);

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
            var result = cache.CheckJsonTokenTypeForBlacklist(jsonTokenToCompare, typeChar);

            //assert
            result.ShouldBeEquivalentTo(false);
        }

    }
}
