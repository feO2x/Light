using System;

namespace Light.Serialization.Json.TokenParsers
{
    public struct Iso8601DurationParser
    {
        public int Days;
        public int Hours;
        public int Minutes;
        public int Seconds;
        public int Milliseconds;
        private int _currentIndex;
        private bool _wasTDesignatorHit;
        private int _indexOfDot;

        public void SetValue(char iso8601Designator, int value)
        {
            switch (iso8601Designator)
            {
                case 'D':
                    Days = value;
                    break;
                case 'H':
                    Hours = value;
                    break;
                case 'M':
                    Minutes = value;
                    break;
                case 'S':
                    Seconds = value;
                    break;
                default:
                    throw new ArgumentException($"The specified ISO 8601 designator {iso8601Designator} is unknown.", nameof(iso8601Designator));
            }
        }

        public TimeSpan ParseToken(ref JsonToken token)
        {
            _currentIndex = 1;
            _wasTDesignatorHit = false;

            ExpectCharacter('P', ref token);

            while (_currentIndex < token.Length - 1)
            {
                if (_wasTDesignatorHit == false)
                    CheckForTDesignator(ref token);

                var startIndex = _currentIndex;
                var designator = ReadUntilDesignator(ref token);
                var numberOfDigitsToParse = _currentIndex - startIndex - 1;

                if (_wasTDesignatorHit == false ||
                    designator != 'S' ||
                    DoesNumberContainDot(startIndex, numberOfDigitsToParse, ref token) == false)
                {
                    var number = ReadNumber(numberOfDigitsToParse, ref token, startIndex);
                    AssignNumberAccordingToDesignator(number, designator, ref token);
                }
                else
                {
                    Seconds = ReadNumber(_indexOfDot - startIndex, ref token, startIndex);
                    Milliseconds = ReadNumber(3, ref token, _indexOfDot + 1);
                }
            }

            try
            {
                return new TimeSpan(Days, Hours, Minutes, Seconds, Milliseconds);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw CreateException(ref token, ex);
            }
            
        }

        private bool DoesNumberContainDot(int startIndex, int numberOfDigitsToParse, ref JsonToken token)
        {
            for (var i = 0; i < numberOfDigitsToParse; i++)
            {
                var currentCharacter = token[startIndex];
                if (currentCharacter == '.')
                {
                    _indexOfDot = startIndex;
                    return true;
                }

                startIndex++;
            }
            return false;
        }

        private void AssignNumberAccordingToDesignator(int number, char designator, ref JsonToken token)
        {
            switch (designator)
            {
                case 'D':
                    Days = number;
                    break;
                case 'H':
                    if (_wasTDesignatorHit == false)
                        throw CreateException(ref token);
                    Hours = number;
                    break;
                case 'M':
                    if (_wasTDesignatorHit == false)
                        throw CreateException(ref token);
                    Minutes = number;
                    break;
                case 'S':
                    if (_wasTDesignatorHit == false)
                        throw CreateException(ref token);
                    Seconds = number;
                    break;
                default:
                    throw CreateException(ref token);
            }
        }

        private static int ReadNumber(int expectedNumberOfDigits, ref JsonToken token, int startIndex)
        {
            var result = 0;
            for (var base10Position = expectedNumberOfDigits; base10Position > 0; base10Position--, startIndex++)
            {
                var digit = GetDigit(ref token, startIndex);
                result += digit * CalculateBase(base10Position);
            }
            return result;
        }

        private static int CalculateBase(int base10Position)
        {
            if (base10Position == 1)
                return 1;

            var result = 10;
            for (var i = 2; i < base10Position; i++)
            {
                result *= 10;
            }
            return result;
        }

        private char ReadUntilDesignator(ref JsonToken token)
        {
            while (true)
            {
                if (_currentIndex == token.Length - 1)
                    throw CreateException(ref token);

                var currentCharacter = token[_currentIndex++];
                if (char.IsDigit(currentCharacter))
                    continue;
                if (IsDesignator(currentCharacter))
                    return currentCharacter;
            }
        }

        private static int GetDigit(ref JsonToken token, int index)
        {
            var character = token[index];
            if (char.IsDigit(character) == false)
                throw CreateException(ref token);
            return character - '0';
        }

        private void CheckForTDesignator(ref JsonToken token)
        {
            var character = token[_currentIndex];
            if (character != 'T')
                return;

            _currentIndex++;
            _wasTDesignatorHit = true;
        }

        private static bool IsDesignator(char character)
        {
            return character == 'D' ||
                   character == 'H' ||
                   character == 'M' ||
                   character == 'S' ||
                   character == 'T';
        }

        private void ExpectCharacter(char character, ref JsonToken token)
        {
            if (token[_currentIndex++] != character)
                throw CreateException(ref token);
        }

        private static JsonDocumentException CreateException(ref JsonToken token, Exception innerException = null)
        {
            return new JsonDocumentException($"The specified token {token} does not represent a valid time span.", token, innerException);
        }
    }
}