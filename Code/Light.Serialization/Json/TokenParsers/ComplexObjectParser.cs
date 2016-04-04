using System;
using System.Collections.Generic;
using Light.GuardClauses;
using Light.Serialization.Json.ComplexTypeConstruction;
using Light.Serialization.Json.LowLevelReading;
using Light.Serialization.Json.ObjectReferencePreservation;
using Light.Serialization.Json.TypeNaming;

namespace Light.Serialization.Json.TokenParsers
{
    public sealed class ComplexObjectParser : IJsonTokenParser
    {
        private readonly IInjectableValueNameNormalizer _nameNormalizer;
        private readonly IObjectFactory _objectFactory;
        private readonly ITypeDescriptionProvider _typeDescriptionProvider;
        private readonly ITypeSectionParser _typeSectionParser;
        private readonly IObjectDeserializationReferencePreserver _referencePreserver;
        int? _preservationIdentifier;

        public ComplexObjectParser(IObjectFactory objectFactory,
                                   IInjectableValueNameNormalizer nameNormalizer,
                                   ITypeDescriptionProvider typeDescriptionProvider,
                                   ITypeSectionParser typeSectionParser,
                                   IObjectDeserializationReferencePreserver referencePreserver)
        {
            objectFactory.MustNotBeNull(nameof(objectFactory));
            nameNormalizer.MustNotBeNull(nameof(nameNormalizer));
            typeDescriptionProvider.MustNotBeNull(nameof(typeDescriptionProvider));
            typeSectionParser.MustNotBeNull(nameof(typeSectionParser));
            referencePreserver.MustNotBeNull(nameof(referencePreserver));

            _objectFactory = objectFactory;
            _nameNormalizer = nameNormalizer;
            _typeDescriptionProvider = typeDescriptionProvider;
            _typeSectionParser = typeSectionParser;
            _referencePreserver = referencePreserver;
        }

        public bool CanBeCached => false;

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
                throw new JsonDocumentException($"Expected JSON string or end of complex JSON object, but found {currentLabelToken}.", currentLabelToken);

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

            if (label == "$ref" || label == "$id")
            {
                jsonReader.ReadAndExpectPairDelimiterToken();
                var token = jsonReader.ReadNextToken();
                _preservationIdentifier = (int) context.DeserializeToken(token, typeof (int));

                if (label == "$ref")
                {
                    object referenceObject;

                    if(_referencePreserver.TryGetReference(_preservationIdentifier.Value, out referenceObject))
                    {
                        jsonReader.ReadAndExpectEndOfObjectOrValueDelimiter();
                        return referenceObject;
                    }

                    throw new Exception($"Expected that the referencePreserver holds a reference to id {_preservationIdentifier.Value}, but the referencePreserver don't hold the expected reference.");
                }

                jsonReader.ReadAndExpectEndOfObjectOrValueDelimiter();
                goto DeserializeKeyValuePair;
            }

            var normalizedLabel = _nameNormalizer.Normalize(label);

            var injectableValueInfo = typeCreationDescription.GetInjectableValueDescriptionFromNormalizedName(normalizedLabel) ??
                                      InjectableValueDescription.FromUnknownValue(normalizedLabel, typeof (object));

            jsonReader.ReadAndExpectPairDelimiterToken();

            var valueToken = jsonReader.ReadNextToken();
            valueToken.ExpectBeginOfValue();
            var value = context.DeserializeToken(valueToken, injectableValueInfo.Type);

            deserializedChildValues.Add(injectableValueInfo, value);

            if (jsonReader.ReadAndExpectEndOfObjectOrValueDelimiter() == JsonTokenType.EndOfObject)
            { 
                var complexObject = _objectFactory.Create(typeCreationDescription, deserializedChildValues);
                if(_preservationIdentifier != null)
                    _referencePreserver.AddReference(_preservationIdentifier.Value, complexObject);

                return complexObject;
            }

            goto DeserializeKeyValuePair;
        }
    }
}