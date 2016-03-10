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
            return CheckTokenTypeCombinationForBlacklist(new JsonTokenTypeCombination(jsonToken, type));
        }

        public bool CheckTokenTypeCombinationForBlacklist(JsonTokenTypeCombination jsonTokenTypeCombination)
        {
            jsonTokenTypeCombination.MustNotBeNull(nameof(jsonTokenTypeCombination));

            return _jsonTokenTypeCombinationCacheBlackList.Contains(jsonTokenTypeCombination);
        }

        public bool TryGetTokenParser(JsonToken jsonToken, Type type, out IJsonTokenParser jsonTokenParser)
        {
            jsonToken.MustNotBeNull(nameof(jsonToken));
            type.MustNotBeNull(nameof(type));

            var jsonTokenTypeCombination = new JsonTokenTypeCombination(jsonToken, type);

            return TryGetTokenParser(jsonTokenTypeCombination, out jsonTokenParser);
        }

        public bool TryGetTokenParser(JsonTokenTypeCombination jsonTokenTypeCombination, out IJsonTokenParser jsonTokenParser)
        {
            jsonTokenTypeCombination.MustNotBeNull(nameof(jsonTokenTypeCombination));

            jsonTokenParser = null;

            if (CheckTokenTypeCombinationForBlacklist(jsonTokenTypeCombination) || _jsonTokenParsers.ContainsKey(jsonTokenTypeCombination) == false)
                return false;
                
            if(_jsonTokenParsers.TryGetValue(jsonTokenTypeCombination, out jsonTokenParser) == false)
                throw new KeyNotFoundException($"Combination JsonToken {nameof(jsonTokenTypeCombination.JsonToken)} and Type {nameof(jsonTokenTypeCombination.Type)} not cached.");

            return true;
        }

        public bool TryAddTokenParserToCache(JsonTokenTypeCombination jsonTokenTypeCombination,
            IJsonTokenParser jsonTokenParser)
        {
            jsonTokenTypeCombination.MustNotBeNull(nameof(jsonTokenTypeCombination));
            jsonTokenParser.MustNotBeNull(nameof(jsonTokenParser));

            if (CheckTokenTypeCombinationForBlacklist(jsonTokenTypeCombination) || _jsonTokenParsers.ContainsKey(jsonTokenTypeCombination))
                return false;

            _jsonTokenParsers.Add(jsonTokenTypeCombination, jsonTokenParser);

            return true;
        }
    }
}
