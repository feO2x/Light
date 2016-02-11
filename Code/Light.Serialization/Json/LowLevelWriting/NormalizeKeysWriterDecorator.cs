using Light.GuardClauses;
using Light.Serialization.FrameworkExtensions;

namespace Light.Serialization.Json.LowLevelWriting
{
    public sealed class NormalizeKeysWriterDecorator : IJsonWriter
    {
        private readonly IJsonWriter _decoratedWriter;

        public NormalizeKeysWriterDecorator(IJsonWriter decoratedWriter)
        {
            decoratedWriter.MustNotBeNull(nameof(decoratedWriter));

            _decoratedWriter = decoratedWriter;
        }

        public void BeginArray()
        {
            _decoratedWriter.BeginArray();
        }

        public void EndArray()
        {
            _decoratedWriter.EndArray();
        }

        public void BeginObject()
        {
            _decoratedWriter.BeginObject();
        }

        public void EndObject()
        {
            _decoratedWriter.EndObject();
        }

        public void WriteKey(string key)
        {
            var normalizedKey = NormalizeJsonKey(key);
            _decoratedWriter.WriteKey(normalizedKey);
        }

        public void WriteDelimiter()
        {
            _decoratedWriter.WriteDelimiter();
        }

        public void WritePrimitiveValue(string @string)
        {
            _decoratedWriter.WritePrimitiveValue(@string);
        }

        public void WriteNull()
        {
            _decoratedWriter.WriteNull();
        }

        private static string NormalizeJsonKey(string key)
        {
            if (key.Length < 1) // TODO: Are empty strings allowed as a key value? I don't think so - Kenny
                return key;

            return key.MakeFirstCharacterLowercase();
        }
    }
}