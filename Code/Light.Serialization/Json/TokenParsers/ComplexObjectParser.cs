using System;
using System.Collections.Generic;

namespace Light.Serialization.Json.TokenParsers
{
    public sealed class ComplexObjectParser : IJsonTokenParser
    {
        private readonly INameToTypeMapping _nameToTypeMapping;
        private readonly IObjectFactory _objectFactory;
        private readonly Type _objectType = typeof (object);
        private string _actualTypeSymbol = "$type";
        private readonly ITypeCreationInfoAnalyzer _typeAnalyzer;


        public ComplexObjectParser(IObjectFactory objectFactory,
                                   INameToTypeMapping nameToTypeMapping,
                                   ITypeCreationInfoAnalyzer typeAnalyzer)
        {
            if (objectFactory == null) throw new ArgumentNullException(nameof(objectFactory));
            if (nameToTypeMapping == null) throw new ArgumentNullException(nameof(nameToTypeMapping));
            if (typeAnalyzer == null) throw new ArgumentNullException(nameof(typeAnalyzer));

            _objectFactory = objectFactory;
            _nameToTypeMapping = nameToTypeMapping;
            _typeAnalyzer = typeAnalyzer;
        }

        public string ActualTypeSymbol
        {
            get { return _actualTypeSymbol; }
            set
            {
                if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(value));

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
            var firstToken = jsonReader.ReadNextToken();

            // If the first token is the end of the complex object, then there are no child values
            if (firstToken.JsonType == JsonTokenType.EndOfObject)
                return _objectFactory.Create(_typeAnalyzer.CreateInfo(context.RequestedType), null);

            // If it's not the end of the object, then there must be a string label
            if (firstToken.JsonType != JsonTokenType.String)
                throw new JsonDocumentException($"Expected JSON string or end of complex JSON object, but found {firstToken}", firstToken);

            var firstTokenString = firstToken.ToStringWithoutQuotationMarks();
            Dictionary<InjectableValueInfo, object> deserializedChildValues;
            TypeConstructionInfo typeConstructionInfo;

            // Check if the first string marks the actual type that should be used for deserializing
            if (firstTokenString == _actualTypeSymbol)
            {
                ReadAndExpectPairDelimiterToken(jsonReader);

                var typeStringToken = jsonReader.ReadNextToken();
                if (typeStringToken.JsonType != JsonTokenType.String)
                    throw new JsonDocumentException($"Expected JSON string containing the type name for deserialization, but found {typeStringToken}", typeStringToken);

                var typeToConstruct = _nameToTypeMapping.Map(typeStringToken.ToStringWithoutQuotationMarks());
                typeConstructionInfo = _typeAnalyzer.CreateInfo(typeToConstruct);

                // If the complex object ends here, then just create the target object using the factory
                // There are no more label value pairs in this object to be deserialized
                if (ReadAndExpectEndOfObjectOrValueDelimiter(jsonReader) == JsonTokenType.EndOfObject)
                    return _objectFactory.Create(typeConstructionInfo, null);

                deserializedChildValues = new Dictionary<InjectableValueInfo, object>();
            }
            // If not then the first label corresponds to a value that must be injected into the object to be created
            else
            {
                deserializedChildValues = new Dictionary<InjectableValueInfo, object>();

                var typeToConstruct = context.RequestedType;
                typeConstructionInfo = _typeAnalyzer.CreateInfo(typeToConstruct);

                var injectableValueInfo = typeConstructionInfo.GetInjectableValueInfoFromName(firstTokenString) ??
                                          new InjectableValueInfo(firstTokenString, firstTokenString, _objectType, InjectableValueKind.UnknownOnTargetObject);

                ReadAndExpectPairDelimiterToken(jsonReader);

                var valueToken = jsonReader.ReadNextToken();
                var value = context.DeserializeToken(valueToken, injectableValueInfo.Type);

                deserializedChildValues.Add(injectableValueInfo, value);

                if (ReadAndExpectEndOfObjectOrValueDelimiter(jsonReader) == JsonTokenType.EndOfObject)
                    return _objectFactory.Create(typeConstructionInfo, deserializedChildValues);
            }

            // At this point, there must be definitely another label value pair in the complex JSON object
            while (true)
            {
                var labelToken = jsonReader.ReadNextToken();
                if (labelToken.JsonType != JsonTokenType.String)
                    throw new JsonDocumentException($"Expected JSON string or end of complex JSON object, but found {labelToken}", labelToken);

                var injectableValueName = labelToken.ToStringWithoutQuotationMarks();
                var injectableValueInfo = typeConstructionInfo.GetInjectableValueInfoFromName(injectableValueName) ??
                                          new InjectableValueInfo(injectableValueName, injectableValueName, _objectType, InjectableValueKind.UnknownOnTargetObject);

                ReadAndExpectPairDelimiterToken(jsonReader);

                var valueToken = jsonReader.ReadNextToken();
                var value = context.DeserializeToken(valueToken, injectableValueInfo.Type);

                deserializedChildValues.Add(injectableValueInfo, value);

                if (ReadAndExpectEndOfObjectOrValueDelimiter(jsonReader) == JsonTokenType.EndOfObject)
                    return _objectFactory.Create(typeConstructionInfo, deserializedChildValues);
            }
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