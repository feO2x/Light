using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Light.GuardClauses;

namespace Light.Serialization.Json.Caching
{
    public struct JsonTokenTypeCombination
    {
        public JsonTokenTypeCombination(JsonToken jsonToken, Type type)
        {
            jsonToken.MustNotBeNull(nameof(jsonToken));
            type.MustNotBeNull(nameof(type));

            JsonToken = jsonToken;
            Type = type;
        }

        public JsonToken JsonToken { get; }
        public Type Type { get; }
    }
}
