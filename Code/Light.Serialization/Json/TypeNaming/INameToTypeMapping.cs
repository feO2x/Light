using System;

namespace Light.Serialization.Json.TypeNaming
{
    public interface INameToTypeMapping
    {
        Type Map(string typeName);
    }
}