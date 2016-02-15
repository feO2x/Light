using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Light.GuardClauses;
using Light.Serialization.Json.TokenParsers;

namespace Light.Serialization.Json.ComplexTypeDecomposition
{
    public sealed class DefaultTypeCreationInfoAnalyzer : ITypeCreationInfoAnalyzer
    {
        private readonly IInjectableValueNameNormalizer _injectableValueNameNormalizer;

        public DefaultTypeCreationInfoAnalyzer(IInjectableValueNameNormalizer injectableValueNameNormalizer)
        {
            injectableValueNameNormalizer.MustNotBeNull(nameof(injectableValueNameNormalizer));

            _injectableValueNameNormalizer = injectableValueNameNormalizer;
        }

        public TypeCreationInfo CreateInfo(Type typeToAnalyze)
        {
            typeToAnalyze.MustNotBeNull(nameof(typeToAnalyze));

            var typeInfo = typeToAnalyze.GetTypeInfo();

            if (typeInfo.IsAbstract || typeInfo.IsInterface)
                throw new ArgumentException($"The specified type {typeToAnalyze.FullName} is abstract and cannot be deserialized", nameof(typeToAnalyze));

            var constructors = typeInfo.DeclaredConstructors
                                       .Where(c => c.IsStatic == false && c.IsPublic);
            var injectableValueInfos = new List<InjectableValueInfo>();
            foreach (var constructorInfo in constructors)
            {
                foreach (var parameterInfo in constructorInfo.GetParameters())
                {
                    var normalizedParameterName = _injectableValueNameNormalizer.Normalize(parameterInfo.Name);

                    var injectableValueInfo = new InjectableValueInfo(normalizedParameterName,
                                                                      parameterInfo.Name,
                                                                      parameterInfo.ParameterType,
                                                                      InjectableValueKind.ConstructorParameter);

                    if (injectableValueInfos.Contains(injectableValueInfo) == false)
                        injectableValueInfos.Add(injectableValueInfo);
                }
            }

            foreach (var propertyInfo in typeToAnalyze.GetRuntimeProperties())
            {
                var setMethodInfo = propertyInfo.SetMethod;
                if (setMethodInfo == null || setMethodInfo.IsPublic == false || setMethodInfo.IsStatic)
                    continue;

                CreateInfoForPropertyOrField(propertyInfo.Name, propertyInfo.PropertyType, InjectableValueKind.PropertySetter, injectableValueInfos, typeToAnalyze);
            }

            foreach (var fieldInfo in typeToAnalyze.GetRuntimeFields())
            {
                if (fieldInfo.IsStatic || fieldInfo.IsPublic == false || fieldInfo.IsInitOnly)
                    continue;

                CreateInfoForPropertyOrField(fieldInfo.Name, fieldInfo.FieldType, InjectableValueKind.SettableField, injectableValueInfos, typeToAnalyze);
            }

            return new TypeCreationInfo(typeToAnalyze, injectableValueInfos);
        }

        private void CreateInfoForPropertyOrField(string actualName, Type memberType, InjectableValueKind kind, List<InjectableValueInfo> list, Type typeToAnalyze)
        {
            var normalizedName = _injectableValueNameNormalizer.Normalize(actualName);

            var existingInfoWithSameName = list.FirstOrDefault(i => i.NormalizedName == normalizedName);
            if (existingInfoWithSameName == null)
            {
                list.Add(new InjectableValueInfo(normalizedName, actualName, memberType, kind));
                return;
            }

            if (existingInfoWithSameName.Kind == InjectableValueKind.ConstructorParameter)
                return;

            throw new DeserializationException($"The type {typeToAnalyze.FullName} has two members that correspond to the same normalized name: {existingInfoWithSameName.ActualName} and {actualName} both are transformed to {normalizedName}. Please resolve this naming issue.");
        }
    }
}