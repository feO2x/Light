using System;
using System.Collections.Generic;

namespace Light.Serialization.Json
{
    public sealed class KnownJsonTokens
    {
        private string _trueToken = "true";

        public string TrueToken
        {
            get { return _trueToken; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _trueToken = value;
            }
        }

        private string _falseToken = "false";

        public string FalseToken
        {
            get { return _falseToken; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _falseToken = value;
            }
        }

        private string _nullToken = "null";

        public string NullToken
        {
            get { return _nullToken; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _nullToken = value;
            }
        }

        public char StringDelimiter = '"';


        public char DecimalPoint = '.';


        private IList<char> _exponentialTokens = new[] { 'e', 'E' };

        public IList<char> ExponentialTokens
        {
            get { return _exponentialTokens; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _exponentialTokens = value;
            }
        }

        public char PositiveSign = '+';

        public char NegativeSign = '-';

        public char StringEscapeCharacter = '\\';


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

        public char HexadicamalEscapeIndicator = 'u';
    }
}