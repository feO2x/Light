namespace Light.Serialization.Json
{
    public class JsonDocumentException : DeserializationException
    {
        public JsonDocumentException(string message, JsonToken erroneousToken) : base(message)
        {
        }
    }
}