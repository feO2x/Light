using System;
using Light.GuardClauses;
using Light.GuardClauses.FrameworkExtensions;

namespace Light.Serialization.Json.Caching
{
    public struct JsonTokenTypeCombination : IEquatable<JsonTokenTypeCombination>
    {
        private readonly int _hashCode;
        public readonly JsonTokenType JsonTokenType;
        public readonly Type Type;

        public JsonTokenTypeCombination(JsonTokenType jsonTokenType, Type type)
        {
            jsonTokenType.MustBeValidEnumValue(nameof(jsonTokenType));
            type.MustNotBeNull(nameof(type));

            JsonTokenType = jsonTokenType;
            Type = type;

            _hashCode = EqualityHelper.CreateHashCode(jsonTokenType, type);
        }

        public bool Equals(JsonTokenTypeCombination other)
        {
            return JsonTokenType == other.JsonTokenType &&
                   Type == other.Type;
        }

        public override bool Equals(object obj)
        {
            try
            {
                return base.Equals((JsonTokenTypeCombination) obj);
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return _hashCode;
        }
    }
}