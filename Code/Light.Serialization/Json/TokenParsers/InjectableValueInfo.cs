using System;

namespace Light.Serialization.Json.TokenParsers
{
    public sealed class InjectableValueInfo : IEquatable<InjectableValueInfo>
    {
        public readonly string ValueName;
        public readonly Type Type;
        public readonly InjectableValueKind Kind;

        public InjectableValueInfo(string valueName, Type type, InjectableValueKind kind)
        {
            if (valueName == null) throw new ArgumentNullException(nameof(valueName));
            if (type == null) throw new ArgumentNullException(nameof(type));

            ValueName = valueName;
            Type = type;
            Kind = kind;
        }

        public bool Equals(InjectableValueInfo other)
        {
            if (other == null)
                return false;

            return ValueName == other.ValueName;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as InjectableValueInfo);
        }

        public override int GetHashCode()
        {
            return ValueName.GetHashCode();
        }
    }
}