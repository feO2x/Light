using System;
using System.Text.RegularExpressions;
using Light.GuardClauses;

namespace Light.Serialization.Json.IntegerMetadata
{
    public sealed class SignedIntegerTypeInfo
    {
        public readonly Type Type;
        public readonly string MinimumAsString;
        public readonly string MaximumAsString;
        public readonly Func<long, object> DowncastValue;

        public SignedIntegerTypeInfo(Type type, string minimumAsString, string maximumAsString, Func<long, object> downcastValue)
        {
            type.MustNotBeNull(nameof(type));
            minimumAsString.MustMatch(new Regex("-[1-9][0-9]*"), nameof(minimumAsString));
            maximumAsString.MustMatch(new Regex("[1-9][0-9]*"), nameof(maximumAsString));
            downcastValue.MustNotBeNull(nameof(downcastValue));

            Type = type;
            MinimumAsString = minimumAsString;
            MaximumAsString = maximumAsString;
            DowncastValue = downcastValue;
        }
    }
}