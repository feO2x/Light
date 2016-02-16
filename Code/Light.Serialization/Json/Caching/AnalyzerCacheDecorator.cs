using System;
using System.Collections.Generic;
using Light.GuardClauses;
using Light.Serialization.Json.ComplexTypeConstruction;
using Light.Serialization.Json.TokenParsers;

namespace Light.Serialization.Json.Caching
{
    public sealed class AnalyzerCacheDecorator : ITypeCreationInfoAnalyzer
    {
        private readonly ITypeCreationInfoAnalyzer _analyzer;
        private readonly Dictionary<Type, TypeCreationDescription> _cache;

        public AnalyzerCacheDecorator(ITypeCreationInfoAnalyzer analyzer, Dictionary<Type, TypeCreationDescription> cache)
        {
            analyzer.MustNotBeNull(nameof(analyzer));
            cache.MustNotBeNull(nameof(cache));

            _analyzer = analyzer;
            _cache = cache;
        }

        public TypeCreationDescription CreateInfo(Type typeToAnalyze)
        {
            typeToAnalyze.MustNotBeNull(nameof(typeToAnalyze));

            TypeCreationDescription typeCreationDescription;
            if (_cache.TryGetValue(typeToAnalyze, out typeCreationDescription))
                return typeCreationDescription;

            typeCreationDescription = _analyzer.CreateInfo(typeToAnalyze);
            _cache.Add(typeToAnalyze, typeCreationDescription);

            return typeCreationDescription;
        }
    }
}