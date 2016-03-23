using System;
using System.Collections.Generic;
using Light.GuardClauses;
using Light.Serialization.Json.ComplexTypeConstruction;

namespace Light.Serialization.Json.Caching
{
    public sealed class TypeDescriptionCacheDecorator : ITypeDescriptionProvider
    {
        public readonly ITypeDescriptionProvider DecoratedProvider;
        private Dictionary<Type, TypeCreationDescription> _cache;

        public TypeDescriptionCacheDecorator(ITypeDescriptionProvider decoratedProvider, Dictionary<Type, TypeCreationDescription> cache)
        {
            decoratedProvider.MustNotBeNull(nameof(decoratedProvider));
            cache.MustNotBeNull(nameof(cache));

            DecoratedProvider = decoratedProvider;
            _cache = cache;
        }

        public Dictionary<Type, TypeCreationDescription> Cache
        {
            get { return _cache; }
            set
            {
                value.MustNotBeNull(nameof(value));
                _cache = value;
            }
        }

        public TypeCreationDescription GetTypeCreationDescription(Type typeToAnalyze)
        {
            typeToAnalyze.MustNotBeNull(nameof(typeToAnalyze));

            TypeCreationDescription typeCreationDescription;
            if (_cache.TryGetValue(typeToAnalyze, out typeCreationDescription))
                return typeCreationDescription;

            typeCreationDescription = DecoratedProvider.GetTypeCreationDescription(typeToAnalyze);
            _cache.Add(typeToAnalyze, typeCreationDescription);

            return typeCreationDescription;
        }
    }
}