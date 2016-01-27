using System;
using System.Collections.Generic;

namespace Light.Serialization.Json.TokenParsers
{
    public sealed class ComplexObjectParser : IJsonTokenParser
    {
        private readonly INameToTypeMapping _nameToTypeMapping;
        private readonly IObjectFactory _objectFactory;
        private readonly Type _objectType = typeof (object);
        private readonly IDictionary<Type, TypeConstructionInfo> _typeConstructionInfoCache;
        private string _actualTypeSymbol = "$type";
        private readonly ITypeCreationInfoAnalyzer _typeAnalyzer;


        public ComplexObjectParser(IObjectFactory objectFactory,
                                   INameToTypeMapping nameToTypeMapping,
                                   IDictionary<Type, TypeConstructionInfo> typeConstructionInfoCache,
                                   ITypeCreationInfoAnalyzer typeAnalyzer)
        {
            if (objectFactory == null) throw new ArgumentNullException(nameof(objectFactory));
            if (nameToTypeMapping == null) throw new ArgumentNullException(nameof(nameToTypeMapping));
            if (typeConstructionInfoCache == null) throw new ArgumentNullException(nameof(typeConstructionInfoCache));
            if (typeAnalyzer == null) throw new ArgumentNullException(nameof(typeAnalyzer));

            _objectFactory = objectFactory;
            _nameToTypeMapping = nameToTypeMapping;
            _typeConstructionInfoCache = typeConstructionInfoCache;
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
            var deserializedChildValues = new Dictionary<InjectableValueInfo, object>();
            var typeToConstruct = context.RequestedType;

            var firstToken = context.JsonReader.ReadNextToken();

            // If the first token is the end of the complex object, then there are no child values
            if (firstToken.JsonType == JsonTokenType.EndOfObject)
                return _objectFactory.Create(context.RequestedType, deserializedChildValues);

            // If it's not the end of the object, then there must be a string label
            if (firstToken.JsonType != JsonTokenType.String)
                throw new JsonDocumentException($"Expected JSON string or end of complex JSON object, but found {firstToken}", firstToken);

            var @string = firstToken.ToString();

            // Check if the first string marks the actual type that should be used for deserializing
            if (@string == _actualTypeSymbol)
            {
                var pairDelimiterToken = context.JsonReader.ReadNextToken();
                if (pairDelimiterToken.JsonType != JsonTokenType.PairDelimiter)
                    throw new JsonDocumentException($"Expected delimiter between label and value in complex JSON object, but found {pairDelimiterToken}", pairDelimiterToken);

                var typeStringToken = context.JsonReader.ReadNextToken();
                if (typeStringToken.JsonType != JsonTokenType.String)
                    throw new JsonDocumentException($"Expected JSON string containing the type name for deserialization, but found {typeStringToken}", typeStringToken);

                typeToConstruct = _nameToTypeMapping.Map(typeStringToken.ToStringWithoutQuotationMarks());

                var valueDelimiterOrEndOfObjectToken = context.JsonReader.ReadNextToken();

                // If the complex object ends here, then just create the target object using the factory
                // There are no more label value pairs in this object to be deserialized
                if (valueDelimiterOrEndOfObjectToken.JsonType == JsonTokenType.EndOfObject)
                    return _objectFactory.Create(typeToConstruct, deserializedChildValues);

                if (valueDelimiterOrEndOfObjectToken.JsonType != JsonTokenType.ValueDelimiter)
                    throw new JsonDocumentException($"Expected value delimiter or end of complex JSON object, but found {valueDelimiterOrEndOfObjectToken}", valueDelimiterOrEndOfObjectToken);
            }

            // Get the type constructor
            TypeConstructionInfo typeConstructionInfo;
            if (_typeConstructionInfoCache.TryGetValue(typeToConstruct, out typeConstructionInfo) == false)
            {
                typeConstructionInfo = _typeAnalyzer.CreateInfo(typeToConstruct);
                _typeConstructionInfoCache.Add(typeToConstruct, typeConstructionInfo);
            }

            // At this point, there must be definitely another label value pair in the complex JSON object
            while (true)
            {
                var labelToken = context.JsonReader.ReadNextToken();
                if (labelToken.JsonType != JsonTokenType.String)
                    throw new JsonDocumentException($"Expected JSON string or end of complex JSON object, but found {labelToken}", labelToken);

                var injectableValueName = labelToken.ToStringWithoutQuotationMarks();
                var injectableValueInfo = typeConstructionInfo.GetInjectableValueInfoFromName(injectableValueName) ??
                                          new InjectableValueInfo(injectableValueName, _objectType, InjectableValueKind.UnknownOnTargetObject);

                var pairDelimiterToken = context.JsonReader.ReadNextToken();
                if (pairDelimiterToken.JsonType != JsonTokenType.PairDelimiter)
                    throw new JsonDocumentException($"Expected delimiter between label and value in complex JSON object, but found {pairDelimiterToken}", pairDelimiterToken);

                var valueToken = context.JsonReader.ReadNextToken();
                var value = context.DeserializeToken(valueToken, injectableValueInfo.Type);

                deserializedChildValues.Add(injectableValueInfo, value);

                var valueDelimiterOrEndOfObjectToken = context.JsonReader.ReadNextToken();

                if (valueDelimiterOrEndOfObjectToken.JsonType == JsonTokenType.EndOfObject)
                    break;
                if (valueDelimiterOrEndOfObjectToken.JsonType != JsonTokenType.ValueDelimiter)
                    throw new JsonDocumentException($"Expected value delimiter or end of complex JSON object, but found {valueDelimiterOrEndOfObjectToken}", valueDelimiterOrEndOfObjectToken);
            }

            return _objectFactory.Create(context.RequestedType, deserializedChildValues);
        }
    }
}