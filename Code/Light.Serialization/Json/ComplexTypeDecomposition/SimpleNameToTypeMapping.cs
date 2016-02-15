using Light.Serialization.Json.TokenParsers;
using System;

namespace Light.Serialization.Json.ComplexTypeDecomposition
{
    public sealed class SimpleNameToTypeMapping : INameToTypeMapping
    {
        public Type Map(string typeName)
        {
            return Type.GetType(typeName, true);
        }
    }
}
