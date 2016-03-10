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
        private IList<JsonTokenTypeCombination> _jsonTokenTypeCombinationCacheBlackList; 

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


    }
}
