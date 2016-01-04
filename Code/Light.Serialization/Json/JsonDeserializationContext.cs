using System;

namespace Light.Serialization.Json
{
    public struct JsonDeserializationContext
    {
        public readonly JsonToken Token;
        public readonly Type RequestedType;
        public readonly IJsonReader JsonReader;
        public readonly Func<JsonToken, Type, object> DeserializeToken;

        public JsonDeserializationContext(JsonToken token,
                                          Type requestedType,
                                          IJsonReader jsonReader,
                                          Func<JsonToken, Type, object> deserializeToken)
        {
            if (requestedType == null) throw new ArgumentNullException(nameof(requestedType));
            if (jsonReader == null) throw new ArgumentNullException(nameof(jsonReader));
            if (deserializeToken == null) throw new ArgumentNullException(nameof(deserializeToken));

            Token = token;
            RequestedType = requestedType;
            JsonReader = jsonReader;
            DeserializeToken = deserializeToken;
        }
    }
}