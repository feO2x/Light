using Light.GuardClauses;
using System.Collections.Generic;

namespace Light.Serialization.Json.LowLevelReading
{
    public sealed class JsonReaderSymbols
    {
        private string _true = JsonSymbols.True;

        public string True
        {
            get { return _true; }
            set
            {
                value.MustNotBeNull(nameof(value));
                _true = value;
            }
        }

        private string _false = JsonSymbols.False;

        public string False
        {
            get { return _false; }
            set
            {
                value.MustNotBeNull(nameof(value));
                _false = value;
            }
        }

        private string _null = JsonSymbols.Null;

        public string Null
        {
            get { return _null; }
            set
            {
                value.MustNotBeNull(nameof(value));
                _null = value;
            }
        }

        public char StringDelimiter = JsonSymbols.StringDelimiter;
        public char DecimalPoint = JsonSymbols.DecimalPoint;


        private IList<char> _exponentialSymbols = new[] { JsonSymbols.LowercaseExponential, JsonSymbols.UppercaseExponential };

        public IList<char> ExponentialSymbols
        {
            get { return _exponentialSymbols; }
            set
            {
                value.MustNotBeNull(nameof(value));
                _exponentialSymbols = value;
            }
        }

        public char PositiveSign = JsonSymbols.Positive;
        public char NegativeSign = JsonSymbols.Negative;
        public char StringEscapeCharacter = JsonSymbols.StringEscapeCharacter;


        private IList<SingleEscapedCharacter> _singleEscapedCharacters = new[]
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

        public IList<SingleEscapedCharacter> SingleEscapedCharacters
        {
            get { return _singleEscapedCharacters; }
            set
            {
                value.MustNotBeNull(nameof(value));
                _singleEscapedCharacters = value;
            }
        }

        public char HexadecimalEscapeIndicator = 'u';
        public char BeginOfArray = JsonSymbols.BeginOfArray;
        public char EndOfArray = JsonSymbols.EndOfArray;
        public char ValueDelimiter = JsonSymbols.ValueDelimiter;
        public char PairDelimiter = JsonSymbols.PairDelimiter;
        public char BeginOfObject = JsonSymbols.BeginOfObject;
        public char EndOfObject = JsonSymbols.EndOfObject;
    }
}