using Light.Serialization.Json.TokenParsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Light.Serialization.Json.ComplexTypeDecomposition
{
    public sealed class DefaultTypeCreationInfoAnalyzer : ITypeCreationInfoAnalyzer
    {
        private readonly IConstructorSelector _constructorSelector;
        private readonly IInjectableValueNameNormalizer _injectableValueNameNormalizer;

        public DefaultTypeCreationInfoAnalyzer(IConstructorSelector constructorSelector, IInjectableValueNameNormalizer injectableValueNameNormalizer)
        {
            if (constructorSelector == null) throw new ArgumentNullException(nameof(constructorSelector));
            if (injectableValueNameNormalizer == null) throw new ArgumentNullException(nameof(injectableValueNameNormalizer));

            _constructorSelector = constructorSelector;
            _injectableValueNameNormalizer = injectableValueNameNormalizer;
        }

        public TypeConstructionInfo CreateInfo(Type typeToAnalyze)
        {
            if (typeToAnalyze == null) throw new ArgumentNullException(nameof(typeToAnalyze));

            var typeInfo = typeToAnalyze.GetTypeInfo();

            if (typeInfo.IsAbstract || typeInfo.IsInterface)
                throw new ArgumentException($"The specified type {typeToAnalyze.FullName} is abstract and cannot be deserialized", nameof(typeToAnalyze));

            var constructors = typeInfo.DeclaredConstructors
                                       .Where(c => c.IsStatic == false && c.IsPublic)
                                       .ToArray();
            if (constructors.Length == 0)
                throw new ArgumentException($"The specified type {typeToAnalyze.FullName} does not have public constructors", nameof(typeToAnalyze));
            var targetContructor = constructors.Length == 1 ? constructors[0] : _constructorSelector.SelectConstructor(constructors, typeInfo);

            var injectableValueInfos = new List<InjectableValueInfo>();

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var parameterInfo in targetContructor.GetParameters())
            {
                var normalizedParameterName = _injectableValueNameNormalizer.Normalize(parameterInfo.Name);
                injectableValueInfos.Add(new InjectableValueInfo(normalizedParameterName,
                                                                 parameterInfo.Name,
                                                                 parameterInfo.ParameterType,
                                                                 InjectableValueKind.ConstructorParameter));
            }

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var propertyInfo in typeToAnalyze.GetRuntimeProperties())
            {
                var setMethodInfo = propertyInfo.SetMethod;
                if (setMethodInfo == null || setMethodInfo.IsPublic == false || setMethodInfo.IsStatic)
                    continue;

                var normalizedPropertyName = _injectableValueNameNormalizer.Normalize(propertyInfo.Name);
                var injectableValuesInfo = new InjectableValueInfo(normalizedPropertyName,
                                                                   propertyInfo.Name,
                                                                   propertyInfo.PropertyType,
                                                                   InjectableValueKind.PropertySetter);
                if (injectableValueInfos.Contains(injectableValuesInfo) == false)
                    injectableValueInfos.Add(injectableValuesInfo);
            }

            return new TypeConstructionInfo(typeToAnalyze, targetContructor, injectableValueInfos);
        }
    }
}