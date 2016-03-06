using System;
using Light.GuardClauses;
using Light.Serialization.Json.TokenParsers;

namespace Light.Serialization.Json.TypeNaming
{
    public sealed class DefaultTypeSectionParser : ITypeSectionParser
    {
        private readonly INameToTypeMapping _nameToTypeMapping;
        private string _actualTypeSymbol = "$type";

        public DefaultTypeSectionParser(INameToTypeMapping nameToTypeMapping)
        {
            nameToTypeMapping.MustNotBeNull(nameof(nameToTypeMapping));

            _nameToTypeMapping = nameToTypeMapping;
        }

        public string ActualTypeSymbol
        {
            get { return _actualTypeSymbol; }
            set
            {
                value.MustNotBeNullOrWhiteSpace(nameof(value));
                _actualTypeSymbol = value;
            }
        }
        public Type ParseTypeSection(JsonDeserializationContext context)
        {
            var token = context.JsonReader.ReadNextToken();

            if (token.JsonType == JsonTokenType.String)
            {
                var typeName = (string) context.DeserializeToken(token, typeof (string));
                return _nameToTypeMapping.Map(typeName);
            }

            throw new NotImplementedException("We have to be able to deserialize generic types");
        }
    }
}