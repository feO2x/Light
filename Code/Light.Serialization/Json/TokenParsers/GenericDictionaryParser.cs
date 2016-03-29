using System;
using System.Collections.Generic;
using System.Reflection;
using Light.GuardClauses;
using Light.Serialization.FrameworkExtensions;
using Light.Serialization.Json.ComplexTypeConstruction;
using Light.Serialization.Json.LowLevelReading;
using Light.Serialization.Json.ObjectReferencePreservation;
using Light.Serialization.Json.TypeNaming;

namespace Light.Serialization.Json.TokenParsers
{
    public sealed class GenericDictionaryParser : IJsonTokenParser
    {
        private readonly IDictionaryFactory _dictionaryFactory;
        private readonly ITypeSectionParser _typeSectionParser;
        private readonly IReferenceParser _referenceParser;
        private readonly IIdentifierParser _identifierParser;
        private readonly MethodInfo _populateGenericDictionaryInfo = typeof (GenericDictionaryParser).GetTypeInfo().GetDeclaredMethod(nameof(PopulateGenericDictionary));
        private readonly object[] _methodParameters = new object[3];

        public bool CanBeCached => true;

        public GenericDictionaryParser(IDictionaryFactory dictionaryFactory, ITypeSectionParser typeSectionParser, IIdentifierParser identifierParser, IReferenceParser referenceParser)
        {
            dictionaryFactory.MustNotBeNull(nameof(dictionaryFactory));
            typeSectionParser.MustNotBeNull(nameof(typeSectionParser));
            identifierParser.MustNotBeNull(nameof(identifierParser));
            referenceParser.MustNotBeNull(nameof(referenceParser));

            _dictionaryFactory = dictionaryFactory;
            _typeSectionParser = typeSectionParser;
            _identifierParser = identifierParser;
            _referenceParser = referenceParser;
        }

        public bool IsSuitableFor(JsonToken token, Type requestedType)
        {
            return token.JsonType == JsonTokenType.BeginOfObject &&
                   requestedType.GetTypeInfo().ImplementsGenericInterface(typeof (IDictionary<,>).GetTypeInfo());
        }

        public object ParseValue(JsonDeserializationContext context)
        {
            var jsonReader = context.JsonReader;
            var currentToken = jsonReader.ReadNextToken();

            // Check if it is an empty JSON object
            if (currentToken.JsonType == JsonTokenType.EndOfObject)
                return _dictionaryFactory.CreateDictionary(context.RequestedType);

            // If not, then there must be a JSON string as the first key of the object
            if (currentToken.JsonType != JsonTokenType.String)
                throw new JsonDocumentException($"Expected JSON string or end of complex JSON object, but found {currentToken}", currentToken);

            var firstTokenString = context.DeserializeToken<string>(currentToken);
            Type targetType;
            int identifier;
            int reference;
            object dictionary;

            // Check if the first string is the concrete type symbol
            if (firstTokenString == _typeSectionParser.ConcreteTypeSymbol)
            {
                jsonReader.ReadAndExpectPairDelimiterToken();

                targetType = _typeSectionParser.ParseTypeSection(context);
                dictionary = _dictionaryFactory.CreateDictionary(targetType);

                if (jsonReader.ReadAndExpectEndOfObjectOrValueDelimiter() == JsonTokenType.EndOfObject)
                    return dictionary;

                currentToken = jsonReader.ReadNextToken();
            }
            else
            {
                if (firstTokenString == _identifierParser.IdentifierSymbol)
                {
                    jsonReader.ReadAndExpectPairDelimiterToken();

                    identifier = _identifierParser.ParseIdentifier(context);
                    currentToken = jsonReader.ReadNextToken();
                }

                if (firstTokenString == _referenceParser.ReferenceSymbol)
                {
                    jsonReader.ReadAndExpectPairDelimiterToken();

                    reference = _referenceParser.ParseReference(context);
                    currentToken = jsonReader.ReadNextToken();
                }

                targetType = context.RequestedType;
                dictionary = _dictionaryFactory.CreateDictionary(targetType);
            }

            var specificDictionaryType = targetType.GetTypeInfo().GetSpecificTypeInfoThatCorrespondsToGenericInterface(typeof (IDictionary<,>).GetTypeInfo());
            var specificPopulateGenericDictionaryMethod = _populateGenericDictionaryInfo.MakeGenericMethod(specificDictionaryType.GenericTypeArguments);

            _methodParameters[0] = currentToken;
            _methodParameters[1] = dictionary;
            _methodParameters[2] = context;

            specificPopulateGenericDictionaryMethod.Invoke(null, _methodParameters);

            ClearObjectArray();

            return dictionary;
        }

        private void ClearObjectArray()
        {
            _methodParameters[0] = _methodParameters[1] = _methodParameters[2] = null;
        }

        private static void PopulateGenericDictionary<TKey, TValue>(JsonToken currentToken, IDictionary<TKey, TValue> dictionary, JsonDeserializationContext context)
        {
            while (true)
            {
                if (currentToken.JsonType != JsonTokenType.String)
                    throw new JsonDocumentException($"Expected key in complex JSON object, but found {currentToken}", currentToken);

                var key = context.DeserializeToken<TKey>(currentToken);

                context.JsonReader.ReadAndExpectPairDelimiterToken();

                currentToken = context.JsonReader.ReadNextToken();
                currentToken.ExpectBeginOfValue();

                var value = context.DeserializeToken<TValue>(currentToken);

                dictionary.Add(key, value);

                if (context.JsonReader.ReadAndExpectEndOfObjectOrValueDelimiter() == JsonTokenType.EndOfObject)
                    return;

                currentToken = context.JsonReader.ReadNextToken();
            }
        }
    }
}