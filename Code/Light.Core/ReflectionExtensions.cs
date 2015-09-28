using System;
using System.Collections.Generic;

namespace Light.Core
{
    public static class ReflectionExtensions
    {
        public static IList<Type> GetAllInterfacesOfInheritanceHierarchy(this Type type)
        {
            var interfaceTypes = new List<Type>();
            return GetAllInterfacesOfInheritanceHierarchy(type, interfaceTypes);
        }

        public static IList<Type> GetAllInterfacesOfInheritanceHierarchy(this Type type, IList<Type> interfaceTypes)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (interfaceTypes == null) throw new ArgumentNullException(nameof(interfaceTypes));

            PopulateInterfacesTypes(type, interfaceTypes);
            return interfaceTypes;
        }

        private static void PopulateInterfacesTypes(Type type, ICollection<Type> interfaceTypes)
        {
            while (true)
            {
                var interfaces = type.GetInterfaces();
                foreach (var @interface in interfaces)
                {
                    interfaceTypes.Add(@interface);
                    PopulateInterfacesTypes(@interface, interfaceTypes);
                }

                var baseClass = type.BaseType;
                if (baseClass != null)
                {
                    type = baseClass;
                    continue;
                }
                break;
            }
        }

        public static bool ImplementsGenericInterface(this Type type, Type genericInterface)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (genericInterface == null) throw new ArgumentNullException(nameof(genericInterface));
            if (genericInterface.IsInterface == false || genericInterface.IsGenericTypeDefinition == false)
                throw new ArgumentException($"Parameter 'genericInterface' is no generic interface definition: {genericInterface}");

            var allInterfaces = type.GetAllInterfacesOfInheritanceHierarchy();
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < allInterfaces.Count; i++)
            {
                var @interface = allInterfaces[i];
                if (@interface.IsGenericType == false)
                    continue;
                if (@interface.IsGenericTypeDefinition == false)
                    @interface = @interface.GetGenericTypeDefinition();
                if (@interface == genericInterface)
                    return true;
            }
            return false;
        }

        public static Type GetSpecificTypeThatCorrespondsToGenericInterface(this Type sourceType, Type genericTypeDefinition)
        {
            if (sourceType == null) throw new ArgumentNullException(nameof(sourceType));
            if (genericTypeDefinition == null) throw new ArgumentNullException(nameof(genericTypeDefinition));

            var allInterfaces = sourceType.GetAllInterfacesOfInheritanceHierarchy();
            // ReSharper disable once ForCanBeConvertedToForeach
            // ReSharper disable once LoopCanBeConvertedToQuery
            for (var i = 0; i < allInterfaces.Count; i++)
            {
                var @interface = allInterfaces[i];
                if (@interface.IsGenericType == false)
                    continue;
                if (@interface.IsGenericTypeDefinition == false &&
                    @interface.GetGenericTypeDefinition() == genericTypeDefinition)
                    return @interface;
            }
            return null;
        }
    }
}
