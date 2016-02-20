using System.Collections.Generic;
using Light.GuardClauses;

namespace Light.Serialization.Json.ComplexTypeConstruction
{
    public struct ConstructorCallInfo
    {
        private readonly ConstructorDescription _constructorDescription;
        private readonly List<InjectableValueInfo> _injectableValueInfos;

        public ConstructorCallInfo(ConstructorDescription constructorDescription, List<InjectableValueInfo> injectableValueInfos)
        {
            constructorDescription.MustNotBeNull(nameof(constructorDescription));
            injectableValueInfos.MustNotBeNull(nameof(injectableValueInfos));

            _constructorDescription = constructorDescription;
            _injectableValueInfos = injectableValueInfos;
        }

        public bool AreAllParametersPresent
        {
            get
            {
                if (_injectableValueInfos.Count == 0)
                    return true;

                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach (var parameterInfo in _injectableValueInfos)
                {
                    if (parameterInfo.HasDeserializedValue == false)
                        return false;
                }

                return true;
            }
        }
    }
}