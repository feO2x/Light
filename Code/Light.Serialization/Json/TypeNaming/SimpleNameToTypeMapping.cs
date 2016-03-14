using System;
using Light.GuardClauses;

namespace Light.Serialization.Json.TypeNaming
{
    public sealed class SimpleNameToTypeMapping : INameToTypeMapping, ITypeToNameMapping
    {
        public Type Map(string typeName)
        {
            return Type.GetType(typeName);
        }

        public string Map(Type type)
        {
            type.MustNotBeNull(nameof(type));

            return type.FullName;
        }
    }
}