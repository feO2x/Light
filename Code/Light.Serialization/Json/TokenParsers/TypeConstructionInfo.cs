using System;
using System.Collections.Generic;
using System.Reflection;

namespace Light.Serialization.Json.TokenParsers
{
    public struct TypeConstructionInfo
    {
        public readonly Type TargetType;
        public readonly ConstructorInfo TargetConstructor;
        public readonly List<InjectableValueInfo> InjectableValueInfos;

        public TypeConstructionInfo(Type targetType, ConstructorInfo targetConstructor, List<InjectableValueInfo> injectableValueInfos)
        {
            if (targetType == null) throw new ArgumentNullException(nameof(targetType));
            if (targetConstructor == null) throw new ArgumentNullException(nameof(targetConstructor));
            if (injectableValueInfos == null) throw new ArgumentNullException(nameof(injectableValueInfos));

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
    }
}