using System;

namespace Light.Serialization.Json.TypeNaming
{
    public interface ITypeToNameMapping
    {
        string Map(Type type);
    }
}