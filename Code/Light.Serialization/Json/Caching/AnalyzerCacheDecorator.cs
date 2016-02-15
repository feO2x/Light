using System;
using System.Collections.Generic;
using Light.GuardClauses;
using Light.Serialization.Json.TokenParsers;

namespace Light.Serialization.Json.Caching
{
    public sealed class AnalyzerCacheDecorator : ITypeCreationInfoAnalyzer
    {
        private readonly ITypeCreationInfoAnalyzer _analyzer;
        private readonly Dictionary<Type, TypeCreationInfo> _cache;

        public AnalyzerCacheDecorator(ITypeCreationInfoAnalyzer analyzer, Dictionary<Type, TypeCreationInfo> cache)
        {
            analyzer.MustNotBeNull(nameof(analyzer));
            cache.MustNotBeNull(nameof(cache));

            _analyzer = analyzer;
            _cache = cache;
        }

        public TypeCreationInfo CreateInfo(Type typeToAnalyze)
        {
            typeToAnalyze.MustNotBeNull(nameof(typeToAnalyze));

            TypeCreationInfo typeCreationInfo;
            if (_cache.TryGetValue(typeToAnalyze, out typeCreationInfo))
                return typeCreationInfo;

            typeCreationInfo = _analyzer.CreateInfo(typeToAnalyze);
            _cache.Add(typeToAnalyze, typeCreationInfo);

            return typeCreationInfo;
        }
    }
}