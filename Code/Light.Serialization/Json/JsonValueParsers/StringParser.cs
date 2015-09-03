using System;

namespace Light.Serialization.Json.JsonValueParsers
{
    public sealed class StringParser : IJsonValueParser
    {
        private readonly Type _stringType = typeof (string);

        public char StringDelimiterCharacter = '"';

        public bool IsSuitableFor(JsonCharacterBuffer buffer, Type requestedType)
        {
            return buffer.JsonType == JsonType.String && requestedType == _stringType;
        }

        public object DeserializeValue(JsonCharacterBuffer buffer, Type requestedType)
        {
            var stringArray = new char[buffer.Count - 2];

            var bufferIndex = 1;
            var stringArrayIndex = 0;
            while (bufferIndex < buffer.Count - 1)
            {
                stringArray[stringArrayIndex++] = buffer[bufferIndex++];
            }

            return new string(stringArray);
        }
    }
}
