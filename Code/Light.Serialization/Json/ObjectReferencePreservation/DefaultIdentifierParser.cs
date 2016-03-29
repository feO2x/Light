using Light.GuardClauses;

namespace Light.Serialization.Json.ObjectReferencePreservation
{
    public class DefaultIdentifierParser : IIdentifierParser
    {
        private string _identifierSymbol = JsonSymbols.DefaultIdSymbol;

        public string IdentifierSymbol
        {
            get { return _identifierSymbol; }
            set
            {
                value.MustNotBeNullOrWhiteSpace(nameof(value));
                _identifierSymbol = value;
            }
        }

        public int ParseIdentifier(JsonDeserializationContext context)
        {
            var nextToken = context.JsonReader.ReadNextToken();

            if (nextToken.JsonType == JsonTokenType.IntegerNumber)
            {
                return context.DeserializeToken<int>(nextToken);
            }

            throw new JsonDocumentException($"Expected JSON integer number to parse actual type, but found {nextToken}.", nextToken);
        }
    }
}
