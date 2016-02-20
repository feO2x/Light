namespace Light.Serialization.Json.ComplexTypeConstruction
{
    public struct InjectableValueInfo
    {
        private readonly InjectableValueDescription _description;
        public object DeserializedValue;

        public InjectableValueInfo(InjectableValueDescription description)
        {
            _description = description;
            DeserializedValue = null;
        }

        public string NormalizedName => _description.NormalizedName;

        public InjectableValueKind Kind => _description.Kind;

        public string ConstructorParameterName => _description.ConstructorParameterName;
        public string PropertyName => _description.PropertyName;
        public string FieldName => _description.FieldName;
        public bool HasDeserializedValue => DeserializedValue != null;
    }
}