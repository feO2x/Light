using Light.GuardClauses;

namespace Light.Serialization.Json.ObjectReferencePreservation
{
    public interface IReferenceParser
    {
        string ReferenceSymbol { get; set; }
        int ParseReference(JsonDeserializationContext context);
    }

    public class DefaultReferenceParser : IReferenceParser
    {
        private string _referenceSymbol = JsonSymbols.DefaultReferenceSymbol;

        public string ReferenceSymbol
        {
            get
            {
                return _referenceSymbol;
            }
            set
            {
                value.MustNotBeNullOrWhiteSpace(nameof(value));
                _referenceSymbol = value;
            }
        }

        public int ParseReference(JsonDeserializationContext context)
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