using Light.GuardClauses;
using System;

namespace Light.Serialization.Json.TokenParsers
{
    public sealed class InjectableValueInfo : IEquatable<InjectableValueInfo>
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
}