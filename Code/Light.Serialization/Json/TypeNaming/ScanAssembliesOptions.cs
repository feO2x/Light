using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Light.Serialization.Json.TypeNaming
{
    public sealed class DomainFriendlyNamesScanner
    {
        private readonly List<Type> _usedTypes = new List<Type>();

        public DomainFriendlyNamesScanner AllTypesFromAssemblies(params Type[] assemblyMarkers)
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

        public DomainFriendlyNamesScanner ExceptNamespaces(params string[] namespaces)
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

        public DomainFriendlyNamesScanner UseOnlyNamespaces(params string[] namespaces)
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

        public DomainFriendlyNamesScanner ExceptTypes(params Type[] types)
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

        public DomainFriendlyNamesScanner UseTypes(params Type[] types)
        {
            _usedTypes.AddRange(types);
            return this;
        }

          
    }
}