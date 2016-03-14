using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Light.GuardClauses;

namespace Light.Serialization.Json.TypeNaming
{
    public sealed class NameToMappingTransformer
    {
        private readonly List<Type> _usedTypes = new List<Type>();

        public NameToMappingTransformer AllTypesFromAssemblies(params Type[] assemblyMarkers)
        {
            var allTypes = assemblyMarkers.Select(m => m.GetTypeInfo().Assembly)
                                          .SelectMany(a => a.ExportedTypes)
                                          .Where(t => assemblyMarkers.Contains(t) == false);

            foreach (var type in allTypes)
            {
                _usedTypes.Add(type);
            }
            return this;
        }

        public NameToMappingTransformer ExceptNamespaces(params string[] namespaces)
        {
            var currentIndex = 0;
            while (currentIndex < _usedTypes.Count)
            {
                var type = _usedTypes[currentIndex];
                if (namespaces.Contains(type.Namespace))
                    _usedTypes.RemoveAt(currentIndex);
                else
                    currentIndex++;
            }
            
            return this;
        }

        public NameToMappingTransformer UseOnlyNamespaces(params string[] namespaces)
        {
            var currentIndex = 0;
            while (currentIndex < _usedTypes.Count)
            {
                var type = _usedTypes[currentIndex];
                if (namespaces.Contains(type.Namespace) == false)
                    _usedTypes.RemoveAt(currentIndex);
                else
                    currentIndex++;
            }

            return this;
        }

        public NameToMappingTransformer ExceptTypes(params Type[] types)
        {
            var currentIndex = 0;
            while (currentIndex < _usedTypes.Count)
            {
                var type = _usedTypes[currentIndex];
                if (types.Contains(type))
                    _usedTypes.RemoveAt(currentIndex);
                else
                    currentIndex++;
            }
            return this;
        }

        public NameToMappingTransformer UseTypes(params Type[] types)
        {
            _usedTypes.AddRange(types);
            return this;
        }

        public void CreateMappings(IAddOneToOneMapping mapping)
        {
            mapping.MustNotBeNull(nameof(mapping));

            foreach (var type in _usedTypes)
            {
                var jsonName = type.Name;
                if (type.GetTypeInfo().IsGenericType)
                    jsonName = type.Name.Split('\'')[0];

                mapping.AddMapping(jsonName, type);
            }
        }
    }
}