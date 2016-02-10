using System;
using Light.GuardClauses;

namespace Light.Serialization.Json.LowLevelWriting
{
    public sealed class JsonWriterNormalizedKeysDecorator : IJsonWriter
    {
        private readonly IJsonWriter _jsonWriter;

        public JsonWriterNormalizedKeysDecorator(IJsonWriter jsonWriter)
        {
            jsonWriter.MustNotBeNull(nameof(jsonWriter));

            _jsonWriter = jsonWriter;
        }

        public void BeginArray()
        {
            _jsonWriter.BeginArray();
        }

        public void EndArray()
        {
            _jsonWriter.EndArray();
        }

        public void BeginObject()
        {
            _jsonWriter.BeginObject();
        }

        public void EndObject()
        {
            _jsonWriter.EndObject();
        }

        public void WriteKey(string key)
        {
            var normalizedKey = NormalizeJsonKey(key);
            _jsonWriter.WriteKey(normalizedKey);
        }

        public void WriteDelimiter()
        {
            _jsonWriter.WriteDelimiter();
        }

        public void WritePrimitiveValue(string @string)
        {
            _jsonWriter.WritePrimitiveValue(@string);
        }

        public void WriteNull()
        {
            _jsonWriter.WriteNull();
        }

        private static string NormalizeJsonKey(string key)
        {
            if (key.Length < 1)
                return key;

            return Char.ToLowerInvariant(key[0]) + key.Substring(1);
        }
    }
}