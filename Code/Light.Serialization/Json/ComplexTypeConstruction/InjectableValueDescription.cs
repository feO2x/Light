using Light.GuardClauses;
using Light.Serialization.Json.TokenParsers;
using System;

namespace Light.Serialization.Json.ComplexTypeConstruction
{
    public sealed class InjectableValueDescription
    {
        public readonly string NormalizedName;
        private string _constructorParameterName;
        private string _propertyName;

        private InjectableValueKind _kind;
        public string ConstructorParameterName => _constructorParameterName;
        
        public readonly Type Type;
        public InjectableValueKind Kind => _kind;
        public string PropertyName => _propertyName;
        private string _fieldName;
        public string FieldName => _fieldName;

        private InjectableValueDescription(string normalizedName, Type type)
        {
            normalizedName.MustNotBeNullOrEmpty(nameof(normalizedName));
            type.MustNotBeNull(nameof(type));

            NormalizedName = normalizedName;
            Type = type;
        }

        public void AddConstructorParameterName(string parameterName)
        {
            _kind &= InjectableValueKind.ConstructorParameter;
            _constructorParameterName = parameterName;
        }

        public void AddPropertyName(string propertyName)
        {
            _kind &= InjectableValueKind.PropertySetter;
            _propertyName = propertyName;
        }

        public void AddFieldName(string fieldName)
        {
            _kind &= InjectableValueKind.SettableField;
            _fieldName = fieldName;
        }

        public static InjectableValueDescription FromConstructorParameter(string normalizedName, string actualName, Type parameterType)
        {
            var injectableValueInfo = new InjectableValueDescription(normalizedName, parameterType);
            injectableValueInfo.AddConstructorParameterName(actualName);
            return injectableValueInfo;
        }

        public static InjectableValueDescription FromProperty(string normalizedName, string actualName, Type propertyType)
        {
            var injectableValueInfo = new InjectableValueDescription(normalizedName, propertyType);
            injectableValueInfo.AddPropertyName(actualName);
            return injectableValueInfo;
        }

        public static InjectableValueDescription FromField(string normalizedName, string actualName, Type fieldType)
        {
            var injectableValueInfo = new InjectableValueDescription(normalizedName, fieldType);
            injectableValueInfo.AddFieldName(actualName);
            return injectableValueInfo;
        }

        public override string ToString()
        {
            return NormalizedName;
        }
    }
}