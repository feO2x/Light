using System;
using System.Collections.Generic;
using Light.GuardClauses;

namespace Light.Serialization.Json.ComplexTypeConstruction
{
    public sealed class TypeCreationDescription
    {
        public readonly Type TargetType;
        public readonly List<InjectableValueDescription> InjectableValueInfos;

        public TypeCreationDescription(Type targetType, List<InjectableValueDescription> injectableValueInfos)
        {
            targetType.MustNotBeNull(nameof(targetType));
            injectableValueInfos.MustNotBeNullOrEmpty(nameof(injectableValueInfos));

            TargetType = targetType;
            InjectableValueInfos = injectableValueInfos;
        }
    }
}