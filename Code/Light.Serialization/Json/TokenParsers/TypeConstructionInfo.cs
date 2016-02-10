using Light.GuardClauses;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Light.Serialization.Json.TokenParsers
{
    public sealed class TypeConstructionInfo : IEquatable<TypeConstructionInfo>
    {
        public readonly Type TargetType;
        public readonly ConstructorInfo TargetConstructor;
        public readonly List<InjectableValueInfo> InjectableValueInfos;

        public TypeConstructionInfo(Type targetType, ConstructorInfo targetConstructor, List<InjectableValueInfo> injectableValueInfos)
        {
            targetType.MustNotBeNull(nameof(targetType));
            targetConstructor.MustNotBeNull(nameof(targetConstructor));
            injectableValueInfos.MustNotBeNullOrEmpty(nameof(injectableValueInfos));

            TargetType = targetType;
            TargetConstructor = targetConstructor;
            InjectableValueInfos = injectableValueInfos;
        }

        public InjectableValueInfo GetInjectableValueInfoFromName(string name)
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var injectableValueInfo in InjectableValueInfos)
            {
                if (injectableValueInfo.NormalizedName == name)
                    return injectableValueInfo;
            }
            return null;
        }

        public bool Equals(TypeConstructionInfo other)
        {
            if (other == null)
                return false;

            return TargetType == other.TargetType;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TypeConstructionInfo);
        }

        public override int GetHashCode()
        {
            return TargetType.GetHashCode();
        }
    }
}