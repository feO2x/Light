using System.Collections.Generic;
using System.Linq;
using Light.GuardClauses;
using Light.Serialization.Json.ComplexTypeConstruction;
using Light.Serialization.Json.TokenParsers;
using Light.Serialization.Json.TypeNaming;

namespace Light.Serialization.Json
{
    public static class JsonDeserializerBuilderExtensions
    {
        public static TCollection AddDefaultTokenParsers<TCollection>(this TCollection targetList,
                                                                      ICollectionFactory collectionFactory,
                                                                      IObjectFactory objectFactory,
                                                                      ITypeSectionParser typeSectionParser,
                                                                      IInjectableValueNameNormalizer nameNormalizer,
                                                                      ITypeDescriptionProvider typeDescriptionProvider)
            where TCollection : IList<IJsonTokenParser>
        {
            targetList.MustNotBeNull(nameof(targetList));

            targetList.Add(new UnsignedIntegerParser());
            targetList.Add(new SignedIntegerParser());
            targetList.Add(new DateTimeParser());
            targetList.Add(new TimeSpanParser());
            targetList.Add(new DateTimeOffsetParser());
            targetList.Add(new FloatParser());
            targetList.Add(new DecimalParser());
            targetList.Add(new DoubleParser());
            targetList.Add(new BooleanParser());
            targetList.Add(new NullParser());
            targetList.Add(new CharacterParser());
            targetList.Add(new EnumerationValueParser());
            var stringParser = new StringParser();
            targetList.Add(stringParser);
            targetList.Add(new JsonStringInheritenceParser(targetList.OfType<IJsonStringToPrimitiveParser>().ToList(), stringParser));
            targetList.Add(new ArrayToGenericCollectionParser(collectionFactory));
            targetList.Add(new ComplexTypeParser(objectFactory, nameNormalizer, typeDescriptionProvider, typeSectionParser));

            return targetList;
        }
    }
}