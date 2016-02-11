using System;
using System.Collections.Generic;
using Light.GuardClauses;
using Light.Serialization.Json.TokenParsers;

namespace Light.Serialization.Json.Caching
{
    public sealed class AnalyzerCacheDecorator : ITypeCreationInfoAnalyzer
    {
        private readonly ITypeCreationInfoAnalyzer _analyzer;
        private readonly Dictionary<Type, TypeConstructionInfo> _cache;

        public AnalyzerCacheDecorator(ITypeCreationInfoAnalyzer analyzer, Dictionary<Type, TypeConstructionInfo> cache)
        {
            analyzer.MustNotBeNull(nameof(analyzer));
            cache.MustNotBeNull(nameof(cache));

            _analyzer = analyzer;
            _cache = cache;
        }

        public TypeConstructionInfo CreateInfo(Type typeToAnalyze)
        {
            typeToAnalyze.MustNotBeNull(nameof(typeToAnalyze));

            TypeConstructionInfo typeConstructionInfo;
            if (_cache.TryGetValue(typeToAnalyze, out typeConstructionInfo))
                return typeConstructionInfo;

            typeConstructionInfo = _analyzer.CreateInfo(typeToAnalyze);
            _cache.Add(typeToAnalyze, typeConstructionInfo);

            return typeConstructionInfo;
        }
    }
}