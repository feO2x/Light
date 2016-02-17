using Light.GuardClauses;
using System.Collections.Generic;
using System.Reflection;

namespace Light.Serialization.Json.ComplexTypeConstruction
{
    public sealed class ConstructorDescription
    {
        public readonly ConstructorInfo ConstructorInfo;

        public List<InjectableValueDescription> ConstructorParameters;

        public ConstructorDescription(ConstructorInfo constructorInfo, List<InjectableValueDescription> constructorParameters)
        {
            constructorInfo.MustNotBeNull(nameof(constructorInfo));
            constructorParameters.MustNotBeNull(nameof(constructorParameters));

            ConstructorInfo = constructorInfo;
            ConstructorParameters = constructorParameters;
        }
    }
}
