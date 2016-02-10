using Light.GuardClauses;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Light.Serialization.Json.ComplexTypeDecomposition
{
    public sealed class PublicPropertiesAndFieldsAnalyzer : IReadableValuesTypeAnalyzer
    {
        private IValueProviderFactory _valueProviderFactory = new ValueProviderFactoryUsingLambdas();

        public IValueProviderFactory ValueProviderFactory
        {
            get { return _valueProviderFactory; }
            set
            {
                value.MustNotBeNull(nameof(value));
                _valueProviderFactory = value;
            }
        }

        public IList<IValueProvider> AnalyzeType(Type type)
        {
            type.MustNotBeNull(nameof(type));

            var valueProviders = new List<IValueProvider>();

            // ReSharper disable LoopCanBeConvertedToQuery
            foreach (var propertyInfo in type.GetRuntimeProperties())
            {
                if (propertyInfo.CanRead == false)
                    continue;

                var getMethod = propertyInfo.GetMethod;
                if (getMethod.IsPublic && getMethod.IsStatic == false)
                    valueProviders.Add(_valueProviderFactory.Create(type, propertyInfo));
            }

            foreach (var fieldInfo in type.GetRuntimeFields())
            {
                if (fieldInfo.IsPublic && fieldInfo.IsStatic == false)
                    valueProviders.Add(_valueProviderFactory.Create(type, fieldInfo));
            }
            // ReSharper restore LoopCanBeConvertedToQuery

            return valueProviders;
        }
    }
}