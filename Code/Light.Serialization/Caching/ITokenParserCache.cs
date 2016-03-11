using System;
using Light.Serialization.Json;

namespace Light.Serialization.Caching
{
    public interface ITokenParserCache
    {
        void AddTokenParser(JsonToken jsonToken, Type type, IJsonTokenParser jsonTokenParser);
        bool TryGetTokenParser(JsonToken jsonToken, Type type, out IJsonTokenParser jsonTokenParser);
    }
}