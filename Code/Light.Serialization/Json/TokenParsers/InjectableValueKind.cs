namespace Light.Serialization.Json.TokenParsers
{
    public enum InjectableValueKind
    {
        ConstructorParameter,
        PropertySetter,
        SettableField,
        UnknownOnTargetObject
    }
}