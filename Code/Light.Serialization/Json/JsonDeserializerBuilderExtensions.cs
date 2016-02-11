using System.Collections.Generic;
using Light.GuardClauses;
using Light.Serialization.Json.LowLevelReading;
using Light.Serialization.Json.TokenParsers;

namespace Light.Serialization.Json
{
    public static class JsonDeserializerBuilderExtensions
    {
        public static IList<IJsonTokenParser> AddDefaultTokenParsers(this IList<IJsonTokenParser> targetList,
                                                                     JsonReaderSymbols jsonReaderSymbols,
                                                                     ICollectionFactory collectionFactory)
        {
            targetList.MustNotBeNull(nameof(targetList));

            targetList.Add(new IntParser());
            targetList.Add(new StringParser(jsonReaderSymbols));
            targetList.Add(new DoubleParser());
            targetList.Add(new NullParser());
            targetList.Add(new CharacterParser(jsonReaderSymbols));
            targetList.Add(new BooleanParser());
            targetList.Add(new ArrayToGenericCollectionParser(collectionFactory));

            return targetList;
        }
    }
}