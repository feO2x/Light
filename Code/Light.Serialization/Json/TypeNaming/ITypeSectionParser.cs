using System;

namespace Light.Serialization.Json.TypeNaming
{
    public interface ITypeSectionParser
    {
        string ConcreteTypeSymbol { get; }
        Type ParseTypeSection(JsonDeserializationContext context);
    }
}