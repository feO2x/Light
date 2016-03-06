using System;

namespace Light.Serialization.Json.TypeNaming
{
    public interface ITypeSectionParser
    {
        string ActualTypeSymbol { get; }
        Type ParseTypeSection(JsonDeserializationContext context);
    }
}