using System;
using System.Collections.Generic;
using System.Reflection;

namespace Light.Serialization.Json.TokenParsers
{
    public sealed class DefaultTypeCreationInfoAnalyzer : ITypeCreationInfoAnalyzer
    {
        private readonly IConstructorSelector _constructorsSelector;

        public DefaultTypeCreationInfoAnalyzer(IConstructorSelector constructorsSelector)
        {
            if (constructorsSelector == null) throw new ArgumentNullException(nameof(constructorsSelector));

            _constructorsSelector = constructorsSelector;
        }

        public TypeConstructionInfo CreateInfo(Type typeToAnalyze)
        {
            if (typeToAnalyze == null) throw new ArgumentNullException(nameof(typeToAnalyze));

            if (typeToAnalyze.IsAbstract || typeToAnalyze.IsInterface)
                throw new ArgumentException($"The specified type {typeToAnalyze.FullName} is abstract and cannot be deserialized", nameof(typeToAnalyze));

            var constructors = typeToAnalyze.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
            if (constructors.Length == 0)
                throw new ArgumentException($"The specified type {typeToAnalyze.FullName} does not have public constructors", nameof(typeToAnalyze));
            var targetContructor = constructors.Length == 1 ? constructors[0] : _constructorsSelector.SelectConstructor(constructors, typeToAnalyze);

            var injectableValueInfos = new List<InjectableValueInfo>();
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var parameterInfo in targetContructor.GetParameters())
            {
                injectableValueInfos.Add(new InjectableValueInfo(parameterInfo.Name, parameterInfo.ParameterType, InjectableValueKind.ConstructorParameter));
            }

            foreach (var propertyInfo in typeToAnalyze.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var setMethodInfo = propertyInfo.GetSetMethod();
                if (setMethodInfo != null)
                    injectableValueInfos.Add(new InjectableValueInfo(propertyInfo.Name, propertyInfo.PropertyType, InjectableValueKind.PropertySetter));
            }

            return new TypeConstructionInfo(injectableValueInfos);
        }
    }
}