using System;
using Light.GuardClauses;

namespace Light.Serialization.Json
{
    public struct JsonDeserializationContext
    {
        public readonly JsonToken Token;
        public readonly Type RequestedType;
        public readonly IJsonReader JsonReader;
        private readonly Func<JsonToken, Type, object> _deserializeToken;

        public JsonDeserializationContext(JsonToken token,
                                          Type requestedType,
                                          IJsonReader jsonReader,
                                          Func<JsonToken, Type, object> deserializeToken)
        {
            requestedType.MustNotBeNull(nameof(requestedType));
            jsonReader.MustNotBeNull(nameof(jsonReader));
            deserializeToken.MustNotBeNull(nameof(deserializeToken));

            Token = token;
            RequestedType = requestedType;
            JsonReader = jsonReader;
            _deserializeToken = deserializeToken;
        }

        public T DeserializeToken<T>(JsonToken token)
        {
            return (T)_deserializeToken(token, typeof (T));
        }

        public object DeserializeToken(JsonToken token, Type requestedType)
        {
            return _deserializeToken(token, requestedType);
        }
    }
}