using System;
using System.Collections.Generic;
using System.Linq;
using Light.GuardClauses;
using Light.GuardClauses.Exceptions;
using Light.Serialization.Json.TokenParsers;

namespace Light.Serialization.Json.TypeNaming
{
    public sealed class DomainFriendlyNameMapping : INameToTypeMapping
    {
        private readonly Dictionary<string, List<Type>> _nameToTypeMappings;
        private readonly Dictionary<Type, string> _typeToNameMappings;

        public DomainFriendlyNameMapping(Dictionary<string, List<Type>> nameToTypeMappings, Dictionary<Type, string> typeToNameMappings)
        {
            nameToTypeMappings.MustNotBeNull(nameof(nameToTypeMappings));
            typeToNameMappings.MustNotBeNull(nameof(typeToNameMappings));

            _nameToTypeMappings = nameToTypeMappings;
            _typeToNameMappings = typeToNameMappings;
        }

        public Type Map(string typeName)
        {
            return _nameToTypeMappings[typeName][0];
        }

        public DomainFriendlyNameMapping AddMapping(string jsonName, Type defaultMappedType, params Type[] otherMappedTypes)
        {
            var mappedTypes = new List<Type> { defaultMappedType };
            mappedTypes.AddRange(otherMappedTypes);
            return AddMapping(jsonName, mappedTypes);
        }

        public DomainFriendlyNameMapping AddMapping(string jsonName, List<Type> mappedTypes)
        {
            jsonName.MustNotBeNullOrWhiteSpace(nameof(jsonName));
            mappedTypes.MustNotBeNull(nameof(mappedTypes));
            Guard.Against(mappedTypes.Count < 1,
                          () => new CollectionException($"{nameof(mappedTypes)} must have at least one type that is mapped to the jsonName", nameof(mappedTypes)));
            Guard.Against(mappedTypes.Any(t => t == null),
                          () => new CollectionException($"{nameof(mappedTypes)} must have no entries that are null.", nameof(mappedTypes)));

            _nameToTypeMappings.Add(jsonName, mappedTypes);
            foreach (var type in mappedTypes)
            {
                _typeToNameMappings.Add(type, jsonName);
            }
            return this;
        }

        public DomainFriendlyNameMapping RemoveMapping(string jsonName)
        {
            jsonName.MustBeKeyOf(_nameToTypeMappings, nameof(jsonName));

            var types = _nameToTypeMappings[jsonName];
            _nameToTypeMappings.Remove(jsonName);
            foreach (var type in types)
            {
                _typeToNameMappings.Remove(type);
            }
            return this;
        }

        public DomainFriendlyNameMapping RemoveMapping(Type type)
        {
            type.MustBeKeyOf(_typeToNameMappings, nameof(type));

            var jsonName = _typeToNameMappings[type];
            _typeToNameMappings.Remove(type);

            var allMappedTypes = _nameToTypeMappings[jsonName];
            allMappedTypes.Remove(type);
            if (allMappedTypes.Count == 0)
                _nameToTypeMappings.Remove(jsonName);
            return this;
        }

        public DomainFriendlyNameMapping SetDefaultType(string jsonName, Type newDefaultType)
        {
            jsonName.MustBeKeyOf(_nameToTypeMappings, nameof(jsonName));

            var types = _nameToTypeMappings[jsonName];
            var indexOfNewDefaultType = types.IndexOf(newDefaultType);
            if (indexOfNewDefaultType == -1)
                _typeToNameMappings.Add(newDefaultType, jsonName);
            else
                types.RemoveAt(indexOfNewDefaultType);

            types.Insert(0, newDefaultType);

            return this;
        }

        public DomainFriendlyNameMapping ClearAllMappings()
        {
            _nameToTypeMappings.Clear();
            _typeToNameMappings.Clear();
            return this;
        }

        public string Map(Type type)
        {
            return _typeToNameMappings[type];
        }
    }
}