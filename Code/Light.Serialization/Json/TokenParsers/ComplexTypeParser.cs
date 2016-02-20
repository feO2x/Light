using System;
using System.Collections.Generic;
using Light.GuardClauses;
using Light.Serialization.Json.ComplexTypeConstruction;

namespace Light.Serialization.Json.TokenParsers
{
    public sealed class ComplexTypeParser : IJsonTokenParser
    {
        private readonly IInjectableValueNameNormalizer _nameNormalizer;
        private readonly INameToTypeMapping _nameToTypeMapping;
        private readonly IObjectFactory _objectFactory;
        private readonly Type _objectType = typeof (object);
        private readonly Type _stringType = typeof (string);
        private readonly ITypeDescriptionProvider _typeDescriptionProvider;
        private string _actualTypeSymbol = "$type";


        public ComplexTypeParser(IObjectFactory objectFactory,
                                 INameToTypeMapping nameToTypeMapping,
                                 IInjectableValueNameNormalizer nameNormalizer,
                                 ITypeDescriptionProvider typeDescriptionProvider)
        {
            objectFactory.MustNotBeNull(nameof(objectFactory));
            nameToTypeMapping.MustNotBeNull(nameof(nameToTypeMapping));
            nameNormalizer.MustNotBeNull(nameof(nameNormalizer));
            typeDescriptionProvider.MustNotBeNull(nameof(typeDescriptionProvider));

            _objectFactory = objectFactory;
            _nameToTypeMapping = nameToTypeMapping;
            _nameNormalizer = nameNormalizer;
            _typeDescriptionProvider = typeDescriptionProvider;
        }

        public string ActualTypeSymbol
        {
            get { return _actualTypeSymbol; }
            set
            {
                value.MustNotBeNullOrEmpty(nameof(value));
                _actualTypeSymbol = value;
            }
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

            var firstTokenString = (string) context.DeserializeToken(currentLabelToken, _stringType);
            Dictionary<InjectableValueDescription, object> deserializedChildValues;
            TypeCreationDescription typeCreationDescription;

            // Check if the first string marks the actual type that should be used for deserializing
            if (firstTokenString == _actualTypeSymbol)
            {
                ReadAndExpectPairDelimiterToken(jsonReader);

                var typeStringToken = jsonReader.ReadNextToken();
                if (typeStringToken.JsonType != JsonTokenType.String)
                    throw new JsonDocumentException($"Expected JSON string containing the type name for deserialization, but found {typeStringToken}", typeStringToken);

                var typeToConstruct = _nameToTypeMapping.Map((string) context.DeserializeToken(typeStringToken, _stringType));
                typeCreationDescription = _typeDescriptionProvider.GetTypeCreationDescription(typeToConstruct);

                // If the complex object ends here, then just create the target object using the factory
                // There are no more label value pairs in this object to be deserialized
                if (ReadAndExpectEndOfObjectOrValueDelimiter(jsonReader) == JsonTokenType.EndOfObject)
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

                goto DeserializeAfterReadingLabel;
            }

            // At this point, there must be definitely another label value pair in the complex JSON object
            DeserializeLabelValuePair:
            currentLabelToken = jsonReader.ReadNextToken();
            if (currentLabelToken.JsonType != JsonTokenType.String)
                throw new JsonDocumentException($"Expected JSON string or end of complex JSON object, but found {currentLabelToken}", currentLabelToken);

            DeserializeAfterReadingLabel:
            var label = (string) context.DeserializeToken(currentLabelToken, _stringType);
            var normalizedLabel = _nameNormalizer.Normalize(label);
            var injectableValueInfo = typeCreationDescription.GetInjectableValueDescriptionFromNormalizedName(normalizedLabel) ??
                                      InjectableValueDescription.FromUnknownValue(normalizedLabel, _objectType);

            ReadAndExpectPairDelimiterToken(jsonReader);

            var valueToken = jsonReader.ReadNextToken();
            var value = context.DeserializeToken(valueToken, injectableValueInfo.Type);

            deserializedChildValues.Add(injectableValueInfo, value);

            if (ReadAndExpectEndOfObjectOrValueDelimiter(jsonReader) == JsonTokenType.EndOfObject)
                return _objectFactory.Create(typeCreationDescription, deserializedChildValues);

            goto DeserializeLabelValuePair;
        }

        private static void ReadAndExpectPairDelimiterToken(IJsonReader reader)
        {
            var pairDelimiterToken = reader.ReadNextToken();
            if (pairDelimiterToken.JsonType != JsonTokenType.PairDelimiter)
                throw new JsonDocumentException($"Expected delimiter between label and value in complex JSON object, but found {pairDelimiterToken}", pairDelimiterToken);
        }

        private static JsonTokenType ReadAndExpectEndOfObjectOrValueDelimiter(IJsonReader reader)
        {
            var valueDelimiterOrEndOfObjectToken = reader.ReadNextToken();
            if (valueDelimiterOrEndOfObjectToken.JsonType == JsonTokenType.EndOfObject)
                return JsonTokenType.EndOfObject;

            if (valueDelimiterOrEndOfObjectToken.JsonType != JsonTokenType.ValueDelimiter)
                throw new JsonDocumentException($"Expected value delimiter or end of complex JSON object, but found {valueDelimiterOrEndOfObjectToken}", valueDelimiterOrEndOfObjectToken);

            return JsonTokenType.ValueDelimiter;
        }
    }
}