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
        public JsonTokenTypeCombination(JsonTokenType jsonTokenType, Type type)
        {
            jsonTokenType.MustNotBeNull(nameof(jsonTokenType));
            type.MustNotBeNull(nameof(type));

            JsonTokenType = jsonTokenType;
            Type = type;
        }

        public JsonTokenType JsonTokenType { get; }
        public Type Type { get; }
    }
}
