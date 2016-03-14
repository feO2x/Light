using System;

namespace Light.Serialization.Json.TypeNaming
{
    public sealed class SimpleNameToTypeMapping : INameToTypeMapping
    {
        public Type Map(string typeName)
        {
            return Type.GetType(typeName);
        }
    }
}