using System;
using Light.Serialization.Json.TokenParsers;

namespace Light.Serialization.Json.ComplexTypeConstruction
{
    public sealed class SimpleNameToTypeMapping : INameToTypeMapping
    {
        public Type Map(string typeName)
        {
            return Type.GetType(typeName);
        }
    }
}