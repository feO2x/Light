using System;
using System.Collections.Generic;

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
                if (value == null) throw new ArgumentNullException("value");
                _valueProviderFactory = value;
            }
        }

        public IList<IValueProvider> AnalyzeType(Type type)
        {
            var valueProviders = new List<IValueProvider>();

            // ReSharper disable LoopCanBeConvertedToQuery
            foreach (var propertyInfo in type.GetProperties())
            {
                if (propertyInfo.CanRead == false)
                    continue;

                var getMethod = propertyInfo.GetGetMethod();
                if (getMethod.IsPublic && getMethod.IsStatic == false)
                    valueProviders.Add(_valueProviderFactory.Create(type, propertyInfo));
            }

            foreach (var fieldInfo in type.GetFields())
            {
                if (fieldInfo.IsPublic && fieldInfo.IsStatic == false)
                    valueProviders.Add(_valueProviderFactory.Create(type, fieldInfo));
            }
            // ReSharper restore LoopCanBeConvertedToQuery

            return valueProviders;
        }
    }
}