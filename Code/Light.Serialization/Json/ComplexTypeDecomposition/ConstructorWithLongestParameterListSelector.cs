using Light.GuardClauses;
using System.Reflection;

namespace Light.Serialization.Json.ComplexTypeDecomposition
{
    public sealed class ConstructorWithLongestParameterListSelector : IConstructorSelector
    {
        public ConstructorInfo SelectConstructor(ConstructorInfo[] constructorInfos, TypeInfo typeToAnalyze)
        {
            constructorInfos.MustNotBeNull(nameof(constructorInfos));
            constructorInfos.Length.MustNotBeLessThan(2, nameof(constructorInfos), $"constructorInfos should have at least two items, but only has {constructorInfos.Length}.");
            typeToAnalyze.MustNotBeNull(nameof(typeToAnalyze));

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