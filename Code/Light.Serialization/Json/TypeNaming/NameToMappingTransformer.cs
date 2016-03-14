using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Light.GuardClauses;

namespace Light.Serialization.Json.TypeNaming
{
    public sealed class NameToMappingTransformer : NameToMappingTransformer.IScanningOptions, NameToMappingTransformer.INamespaceOptions, NameToMappingTransformer.IExeptTypeOptions
    {
        private readonly List<Type> _usedTypes = new List<Type>();

        IScanningOptions IExeptTypeOptions.ExceptTypes(params Type[] types)
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

        IExeptTypeOptions INamespaceOptions.ExceptNamespaces(params string[] namespaces)
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

        IExeptTypeOptions INamespaceOptions.UseOnlyNamespaces(params string[] namespaces)
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

        INamespaceOptions IScanningOptions.AllTypesFromAssemblies(params Type[] assemblyMarkers)
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

        INamespaceOptions IScanningOptions.AllTypesFromAssemblies(params Assembly[] assemblies)
        {
            foreach (var type in assemblies.SelectMany(a => a.ExportedTypes))
            {
                _usedTypes.Add(type);
            }

            return this;
        }

        NameToMappingTransformer IScanningOptions.UseTypes(params Type[] types)
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
                    jsonName = type.Name.Split('`')[0];

                mapping.AddMapping(jsonName, type);
            }
        }

        public interface IScanningOptions
        {
            INamespaceOptions AllTypesFromAssemblies(params Type[] assemblyMarkers);
            INamespaceOptions AllTypesFromAssemblies(params Assembly[] assemblies);
            NameToMappingTransformer UseTypes(params Type[] types);
        }

        public interface INamespaceOptions
        {
            IExeptTypeOptions ExceptNamespaces(params string[] namespaces);
            IExeptTypeOptions UseOnlyNamespaces(params string[] namespaces);
        }

        public interface IExeptTypeOptions
        {
            IScanningOptions ExceptTypes(params Type[] types);
        }
    }
}