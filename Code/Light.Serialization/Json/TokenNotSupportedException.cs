namespace Light.Serialization.Json
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