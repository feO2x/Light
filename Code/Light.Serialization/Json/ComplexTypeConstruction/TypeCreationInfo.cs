using System;
using Light.GuardClauses;

namespace Light.Serialization.Json.ComplexTypeConstruction
{
    public struct TypeCreationInfo
    {
        private readonly TypeCreationDescription _typeCreationDescription;

        public TypeCreationInfo(TypeCreationDescription typeCreationDescription)
        {
            typeCreationDescription.MustNotBeNull(nameof(typeCreationDescription));

            _typeCreationDescription = typeCreationDescription;
        }

        public Type TargetType => _typeCreationDescription.TargetType;

        public static TypeCreationInfo FromTypeCreationDescription(TypeCreationDescription description)
        {
            description.MustNotBeNull(nameof(description));
        }
    }
}
