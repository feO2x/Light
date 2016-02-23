using Light.Serialization.Json.ComplexTypeConstruction;
using System.Collections.Generic;
using System.Linq;
using Light.GuardClauses;
using Microsoft.Practices.Unity;

namespace Light.Serialization.UnityContainerIntegration
{
    public sealed class UnityObjectFactory : IObjectFactory
    {
        private readonly IUnityContainer _unityContainer;

        public UnityObjectFactory(IUnityContainer unityContainer)
        {
            unityContainer.MustNotBeNull(nameof(unityContainer));

            _unityContainer = unityContainer;
        }

        public object Create(TypeCreationDescription typeCreationDescription, Dictionary<InjectableValueDescription, object> deserializedChildValues)
        {
            var targetConstructorDescription = typeCreationDescription.ConstructorDescriptions
                                                                      .OrderByDescending(c => c.ConstructorParameters.Count)
                                                                      .First();

            object newObject;
            if (targetConstructorDescription.ConstructorParameters.Count == 0)
                newObject = targetConstructorDescription.TryCallConstructor(null);
            else
            {
                var parameterOverrides = new ParameterOverrides();
                foreach (var injectableValueInfo in targetConstructorDescription.ConstructorParameters)
                {
                    object parametervalue;
                    if (deserializedChildValues.TryGetValue(injectableValueInfo, out parametervalue) == false)
                        continue;

                    parameterOverrides.Add(injectableValueInfo.ConstructorParameterInfo.Name, parametervalue);
                }

                newObject = _unityContainer.Resolve(typeCreationDescription.TargetType, parameterOverrides);
            }

            foreach (var injectablePropertyInfo in deserializedChildValues.Keys.Where(i => i.Kind == InjectableValueKind.PropertySetter))
            {
                injectablePropertyInfo.SetPropertyValue(newObject, deserializedChildValues[injectablePropertyInfo]);
            }

            foreach (var injectableFieldInfo in deserializedChildValues.Keys.Where(i => i.Kind == InjectableValueKind.SettableField))
            {
                injectableFieldInfo.SetFieldValue(newObject, deserializedChildValues[injectableFieldInfo]);
            }

            return newObject;
        }
    }
}
