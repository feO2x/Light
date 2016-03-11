using System;
using System.Collections.Generic;
using Light.GuardClauses;
using Light.Serialization.Caching;

namespace Light.Serialization.Json.Caching
{
    public class JsonTokenParserCache : ITokenParserCache
    {
        private readonly IList<JsonTokenTypeCombination> _jsonTokenTypeCombinationCacheBlackList;
        private readonly IDictionary<JsonTokenTypeCombination, IJsonTokenParser> _jsonTokenParsers = new Dictionary<JsonTokenTypeCombination, IJsonTokenParser>(); 

        public JsonTokenParserCache(IList<JsonTokenTypeCombination> jsonTokenTypeCombinationCacheBlackList)
        {
            jsonTokenTypeCombinationCacheBlackList.MustNotBeNull(nameof(jsonTokenTypeCombinationCacheBlackList));

            _jsonTokenTypeCombinationCacheBlackList = jsonTokenTypeCombinationCacheBlackList;
        }

        public bool CheckTokenTypeForBlacklist(JsonToken jsonToken, Type type)
        {
            jsonToken.MustNotBeNull(nameof(jsonToken));
            type.MustNotBeNull(nameof(type));

            return _jsonTokenTypeCombinationCacheBlackList.Contains(new JsonTokenTypeCombination(jsonToken.JsonType, type));
        }

        public bool TryGetTokenParser(JsonToken jsonToken, Type type, out IJsonTokenParser jsonTokenParser)
        {
            jsonToken.MustNotBeNull(nameof(jsonToken));
            type.MustNotBeNull(nameof(type));

            jsonTokenParser = null;
            var jsonTokenTypeCombination = new JsonTokenTypeCombination(jsonToken.JsonType, type);

            if (CheckTokenTypeForBlacklist(jsonToken, type) || _jsonTokenParsers.ContainsKey(jsonTokenTypeCombination) == false)
                return false;
                
            if(_jsonTokenParsers.TryGetValue(jsonTokenTypeCombination, out jsonTokenParser) == false)
                throw new KeyNotFoundException($"Combination JsonToken {nameof(jsonTokenTypeCombination.JsonTokenType)} and Type {nameof(jsonTokenTypeCombination.Type)} not cached.");

            return true;
        }

        public bool TryAddTokenParserToCache(JsonToken jsonToken, Type type,
            IJsonTokenParser jsonTokenParser)
        {
            jsonToken.MustNotBeNull(nameof(jsonToken));
            type.MustNotBeNull(nameof(type));
            jsonTokenParser.MustNotBeNull(nameof(jsonTokenParser));

            var jsonTokenTypeCombination = new JsonTokenTypeCombination(jsonToken.JsonType, type);

            if (CheckTokenTypeForBlacklist(jsonToken, type) || _jsonTokenParsers.ContainsKey(jsonTokenTypeCombination))
                return false;

            _jsonTokenParsers.Add(jsonTokenTypeCombination, jsonTokenParser);

            return true;
        }
    }
}
