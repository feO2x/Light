using System;

namespace Light.Serialization.Json.ComplexTypeConstruction
{
    public interface IDictionaryFactory
    {
        object CreateDictionary(Type requestedDictionaryType);
    }
}