using System.Collections.Generic;
using System.Linq;
using Light.GuardClauses;

namespace Light.Serialization.Json.ComplexTypeConstruction
{
    public sealed class DefaultObjectFactory : IObjectFactory
    {
        public object Create(TypeCreationDescription typeCreationDescription, Dictionary<InjectableValueDescription, object> deserializedChildValues)
        {
            typeCreationDescription.MustNotBeNull(nameof(typeCreationDescription));

            if (deserializedChildValues == null || deserializedChildValues.Count == 0)
            {
                var defaultConstructorDescription = typeCreationDescription.ConstructorDescriptions.FirstOrDefault(c => c.ConstructorParameters.Count == 0);
                if (defaultConstructorDescription == null)
                    throw new DeserializationException($"Could not create instance of type {typeCreationDescription.TargetType.FullName} because there was not any JSON data and no default constructor."); // TODO: maybe we can express this a little bit clearer

                return defaultConstructorDescription.TryCallConstructor(null);
            }

            object newObject = null;
            foreach (var constructorDescription in typeCreationDescription.ConstructorDescriptions
                                                                          .OrderByDescending(c => c.ConstructorParameters.Count))
            {
                newObject = constructorDescription.TryCallConstructor(deserializedChildValues);
                if (newObject != null)
                    goto SetPropertiesAndFields;
            }

            if (newObject == null)
                throw new DeserializationException($"The specified type {typeCreationDescription.TargetType.FullName} cannot be created with the given type info."); // TODO: add the deserialized values to this exception message

            SetPropertiesAndFields:
            foreach (var injectablePropertyInfo in deserializedChildValues.Keys.Where(i => i.Kind == InjectableValueKind.PropertySetter))
            {
                injectablePropertyInfo.SetPropertyValue(newObject, deserializedChildValues[injectablePropertyInfo]);
            }

            foreach (var injectableFieldInfo in deserializedChildValues.Keys.Where(i => i.Kind == InjectableValueKind.SettableField))
            {
                injectableFieldInfo.SetFieldValue(newObject, deserializedChildValues[injectableFieldInfo]);
            }

            // TODO: set values that could not be matched with a constructor parameter, property setter or public field to a dictionary of some kind if possible

            return newObject;
        }
    }
}