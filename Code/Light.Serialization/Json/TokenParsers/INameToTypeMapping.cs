using System;

namespace Light.Serialization.Json.TokenParsers
{
    public interface INameToTypeMapping
    {
        Type Map(string typeName);
    }
}