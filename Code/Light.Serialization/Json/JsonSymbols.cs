using System.Collections.Generic;
using Light.Serialization.Json.LowLevelReading;

namespace Light.Serialization.Json
{
    public static class JsonSymbols
    {
        public const string Number = "number";
        public const string Array = "array";
        public const string Object = "object";
        public const string String = "string";
        public const string True = "true";
        public const string False = "false";
        public const string Null = "null";
        public const char LowercaseExponential = 'e';
        public const char UppercaseExponential = 'E';
        public const char BeginOfObject = '{';
        public const char EndOfObject = '}';
        public const char BeginOfArray = '[';
        public const char EndOfArray = ']';
        public const char StringDelimiter = '"';
        public const char PairDelimiter = ':';
        public const char ValueDelimiter = ',';
        public const char DecimalPoint = '.';
        public const char Plus = '+';
        public const char Minus = '-';
        public const char StringEscapeCharacter = '\\';
        public const char HexadecimalEscapeIndicator = 'u';
        public const string DefaultIdSymbol = "$id";
        public const string DefaultReferenceSymbol = "$ref";


        public static readonly IReadOnlyList<SingleEscapedCharacter> SingleEscapedCharacters =
            new[]
            {
                new SingleEscapedCharacter('"', '"'),
                new SingleEscapedCharacter('\\', '\\'),
                new SingleEscapedCharacter('/', '/'),
                new SingleEscapedCharacter('\b', 'b'),
                new SingleEscapedCharacter('\f', 'f'),
                new SingleEscapedCharacter('\n', 'n'),
                new SingleEscapedCharacter('\r', 'r'),
                new SingleEscapedCharacter('\t', 't')
            };

        public static bool IsExponentialSymbol(this char character)
        {
            return character == LowercaseExponential || character == UppercaseExponential;
        }
    }
}