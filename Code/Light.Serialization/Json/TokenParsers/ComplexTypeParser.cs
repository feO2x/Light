using System;
using System.Collections.Generic;
using Light.GuardClauses;
using Light.Serialization.Json.ComplexTypeConstruction;
using Light.Serialization.Json.LowLevelReading;
using Light.Serialization.Json.TypeNaming;

namespace Light.Serialization.Json.TokenParsers
{
    public sealed class ComplexTypeParser : IJsonTokenParser
    {
        private readonly IInjectableValueNameNormalizer _nameNormalizer;
        private readonly IObjectFactory _objectFactory;
        private readonly ITypeDescriptionProvider _typeDescriptionProvider;
        private readonly ITypeSectionParser _typeSectionParser;


        public ComplexTypeParser(IObjectFactory objectFactory,
                                 IInjectableValueNameNormalizer nameNormalizer,
                                 ITypeDescriptionProvider typeDescriptionProvider,
                                 ITypeSectionParser typeSectionParser)
        {
            objectFactory.MustNotBeNull(nameof(objectFactory));
            nameNormalizer.MustNotBeNull(nameof(nameNormalizer));
            typeDescriptionProvider.MustNotBeNull(nameof(typeDescriptionProvider));
            typeSectionParser.MustNotBeNull(nameof(typeSectionParser));

            _objectFactory = objectFactory;
            _nameNormalizer = nameNormalizer;
            _typeDescriptionProvider = typeDescriptionProvider;
            _typeSectionParser = typeSectionParser;
        }

        public bool IsSuitableFor(JsonToken token, Type requestedType)
        {
            return token.JsonType == JsonTokenType.BeginOfObject;
        }

        public object ParseValue(JsonDeserializationContext context)
        {
            var jsonReader = context.JsonReader;
            var currentLabelToken = jsonReader.ReadNextToken();

            // If the first token is the end of the complex object, then there are no child values
            if (currentLabelToken.JsonType == JsonTokenType.EndOfObject)
                return _objectFactory.Create(_typeDescriptionProvider.GetTypeCreationDescription(context.RequestedType), null);

            // If it's not the end of the object, then there must be a string label
            if (currentLabelToken.JsonType != JsonTokenType.String)
                throw new JsonDocumentException($"Expected JSON string or end of complex JSON object, but found {currentLabelToken}", currentLabelToken);

            var firstTokenString = context.DeserializeToken<string>(currentLabelToken);
            Dictionary<InjectableValueDescription, object> deserializedChildValues;
            TypeCreationDescription typeCreationDescription;

            // Check if the first string marks the actual type that should be used for deserializing
            if (firstTokenString == _typeSectionParser.ConcreteTypeSymbol)
            {
                jsonReader.ReadAndExpectPairDelimiterToken();

                var targetType = _typeSectionParser.ParseTypeSection(context);
                typeCreationDescription = _typeDescriptionProvider.GetTypeCreationDescription(targetType);
                // TODO: here we might have to switch to to a DictionaryParser if we read a corresponding type
                // It might be useful to return a more complex type than just type

                // If the complex object ends here, then just create the target object using the factory
                // There are no more label value pairs in this object to be deserialized
                if (jsonReader.ReadAndExpectEndOfObjectOrValueDelimiter() == JsonTokenType.EndOfObject)
                    return _objectFactory.Create(typeCreationDescription, null);

                // Otherwise create a dictionary for the child values
                deserializedChildValues = new Dictionary<InjectableValueDescription, object>();
            }
            // If not then the first label corresponds to a value that must be injected into the object to be created
            else
            {
                deserializedChildValues = new Dictionary<InjectableValueDescription, object>();

                var typeToConstruct = context.RequestedType;
                typeCreationDescription = _typeDescriptionProvider.GetTypeCreationDescription(typeToConstruct);

                goto DeserializeAfterReadingKey;
            }

            // At this point, there must be definitely another label value pair in the complex JSON object
            DeserializeKeyValuePair:
            currentLabelToken = jsonReader.ReadNextToken();
            if (currentLabelToken.JsonType != JsonTokenType.String)
                throw new JsonDocumentException($"Expected JSON string or end of complex JSON object, but found {currentLabelToken}", currentLabelToken);

            DeserializeAfterReadingKey:
            var label = context.DeserializeToken<string>(currentLabelToken);
            var normalizedLabel = _nameNormalizer.Normalize(label);
            var injectableValueInfo = typeCreationDescription.GetInjectableValueDescriptionFromNormalizedName(normalizedLabel) ??
                                      InjectableValueDescription.FromUnknownValue(normalizedLabel, typeof(object));

            jsonReader.ReadAndExpectPairDelimiterToken();

            var valueToken = jsonReader.ReadNextToken();
            var value = context.DeserializeToken(valueToken, injectableValueInfo.Type);

            deserializedChildValues.Add(injectableValueInfo, value);

            if (jsonReader.ReadAndExpectEndOfObjectOrValueDelimiter() == JsonTokenType.EndOfObject)
                return _objectFactory.Create(typeCreationDescription, deserializedChildValues);

            goto DeserializeKeyValuePair;
        }
    }
}