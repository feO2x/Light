using System;

namespace Light.Serialization.Json
{
    public interface IJsonValueParser
    {
        bool IsSuitableFor(JsonCharacterBuffer buffer, Type requestedType);

        object ParseValue(JsonCharacterBuffer buffer, Type requestedType);
    }
}