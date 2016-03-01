using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Light.Serialization.Json.TypeNaming
{
    public interface IAssemblyNameMappingOptions
    {
        DomainFriendlyNameMappingBuilder.IInitialMappingOptions FromAssemblies(params Type[] markerTypes);
    }

    public sealed class DomainFriendlyNameMappingBuilder : DomainFriendlyNameMappingBuilder.IInitialMappingOptions, DomainFriendlyNameMappingBuilder.IWhiteListNamespaceOptions, IAssemblyNameMappingOptions
    {
        private readonly HashSet<Assembly> _assemblies = new HashSet<Assembly>();
        private readonly HashSet<string> _usedNamespaces = new HashSet<string>();

        IInitialMappingOptions IAssemblyNameMappingOptions.FromAssemblies(params Type[] markerTypes)
        {
            foreach (var markerType in markerTypes)
            {
                var assembly = markerType.GetTypeInfo().Assembly;
                _assemblies.Add(assembly);

                foreach (var @namespace in assembly.ExportedTypes
                                                   .Select(t => t.Namespace)
                                                   .Distinct())
                {
                    _usedNamespaces.Add(@namespace);
                }
            }

            return this;
        }

        void IInitialMappingOptions.ExceptNamespaces(params string[] namespaces)
        {
            foreach (var @namespace in namespaces)
            {
                _usedNamespaces.Remove(@namespace);
            }
        }

        IWhiteListNamespaceOptions IInitialMappingOptions.IgnoreAllNamespaces()
        {
            _usedNamespaces.Clear();
            return this;
        }

        void IWhiteListNamespaceOptions.But(params string[] namespaces)
        {
            foreach (var @namespace in namespaces)
            {
                _usedNamespaces.Add(@namespace);
            }
        }

        public DomainFriendlyNameMapping Build()
        {
            var domainFriendlyNameMappings = _assemblies.SelectMany(a => a.ExportedTypes)
                                                        .Where(t => _usedNamespaces.Contains(t.Namespace))
                                                        .ToDictionary(t => t.Name);

            return new DomainFriendlyNameMapping(domainFriendlyNameMappings);
        }

        public interface IInitialMappingOptions
        {
            void ExceptNamespaces(params string[] namespaces);
            IWhiteListNamespaceOptions IgnoreAllNamespaces();
        }

        public interface IWhiteListNamespaceOptions
        {
            void But(params string[] namespaces);
        }
    }
}