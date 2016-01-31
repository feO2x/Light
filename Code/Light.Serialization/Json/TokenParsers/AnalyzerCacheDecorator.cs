using System;
using System.Collections.Generic;

namespace Light.Serialization.Json.TokenParsers
{
    public sealed class AnalyzerCacheDecorator : ITypeCreationInfoAnalyzer
    {
        private readonly ITypeCreationInfoAnalyzer _analyzer;
        private readonly Dictionary<Type, TypeConstructionInfo> _cache;

        public AnalyzerCacheDecorator(ITypeCreationInfoAnalyzer analyzer, Dictionary<Type, TypeConstructionInfo> cache)
        {
            if (analyzer == null) throw new ArgumentNullException(nameof(analyzer));
            if (cache == null) throw new ArgumentNullException(nameof(cache));

            _analyzer = analyzer;
            _cache = cache;
        }

        public TypeConstructionInfo CreateInfo(Type typeToAnalyze)
        {
            TypeConstructionInfo typeConstructionInfo;
            if (_cache.TryGetValue(typeToAnalyze, out typeConstructionInfo))
                return typeConstructionInfo;

            typeConstructionInfo = _analyzer.CreateInfo(typeToAnalyze);
            _cache.Add(typeToAnalyze, typeConstructionInfo);

            return typeConstructionInfo;
        }
    }
}