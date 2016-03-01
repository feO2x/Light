using System;
using System.Collections.Generic;

namespace Light.Serialization.Json.TokenParsers
{
    public interface IJsonStringToPrimitiveParser : IJsonTokenParser
    {
        IReadOnlyList<Type> AssociatedInterfacesAndBaseClasses { get; }

        ParseResult TryParse(JsonToken token);
    }
}