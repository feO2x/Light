using System;

namespace Light.Serialization.Json
{
    public interface IJsonValueDeserializer
    {
        bool IsSuitableFor(JsonCharacterBuffer buffer, Type requestedType);

        object DeserializeValue(JsonCharacterBuffer buffer, Type requestedType);
    }
}