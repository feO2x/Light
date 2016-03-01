using System;
using System.Collections.Generic;
using Light.GuardClauses;
using Light.Serialization.Json.TokenParsers;

namespace Light.Serialization.Json.TypeNaming
{
    public sealed class DomainFriendlyNameMapping : INameToTypeMapping
    {
        private readonly Dictionary<string, Type> _mapping;

        public DomainFriendlyNameMapping(Dictionary<string, Type> mapping)
        {
            mapping.MustNotBeNull(nameof(mapping));

            _mapping = mapping;
        }

        public Type Map(string typeName)
        {
            return _mapping[typeName];
        }
    }
}