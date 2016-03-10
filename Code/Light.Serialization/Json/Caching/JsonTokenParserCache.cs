using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Light.GuardClauses;

namespace Light.Serialization.Json.Caching
{
    public class JsonTokenParserCache
    {
        private readonly IList<JsonTokenTypeCombination> _jsonTokenTypeCombinationCacheBlackList;
        private IDictionary<JsonTokenTypeCombination, IJsonTokenParser> _jsonTokenParsers = new Dictionary<JsonTokenTypeCombination, IJsonTokenParser>(); 

        public JsonTokenParserCache(IList<JsonTokenTypeCombination> jsonTokenTypeCombinationCacheBlackList)
        {
            jsonTokenTypeCombinationCacheBlackList.MustNotBeNull(nameof(jsonTokenTypeCombinationCacheBlackList));

            _jsonTokenTypeCombinationCacheBlackList = jsonTokenTypeCombinationCacheBlackList;
        }

        public bool CheckJsonTokenTypeForBlacklist(JsonToken jsonToken, Type type)
        {
            jsonToken.MustNotBeNull(nameof(jsonToken));
            type.MustNotBeNull(nameof(type));
            return CheckJsonTokenTypeCombinationForBlacklist(new JsonTokenTypeCombination(jsonToken, type));
        }

        public bool CheckJsonTokenTypeCombinationForBlacklist(JsonTokenTypeCombination jsonTokenTypeCombination)
        {
            jsonTokenTypeCombination.MustNotBeNull(nameof(jsonTokenTypeCombination));

            return _jsonTokenTypeCombinationCacheBlackList.Contains(jsonTokenTypeCombination);
        }

        public bool TryGetJsonTokenParser(JsonTokenTypeCombination jsonTokenTypeCombination, out IJsonTokenParser jsonTokenParser)
        {
            jsonTokenTypeCombination.MustNotBeNull(nameof(jsonTokenTypeCombination));

            jsonTokenParser = null;

            if (CheckJsonTokenTypeCombinationForBlacklist(jsonTokenTypeCombination) || _jsonTokenParsers.ContainsKey(jsonTokenTypeCombination) == false)
                return false;
                
            if(_jsonTokenParsers.TryGetValue(jsonTokenTypeCombination, out jsonTokenParser) == false)
                throw new KeyNotFoundException($"Combination JsonToken {nameof(jsonTokenTypeCombination.JsonToken)} and Type {nameof(jsonTokenTypeCombination.Type)} not cached.");

            return true;
        }

        public bool TryAddJsonTokenParserToCache(JsonTokenTypeCombination jsonTokenTypeCombination,
            IJsonTokenParser jsonTokenParser)
        {
            jsonTokenTypeCombination.MustNotBeNull(nameof(jsonTokenTypeCombination));
            jsonTokenParser.MustNotBeNull(nameof(jsonTokenParser));

            if (CheckJsonTokenTypeCombinationForBlacklist(jsonTokenTypeCombination) || _jsonTokenParsers.ContainsKey(jsonTokenTypeCombination))
                return false;

            _jsonTokenParsers.Add(jsonTokenTypeCombination, jsonTokenParser);

            return true;
        }
    }
}
