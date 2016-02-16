using System;

namespace Light.Serialization.Json.TokenParsers
{
    [Flags]
    public enum InjectableValueKind
    {
        ConstructorParameter,
        PropertySetter,
        SettableField,
        UnknownOnTargetObject
    }
}