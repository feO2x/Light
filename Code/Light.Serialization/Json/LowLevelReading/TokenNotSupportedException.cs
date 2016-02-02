namespace Light.Serialization.Json.LowLevelReading
{
    public class TokenNotSupportedException : DeserializationException
    {
        public readonly JsonToken Token;

        public TokenNotSupportedException(string message, JsonToken token) : base(message)
        {
            Token = token;
        }
    }
}