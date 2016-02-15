using Light.GuardClauses;
using System;
using System.Collections.Generic;

namespace Light.Serialization.Json.TokenParsers
{
    public sealed class TypeCreationInfo : IEquatable<TypeCreationInfo>
    {
        public readonly Type TargetType;
        public readonly List<InjectableValueInfo> InjectableValueInfos;

        public TypeCreationInfo(Type targetType, List<InjectableValueInfo> injectableValueInfos)
        {
            targetType.MustNotBeNull(nameof(targetType));
            injectableValueInfos.MustNotBeNullOrEmpty(nameof(injectableValueInfos));

            TargetType = targetType;
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

        public bool Equals(TypeCreationInfo other)
        {
            if (other == null)
                return false;

            return TargetType == other.TargetType;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TypeCreationInfo);
        }

        public override int GetHashCode()
        {
            return TargetType.GetHashCode();
        }
    }
}