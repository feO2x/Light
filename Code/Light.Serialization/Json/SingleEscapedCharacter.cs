namespace Light.Serialization.Json
{
    public struct SingleEscapedCharacter
    {
        public readonly char EscapedCharacter;
        public readonly char ValueAfterEscapeCharacter;

        public SingleEscapedCharacter(char escapedCharacter, char valueAfterEscapeCharacter)
        {
            EscapedCharacter = escapedCharacter;
            ValueAfterEscapeCharacter = valueAfterEscapeCharacter;
        }
    }
}