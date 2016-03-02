using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Light.GuardClauses;

namespace Light.Serialization.Json.IntegerMetadata
{
    public sealed class UnsignedIntegerTypeInfo
    {
        public readonly Type Type;
        public readonly string MaximumAsString;
        public readonly Func<ulong, object> DowncastValue;

        public UnsignedIntegerTypeInfo(Type type, string maximumAsString, Func<ulong, object> downcastValue)
            
        {
            type.MustNotBeNull(nameof(type));
            maximumAsString.MustMatch(new Regex("[1-9][0-9]*"), nameof(maximumAsString));
            downcastValue.MustNotBeNull(nameof(downcastValue));

            Type = type;
            MaximumAsString = maximumAsString;
            DowncastValue = downcastValue;
        }

        public static Dictionary<Type, UnsignedIntegerTypeInfo> CreateDefaultUnsignedIntegerTypes()
        {
            return new[]
                   {
                       new UnsignedIntegerTypeInfo(typeof (uint), uint.MaxValue.ToString(), ul => (uint) ul),
                       new UnsignedIntegerTypeInfo(typeof (ulong), ulong.MaxValue.ToString(), ul => ul),
                       new UnsignedIntegerTypeInfo(typeof (ushort), ushort.MaxValue.ToString(), ul => (ushort) ul),
                       new UnsignedIntegerTypeInfo(typeof (byte), byte.MaxValue.ToString(), ul => (byte) ul)
                   }.ToDictionary(t => t.Type);
        }
    }
}