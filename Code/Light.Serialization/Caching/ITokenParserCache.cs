using System;
using Light.Serialization.Json;
using Light.Serialization.Json.Caching;

namespace Light.Serialization.Caching
{
    public interface ITokenParserCache
    {
        bool CheckTokenTypeCombinationForBlacklist(JsonTokenTypeCombination jsonTokenTypeCombination);
        bool CheckTokenTypeForBlacklist(JsonToken jsonToken, Type type);
        bool TryAddTokenParserToCache(JsonTokenTypeCombination jsonTokenTypeCombination, IJsonTokenParser jsonTokenParser);
        bool TryGetTokenParser(JsonTokenTypeCombination jsonTokenTypeCombination, out IJsonTokenParser jsonTokenParser);
        bool TryGetTokenParser(JsonToken jsonToken, Type type, out IJsonTokenParser jsonTokenParser);
    }
}