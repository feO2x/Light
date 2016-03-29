namespace Light.Serialization.Json.ObjectReferencePreservation
{
    public interface IIdentifierParser
    {
        string IdentifierSymbol { get; set; }
        int ParseIdentifier(JsonDeserializationContext context);
    }
}