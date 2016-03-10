using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Light.GuardClauses;

namespace Light.Serialization.Json.Caching
{
    public class TokenParserCacheDecorator
    {
        private IList<JsonTokenTypeCombination> _jsonTokenTypeCombinationCacheBlackList; 

        public TokenParserCacheDecorator(IList<JsonTokenTypeCombination> jsonTokenTypeCombinationCacheBlackList)
        {
            jsonTokenTypeCombinationCacheBlackList.MustNotBeNull(nameof(jsonTokenTypeCombinationCacheBlackList));

            _jsonTokenTypeCombinationCacheBlackList = jsonTokenTypeCombinationCacheBlackList;
        }

        public bool CheckJsonTokenTypeForBlacklist(JsonToken jsonToken, Type type)
        {
            jsonToken.MustNotBeNull(nameof(jsonToken));
            type.MustNotBeNull(nameof(type));
            CheckJsonTokenTypeCombinationForBlacklist(new JsonTokenTypeCombination(jsonToken, type));
        }

        public bool CheckJsonTokenTypeCombinationForBlacklist(JsonTokenTypeCombination jsonTokenTypeCombination)
        {
            
        }
    }
}
