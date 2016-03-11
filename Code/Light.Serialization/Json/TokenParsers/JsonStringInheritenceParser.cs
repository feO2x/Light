using System;
using System.Collections.Generic;
using System.Linq;
using Light.GuardClauses;

namespace Light.Serialization.Json.TokenParsers
{
    public sealed class JsonStringInheritenceParser : IJsonTokenParser
    {
        public bool CanBeCached => false;

        private readonly StringParser _stringParser;
        private readonly IReadOnlyList<IJsonStringToPrimitiveParser> _stringToPrimitiveParsers;
        private object _lastParsedValue;

        public JsonStringInheritenceParser(IReadOnlyList<IJsonStringToPrimitiveParser> stringToPrimitiveParsers, StringParser stringParser)
        {
            stringToPrimitiveParsers.MustNotBeNull(nameof(stringToPrimitiveParsers));
            stringParser.MustNotBeNull(nameof(stringParser));

            _stringToPrimitiveParsers = stringToPrimitiveParsers;
            _stringParser = stringParser;
        }

        public bool IsSuitableFor(JsonToken token, Type requestedType)
        {
            if (token.JsonType != JsonTokenType.String)
                return false;

            // Check if there is any primitive parser that can make sense of the JSON string
            foreach (var parser in _stringToPrimitiveParsers.Where(p => p.AssociatedInterfacesAndBaseClasses.Contains(requestedType)))
            {
                var parseResult = parser.TryParse(token);
                if (parseResult.WasTokenParsedSuccessfully == false)
                    continue;

                _lastParsedValue = parseResult.ParsedObject;
                return true;
            }

            // If the string could not be interpreted, then use the StringParser if possible
            if (requestedType == typeof (ValueType) ||
                requestedType == typeof (Enum) ||
                requestedType == typeof (Delegate))
                return false;

            _lastParsedValue = _stringParser.ParseValue(token);
            return true;
        }

        public object ParseValue(JsonDeserializationContext context)
        {
            var returnValue = _lastParsedValue;
            _lastParsedValue = null;
            return returnValue;
        }
    }
}