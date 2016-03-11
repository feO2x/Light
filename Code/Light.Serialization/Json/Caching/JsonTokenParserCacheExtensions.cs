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

        public static TCollection AddJsonTokenAndTypeCombinationToBlacklist<TCollection>(
            this TCollection blackList, IList<JsonTokenTypeCombination> jsonTokenTypeCombinations)
            where TCollection : IList<JsonTokenTypeCombination>
        {
            blackList.MustNotBeNull(nameof(blackList));
            jsonTokenTypeCombinations.MustNotBeNull(nameof(jsonTokenTypeCombinations));

            foreach (var jsonTokenTypeCombination in jsonTokenTypeCombinations)
            {
                blackList.Add(jsonTokenTypeCombination);
            }

            return blackList;
        }
    }
}
