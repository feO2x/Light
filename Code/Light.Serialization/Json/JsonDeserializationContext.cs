using System;

namespace Light.Serialization.Json
{
    public struct JsonDeserializationContext
    {
        public readonly JsonCharacterBuffer Buffer;
        public readonly Type RequestedType;
        public readonly IJsonReader JsonReader;
        public readonly Func<IJsonReader, Type, object> DeserializeChildValue;

        public JsonDeserializationContext(JsonCharacterBuffer buffer,
                                          Type requestedType,
                                          IJsonReader jsonReader,
                                          Func<IJsonReader, Type, object> deserializeChildValue)
        {
            if (requestedType == null) throw new ArgumentNullException(nameof(requestedType));
            if (jsonReader == null) throw new ArgumentNullException(nameof(jsonReader));
            if (deserializeChildValue == null) throw new ArgumentNullException(nameof(deserializeChildValue));

            Buffer = buffer;
            RequestedType = requestedType;
            JsonReader = jsonReader;
            DeserializeChildValue = deserializeChildValue;
        }
    }
}