using Light.GuardClauses;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Light.Serialization.Json.TokenParsers
{
    public class InjectableValueInfo : IEquatable<InjectableValueInfo>
    {
        public readonly string NormalizedName;
        public readonly string ActualName;
        public readonly Type Type;
        public readonly InjectableValueKind Kind;

        public InjectableValueInfo(string normalizedName, string actualName, Type type, InjectableValueKind kind)
        {
            normalizedName.MustNotBeNullOrEmpty(nameof(normalizedName));
            actualName.MustNotBeNullOrEmpty(nameof(actualName));
            type.MustNotBeNull(nameof(type));
            kind.MustBeValidEnumValue(nameof(kind));

            NormalizedName = normalizedName;
            ActualName = actualName;
            Type = type;
            Kind = kind;
        }

        public bool Equals(InjectableValueInfo other)
        {
            if (other == null)
                return false;

            return NormalizedName == other.NormalizedName;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as InjectableValueInfo);
        }

        public override int GetHashCode()
        {
            return NormalizedName.GetHashCode();
        }
    }

    public class ConstructorParameterInfo : InjectableValueInfo
    {
        public readonly ParameterInfo ParameterInfo;

        public ConstructorParameterInfo(string normalizedName, string actualName, ParameterInfo parameterInfo)
            : base(normalizedName, actualName, parameterInfo.ParameterType, InjectableValueKind.ConstructorParameter)
        {
            ParameterInfo = parameterInfo;
        }
    }
}