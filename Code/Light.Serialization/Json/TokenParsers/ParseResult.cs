namespace Light.Serialization.Json.TokenParsers
{
    public struct ParseResult
    {
        public readonly bool WasTokenParsedSuccessfully;
        public readonly object ParsedObject;

        public ParseResult(bool wasTokenParsedSuccessfully, object parsedObject = null)
        {
            WasTokenParsedSuccessfully = wasTokenParsedSuccessfully;
            ParsedObject = parsedObject;
        }
    }
}