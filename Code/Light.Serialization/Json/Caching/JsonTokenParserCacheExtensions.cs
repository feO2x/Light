using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Light.GuardClauses;
using Light.Serialization.Json.TokenParsers;

namespace Light.Serialization.Json.Caching
{
    public static class JsonTokenParserCacheExtensions
    {
        public static TCollection AddDefaultJsonTokenAndTypeCombinationsToBlacklist<TCollection>(
            this TCollection blackList)
            where TCollection : IList<JsonTokenTypeCombination>
        {
            blackList.MustNotBeNull(nameof(blackList));

            blackList.Add(new JsonTokenTypeCombination(JsonTokenType.String, typeof(object)));
            blackList.Add(new JsonTokenTypeCombination(JsonTokenType.String, typeof(JsonStringParserOrchestrator)));

            return blackList;
        }
    }
}
