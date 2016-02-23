using System;
using Light.GuardClauses;

namespace Light.Serialization.Json
{
    public class JsonDocumentException : DeserializationException
    {
        public readonly JsonToken ErroneousToken;

        public JsonDocumentException(string message, JsonToken erroneousToken, Exception innerException = null) 
            : base(message, innerException)
        {
            erroneousToken.MustNotBeNull(nameof(erroneousToken));

            ErroneousToken = erroneousToken;
        }
    }
}