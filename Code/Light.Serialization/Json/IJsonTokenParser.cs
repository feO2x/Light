using System;

namespace Light.Serialization.Json
{
    public interface IJsonTokenParser
    {
        bool CanBeCached { get; }
        bool IsSuitableFor(JsonToken token, Type requestedType);
        object ParseValue(JsonDeserializationContext context);
    }
}