using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Light.GuardClauses;
using Light.Serialization.Json.TokenParsers;

namespace Light.Serialization.Json.ComplexTypeDecomposition
{
    public sealed class DefaultObjectFactory : IObjectFactory
    {
        private readonly IInjectableValueNameNormalizer _normalizer;

        public DefaultObjectFactory(IInjectableValueNameNormalizer normalizer)
        {
            normalizer.MustNotBeNull(nameof(normalizer));

            _normalizer = normalizer;
        }

        public object Create(TypeCreationInfo typeCreationInfo, Dictionary<InjectableValueInfo, object> deserializedChildValues)
        {
            var targetTypeInfo = typeCreationInfo.TargetType.GetTypeInfo();

            if (deserializedChildValues == null || deserializedChildValues.Count == 0)
            {
                var defaultConstructor = targetTypeInfo.DeclaredConstructors.FirstOrDefault(c => c.GetParameters().Length == 0);
                if (defaultConstructor == null)
                    throw new DeserializationException($"Could not create instance of type {typeCreationInfo.TargetType.FullName} because there was not any JSON data and no default constructor."); // TODO: maybe we can express this a little bit clearer

                return defaultConstructor.Invoke(null);
            }

            object newObject = null;
            foreach (var constructorInfo in targetTypeInfo.DeclaredConstructors
                                                          .Where(c => c.IsPublic && c.IsStatic == false)
                                                          .OrderByDescending(c => c.GetParameters().Length))
            {
                var parameters = constructorInfo.GetParameters();
                if (parameters.Length > deserializedChildValues.Count)
                    continue;

                newObject = CreateObjectIfPossible(constructorInfo, parameters, deserializedChildValues);
                if (newObject != null)
                    break;
            }

            if (newObject == null)
                throw new DeserializationException($"The specified type {typeCreationInfo.TargetType.FullName} cannot be created with the given type info."); // TODO: add the deserialized values to this exception message

            foreach (var injectablePropertyInfo in deserializedChildValues.Keys.Where(i => i.Kind == InjectableValueKind.PropertySetter))
            {
                var propertyInfo = typeCreationInfo.TargetType.GetRuntimeProperty(injectablePropertyInfo.ActualName);
                propertyInfo.SetMethod.Invoke(newObject, new[] { deserializedChildValues[injectablePropertyInfo] });
            }

            foreach (var injectableFieldInfo in deserializedChildValues.Keys.Where(i => i.Kind == InjectableValueKind.SettableField))
            {
                var fieldInfo = typeCreationInfo.TargetType.GetRuntimeField(injectableFieldInfo.ActualName);
                fieldInfo.SetValue(newObject, deserializedChildValues[injectableFieldInfo]);
            }

            // TODO: set values that could not be matched with a constructor parameter, property setter or public field to a dictionary of some kind if possible

            return newObject;
        }

        private object CreateObjectIfPossible(ConstructorInfo constructorInfo,
                                              ParameterInfo[] constructorParameters,
                                              Dictionary<InjectableValueInfo, object> deserializedChildValues)
        {
            if (constructorParameters.Length == 0)
                return constructorInfo.Invoke(null);

            var passedParameters = new object[constructorParameters.Length];

            for (var i = 0; i < constructorParameters.Length; i++)
            {
                var parameterInfo = constructorParameters[i];
                var normalizedParameterName = _normalizer.Normalize(parameterInfo.Name);

                var correspondingInjectableValueInfo = deserializedChildValues.Keys.FirstOrDefault(info => info.NormalizedName == normalizedParameterName);
                if (correspondingInjectableValueInfo == null)
                    return null;

                passedParameters[i] = deserializedChildValues[correspondingInjectableValueInfo];
            }

            return constructorInfo.Invoke(passedParameters);
        }
    }
}