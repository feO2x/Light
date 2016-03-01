using System;
using System.Collections.Generic;
using System.Reflection;
using Light.GuardClauses;

namespace Light.Serialization.Json.TokenParsers
{
    public abstract class BaseJsonStringToPrimitiveParser<T>
    {
        private readonly List<Type> _associatedInterfacesAndBaseClasses = new List<Type>();

        protected BaseJsonStringToPrimitiveParser()
        {
            var typeInfo = typeof (T).GetTypeInfo();
            Guard.Against(typeInfo.IsInterface, () => new InvalidOperationException($"The specified type {typeInfo.FullName} is an interface and cannot be used with this base class."));
            Guard.Against(typeInfo.BaseType == typeof (Delegate), () => new InvalidOperationException($"The specified type {typeInfo.FullName} is a delegate and cannot be used with this base class."));

            _associatedInterfacesAndBaseClasses.Add(typeof (object));
            // If it is a value type, it can only be a struct or an enum
            if (typeInfo.IsValueType)
            {
                _associatedInterfacesAndBaseClasses.Add(typeof (ValueType));
                if (typeInfo.IsEnum)
                    _associatedInterfacesAndBaseClasses.Add(typeof (Enum));
                else
                {
                    foreach (var @interface in typeInfo.ImplementedInterfaces)
                    {
                        _associatedInterfacesAndBaseClasses.Add(@interface);
                    }
                }
                return;
            }

            // Else it is a class - get all base classes and interfaces along the inheritance hierarchy
            var currentType = typeInfo;
            while (currentType.GetType() != typeof (object))
            {
                foreach (var @interface in currentType.ImplementedInterfaces)
                {
                    if (_associatedInterfacesAndBaseClasses.Contains(@interface) == false)
                        _associatedInterfacesAndBaseClasses.Add(@interface);
                }

                if (_associatedInterfacesAndBaseClasses.Contains(currentType.BaseType) == false)
                    _associatedInterfacesAndBaseClasses.Add(currentType.BaseType);

                currentType = currentType.BaseType.GetTypeInfo();
            }
        }

        public IReadOnlyList<Type> AssociatedInterfacesAndBaseClasses => _associatedInterfacesAndBaseClasses;
    }
}