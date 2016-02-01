using System;
using System.Reflection;

namespace Light.Serialization.Json.ComplexTypeDecomposition
{
    public sealed class ConstructorWithLongestParameterListSelector : IConstructorSelector
    {
        public ConstructorInfo SelectConstructor(ConstructorInfo[] constructorInfos, Type typeToAnalyze)
        {
            if (constructorInfos == null) throw new ArgumentNullException(nameof(constructorInfos));
            if (typeToAnalyze == null) throw new ArgumentNullException(nameof(typeToAnalyze));
            if (constructorInfos.Length < 2)
                throw new ArgumentException($"constructorInfos should have at least two items, but only has {constructorInfos.Length}.", nameof(constructorInfos));

            var numberOfParameters = -1;
            ConstructorInfo targetConstructor = null;

            foreach (var constructorInfo in constructorInfos)
            {
                var parameters = constructorInfo.GetParameters();
                if (parameters.Length <= numberOfParameters) continue;

                targetConstructor = constructorInfo;
                numberOfParameters = parameters.Length;
            }

            return targetConstructor;
        }
    }
}