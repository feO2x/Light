using System;
using System.Reflection;
using Light.GuardClauses;

namespace Light.Serialization.Json.ComplexTypeConstruction
{
    public sealed class ComplexTypeCreationAnalysis
    {
        public readonly Type TargetType;

        public ComplexTypeCreationAnalysis(Type targetType)
        {
            targetType.MustNotBeNull(nameof(targetType));

            TargetType = targetType;
        }
    }

    public sealed class ConstructorAnalysis
    {
        public readonly ConstructorInfo TargetConstructor;

        public ConstructorAnalysis(ConstructorInfo targetConstructor)
        {
            targetConstructor.MustNotBeNull(nameof(targetConstructor));

            TargetConstructor = targetConstructor;
        }
    }

    public sealed class InjectableValue
    {
        public readonly string NormalizedName;
        public readonly string ActualName;
        public readonly Type Type;

        public InjectableValue(string normalizedName, string actualName, Type type)
        {
            normalizedName.MustNotBeNullOrWhiteSpace(nameof(normalizedName));
            actualName.MustNotBeNullOrWhiteSpace(nameof(actualName));
            type.MustNotBeNull(nameof(type));

            NormalizedName = normalizedName;
            ActualName = actualName;
            Type = type;
        }
    }
}
