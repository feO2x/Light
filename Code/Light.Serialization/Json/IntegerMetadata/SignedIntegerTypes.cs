using System;
using System.Collections.Generic;
using System.Linq;
using Light.GuardClauses;

namespace Light.Serialization.Json.IntegerMetadata
{
    public sealed class SignedIntegerTypes
    {
        public readonly SignedIntegerTypeInfo DefaultType;
        public readonly Dictionary<Type, SignedIntegerTypeInfo> IntegerTypeInfos;

        public SignedIntegerTypes(Dictionary<Type, SignedIntegerTypeInfo> integerTypeInfos, SignedIntegerTypeInfo defaultType)
        {
            integerTypeInfos.MustNotBeNull(nameof(integerTypeInfos));
            defaultType.MustNotBeNull(nameof(defaultType));

            IntegerTypeInfos = integerTypeInfos;
            DefaultType = defaultType;
        }

        public static SignedIntegerTypes CreateDefaultSignedIntegerTypes()
        {
            var signedIntegerTypes = new[]
                                     {
                                         new SignedIntegerTypeInfo(typeof (int), int.MinValue.ToString(), int.MaxValue.ToString(), l => (int) l),
                                         new SignedIntegerTypeInfo(typeof (long), long.MinValue.ToString(), long.MaxValue.ToString(), l => l),
                                         new SignedIntegerTypeInfo(typeof (short), short.MinValue.ToString(), short.MaxValue.ToString(), l => (short) l),
                                         new SignedIntegerTypeInfo(typeof (sbyte), sbyte.MinValue.ToString(), sbyte.MaxValue.ToString(), l => (sbyte) l)
                                     };

            return new SignedIntegerTypes(signedIntegerTypes.ToDictionary(i => i.Type), signedIntegerTypes[0]);
        }
    }
}