using System;
using System.Collections.Generic;

namespace Light.Serialization.Json.TokenParsers
{
    public struct TypeConstructionInfo
    {
        public readonly List<InjectableValueInfo> InjectableValueInfos;

        public TypeConstructionInfo(List<InjectableValueInfo> injectableValueInfos)
        {
            if (injectableValueInfos == null) throw new ArgumentNullException(nameof(injectableValueInfos));

            InjectableValueInfos = injectableValueInfos;
        }

        public InjectableValueInfo GetInjectableValueInfoFromName(string name)
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var injectableValueInfo in InjectableValueInfos)
            {
                if (injectableValueInfo.ValueName == name)
                    return injectableValueInfo;
            }
            return null;
        }
    }
}