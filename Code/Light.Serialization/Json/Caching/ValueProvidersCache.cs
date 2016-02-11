using Light.GuardClauses;
using Light.Serialization.Json.ComplexTypeDecomposition;
using System;
using System.Collections.Generic;

namespace Light.Serialization.Json.Caching
{
    public sealed class ValueProvidersCacheDecorator : IReadableValuesTypeAnalyzer
    {
        private readonly IReadableValuesTypeAnalyzer _decoratedAnalyzer;
        private readonly IDictionary<Type, IList<IValueProvider>> _cache;

        public ValueProvidersCacheDecorator(IReadableValuesTypeAnalyzer decoratedAnalyzer, IDictionary<Type, IList<IValueProvider>> cache)
        {
            decoratedAnalyzer.MustNotBeNull(nameof(decoratedAnalyzer));
            cache.MustNotBeNull(nameof(cache));

            _decoratedAnalyzer = decoratedAnalyzer;
            _cache = cache;
        }

        public IList<IValueProvider> AnalyzeType(Type type)
        {
            type.MustNotBeNull(nameof(type));

            IList<IValueProvider> targetValueProviders;
            if (_cache.TryGetValue(type, out targetValueProviders))
                return targetValueProviders;

            targetValueProviders = _decoratedAnalyzer.AnalyzeType(type);
            _cache.Add(type, targetValueProviders);

            return targetValueProviders;
        }
    }
}
