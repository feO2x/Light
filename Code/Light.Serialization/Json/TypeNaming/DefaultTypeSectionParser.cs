using System;
using System.Reflection;
using Light.GuardClauses;
using Light.Serialization.Json.LowLevelReading;
using Light.Serialization.Json.TokenParsers;

namespace Light.Serialization.Json.TypeNaming
{
    public sealed class DefaultTypeSectionParser : ITypeSectionParser
    {
        private readonly INameToTypeMapping _nameToTypeMapping;
        private string _concreteTypeSymbol = JsonSymbols.DefaultConcreteTypeSymbol;
        private string _typeArgumentsSymbol = JsonSymbols.DefaultTypeArgumentSymbol;
        private string _typeNameSymbol = JsonSymbols.DefaultTypeNameSymbol;

        public DefaultTypeSectionParser(INameToTypeMapping nameToTypeMapping)
        {
            nameToTypeMapping.MustNotBeNull(nameof(nameToTypeMapping));

            _nameToTypeMapping = nameToTypeMapping;
        }

        public string TypeNameSymbol
        {
            get { return _typeNameSymbol; }
            set
            {
                value.MustNotBeNullOrWhiteSpace(nameof(value));
                _typeNameSymbol = value;
            }
        }

        public string TypeArgumentsSymbol
        {
            get { return _typeArgumentsSymbol; }
            set
            {
                value.MustNotBeNullOrWhiteSpace(nameof(value));
                _typeArgumentsSymbol = value;
            }
        }

        public string ConcreteTypeSymbol
        {
            get { return _concreteTypeSymbol; }
            set
            {
                value.MustNotBeNullOrWhiteSpace(nameof(value));
                _concreteTypeSymbol = value;
            }
        }

        public Type ParseTypeSection(JsonDeserializationContext context)
        {
            var nextToken = context.JsonReader.ReadNextToken();
            string typeName;

            if (nextToken.JsonType == JsonTokenType.String)
            {
                typeName = context.DeserializeToken<string>(nextToken);
                return _nameToTypeMapping.Map(typeName);
            }

            if (nextToken.JsonType != JsonTokenType.BeginOfObject)
                throw new JsonDocumentException($"Expected JSON string or begin of object to parse actual type, but found {nextToken}.", nextToken);

            nextToken = context.JsonReader.ReadNextToken();
            if (nextToken.JsonType != JsonTokenType.String)
                throw new JsonDocumentException($"Expected name of generic type in JSON document, but found {nextToken}.", nextToken);

            typeName = context.DeserializeToken<string>(nextToken);
            var genericType = _nameToTypeMapping.Map(typeName);
            var genericTypeInfo = genericType.GetTypeInfo();
            if (genericTypeInfo.IsGenericTypeDefinition == false)
                throw new InvalidOperationException($"The specified type {genericType} should be a generic type definition, but is not.");

            context.JsonReader.ReadAndExpectValueDelimiterToken();
            var genericTypeParameters = genericTypeInfo.GenericTypeParameters;
            var typeArguments = new Type[genericTypeParameters.Length];

            context.JsonReader.ReadAndExpectBeginOfArray();
            for (var i = 0; i < genericTypeParameters.Length; i++)
            {
                typeArguments[i] = ParseTypeSection(context);
                if (i < genericTypeParameters.Length - 1)
                    context.JsonReader.ReadAndExpectValueDelimiterToken();
                else
                    context.JsonReader.ReadAndExpectedEndOfArray();
            }

            return genericTypeInfo.MakeGenericType(typeArguments);
        }
    }
}