using System;

namespace Light.Serialization.Json
{
    public interface IJsonTokenParser
    {
        bool IsSuitableFor(JsonToken token, Type requestedType);

        object ParseValue(JsonDeserializationContext context);
    }
}