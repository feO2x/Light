using System;
using System.Collections.Generic;
using Light.GuardClauses;
using Light.Serialization.Json.ComplexTypeConstruction;

namespace Light.Serialization.Json.Caching
{
    public sealed class TypeDescriptionCacheDecorator : ITypeDescriptionProvider
    {
        private readonly ITypeDescriptionProvider _decoratedProvider;
        private readonly Dictionary<Type, TypeCreationDescription> _cache;

        public TypeDescriptionCacheDecorator(ITypeDescriptionProvider decoratedProvider, Dictionary<Type, TypeCreationDescription> cache)
        {
            decoratedProvider.MustNotBeNull(nameof(decoratedProvider));
            cache.MustNotBeNull(nameof(cache));

            _decoratedProvider = decoratedProvider;
            _cache = cache;
        }

        public TypeCreationDescription GetTypeCreationDescription(Type typeToAnalyze)
        {
            typeToAnalyze.MustNotBeNull(nameof(typeToAnalyze));

            TypeCreationDescription typeCreationDescription;
            if (_cache.TryGetValue(typeToAnalyze, out typeCreationDescription))
                return typeCreationDescription;

            typeCreationDescription = _decoratedProvider.GetTypeCreationDescription(typeToAnalyze);
            _cache.Add(typeToAnalyze, typeCreationDescription);

            return typeCreationDescription;
        }
    }
}