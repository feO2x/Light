using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var cache = new TokenParserCacheDecorator(new List<JsonTokenTypeCombination> {jsonTokenTypeCombination });

            var result = cache.CheckJsonTokenTypeCombinationForBlacklist(jsonTokenTypeCombination);

            result.ShouldBeEquivalentTo(true);
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

            var cache = new TokenParserCacheDecorator(new List<JsonTokenTypeCombination> { jsonTokenTypeCombinationForBlacklist });

            //act
            var result = cache.CheckJsonTokenTypeCombinationForBlacklist(jsonTokenTypeCombinationToCompare);

            //assert
            result.ShouldBeEquivalentTo(false);
        }
    }
}
