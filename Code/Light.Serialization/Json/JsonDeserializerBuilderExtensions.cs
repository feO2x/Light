using System.Collections.Generic;
using Light.GuardClauses;
using Light.Serialization.Json.ComplexTypeConstruction;
using Light.Serialization.Json.LowLevelReading;
using Light.Serialization.Json.TokenParsers;

namespace Light.Serialization.Json
{
    public static class JsonDeserializerBuilderExtensions
    {
        public static TCollection AddDefaultTokenParsers<TCollection>(this TCollection targetList,
                                                                      JsonReaderSymbols jsonReaderSymbols,
                                                                      ICollectionFactory collectionFactory,
                                                                      IObjectFactory objectFactory,
                                                                      INameToTypeMapping nameToTypeMapping,
                                                                      IInjectableValueNameNormalizer nameNormalizer,
                                                                      ITypeDescriptionProvider typeDescriptionProvider)
            where TCollection : IList<IJsonTokenParser>
        {
            targetList.MustNotBeNull(nameof(targetList));

            targetList.Add(new IntParser());
            targetList.Add(new DateTimeParser());
            targetList.Add(new TimeSpanParser());
            targetList.Add(new UIntParser());
            targetList.Add(new ShortParser());
            targetList.Add(new UShortParser());
            targetList.Add(new ByteParser());
            targetList.Add(new SByteParser());
            targetList.Add(new LongParser());
            targetList.Add(new StringParser(jsonReaderSymbols));
            targetList.Add(new DoubleParser());
            targetList.Add(new FloatParser());
            targetList.Add(new DecimalParser());
            targetList.Add(new NullParser());
            targetList.Add(new CharacterParser(jsonReaderSymbols));
            targetList.Add(new BooleanParser());
            targetList.Add(new EnumerationValueParser());
            targetList.Add(new ArrayToGenericCollectionParser(collectionFactory));
            targetList.Add(new ComplexTypeParser(objectFactory, nameToTypeMapping, nameNormalizer, typeDescriptionProvider));

            return targetList;
        }
    }
}