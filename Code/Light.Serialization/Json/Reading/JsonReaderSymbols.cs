using System;
using System.Collections.Generic;

namespace Light.Serialization.Json.Reading
{
    public sealed class JsonReaderSymbols
    {
        private string _true = DefaultJsonSymbols.True;

        public string True
        {
            get { return _true; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _true = value;
            }
        }

        private string _false = DefaultJsonSymbols.False;

        public string False
        {
            get { return _false; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _false = value;
            }
        }

        private string _null = DefaultJsonSymbols.Null;

        public string Null
        {
            get { return _null; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _null = value;
            }
        }

        public char StringDelimiter = DefaultJsonSymbols.StringDelimiter;
        public char DecimalPoint = DefaultJsonSymbols.DecimalPoint;


        private IList<char> _exponentialSymbols = new[] { DefaultJsonSymbols.LowercaseExponential, DefaultJsonSymbols.UppercaseExponential };

        public IList<char> ExponentialSymbols
        {
            get { return _exponentialSymbols; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _exponentialSymbols = value;
            }
        }

        public char PositiveSign = DefaultJsonSymbols.Positive;
        public char NegativeSign = DefaultJsonSymbols.Negative;
        public char StringEscapeCharacter = DefaultJsonSymbols.StringEscapeCharacter;


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
                if (value == null) throw new ArgumentNullException(nameof(value));
                _singleEscapedCharacters = value;
            }
        }

        public char HexadecimalEscapeIndicator = 'u';
        public char BeginOfArray = DefaultJsonSymbols.BeginOfArray;
        public char EndOfArray = DefaultJsonSymbols.EndOfArray;
        public char ValueDelimiter = DefaultJsonSymbols.ValueDelimiter;
        public char PairDelimiter = DefaultJsonSymbols.PairDelimiter;
        public char BeginOfObject = DefaultJsonSymbols.BeginOfObject;
        public char EndOfObject = DefaultJsonSymbols.EndOfObject;
    }
}