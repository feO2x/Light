using System;
using System.Collections.Generic;
using System.Reflection;

namespace Light.Serialization.FrameworkExtensions
{
    public static class ReflectionExtensions
    {
        public static IList<Type> GetAllInterfacesOfInheritanceHierarchy(this TypeInfo type)
        {
            var interfaceTypes = new List<Type>();
            return GetAllInterfacesOfInheritanceHierarchy(type, interfaceTypes);
        }

        public static IList<Type> GetAllInterfacesOfInheritanceHierarchy(this TypeInfo type, IList<Type> interfaceTypes)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (interfaceTypes == null) throw new ArgumentNullException(nameof(interfaceTypes));

            PopulateInterfacesTypes(type, interfaceTypes);
            return interfaceTypes;
        }

        private static void PopulateInterfacesTypes(TypeInfo type, ICollection<Type> interfaceTypes)
        {
            while (true)
            {
                var interfaces = type.ImplementedInterfaces;
                foreach (var @interface in interfaces)
                {
                    interfaceTypes.Add(@interface);
                    PopulateInterfacesTypes(@interface.GetTypeInfo(), interfaceTypes);
                }

                var baseClass = type.BaseType;
                if (baseClass != null)
                {
                    type = baseClass.GetTypeInfo();
                    continue;
                }
                break;
            }
        }

        public static bool ImplementsGenericInterface(this TypeInfo type, TypeInfo genericInterface)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (genericInterface == null) throw new ArgumentNullException(nameof(genericInterface));
            if (genericInterface.IsInterface == false || genericInterface.IsGenericTypeDefinition == false)
                throw new ArgumentException($"Parameter 'genericInterface' is no generic interface definition: {genericInterface}");

            var allInterfaces = type.GetAllInterfacesOfInheritanceHierarchy();
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < allInterfaces.Count; i++)
            {
                var @interface = allInterfaces[i].GetTypeInfo();
                if (@interface.IsGenericType == false)
                    continue;
                if (@interface.IsGenericTypeDefinition == false)
                    @interface = @interface.GetGenericTypeDefinition().GetTypeInfo();
                if (@interface.Equals(genericInterface)) // TODO: I should include a comparison with GetHashCode first, this can be done after integrating Guard Clauses
                    return true;
            }
            return false;
        }

        public static TypeInfo GetSpecificTypeInfoThatCorrespondsToGenericInterface(this TypeInfo sourceType, TypeInfo genericTypeDefinition)
        {
            if (sourceType == null) throw new ArgumentNullException(nameof(sourceType));
            if (genericTypeDefinition == null) throw new ArgumentNullException(nameof(genericTypeDefinition));

            var allInterfaces = sourceType.GetAllInterfacesOfInheritanceHierarchy();
            // ReSharper disable once ForCanBeConvertedToForeach
            // ReSharper disable once LoopCanBeConvertedToQuery
            for (var i = 0; i < allInterfaces.Count; i++)
            {
                var @interface = allInterfaces[i].GetTypeInfo();
                if (@interface.IsGenericType == false)
                    continue;
                if (@interface.IsGenericTypeDefinition == false &&
                    @interface.GetGenericTypeDefinition().GetTypeInfo().Equals(genericTypeDefinition)) // TODO: same here
                    return @interface;
            }
            return null;
        }
    }
}
