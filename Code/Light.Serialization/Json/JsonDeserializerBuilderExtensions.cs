using System.Collections.Generic;
using System.Linq;
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

            targetList.Add(new UnsignedIntegerParser());
            targetList.Add(new SignedIntegerParser());
            targetList.Add(new DateTimeParser());
            targetList.Add(new TimeSpanParser());
            targetList.Add(new FloatParser());
            targetList.Add(new DecimalParser());
            targetList.Add(new DoubleParser());
            targetList.Add(new BooleanParser());
            targetList.Add(new NullParser());
            targetList.Add(new CharacterParser(jsonReaderSymbols));
            targetList.Add(new EnumerationValueParser());
            var stringParser = new StringParser(jsonReaderSymbols);
            targetList.Add(stringParser);
            targetList.Add(new JsonStringParserOrchestrator(targetList.OfType<IJsonStringToPrimitiveParser>().ToList(), stringParser));
            targetList.Add(new ArrayToGenericCollectionParser(collectionFactory));
            targetList.Add(new ComplexTypeParser(objectFactory, nameToTypeMapping, nameNormalizer, typeDescriptionProvider));

            return targetList;
        }
    }
}