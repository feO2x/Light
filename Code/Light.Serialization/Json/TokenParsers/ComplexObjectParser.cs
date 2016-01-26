using System;
using System.Collections.Generic;

namespace Light.Serialization.Json.TokenParsers
{
    public sealed class ComplexObjectParser : IJsonTokenParser
    {
        private readonly IObjectFactory _objectFactory;
        private readonly Type _objectType = typeof (object);

        public ComplexObjectParser(IObjectFactory objectFactory)
        {
            if (objectFactory == null) throw new ArgumentNullException(nameof(objectFactory));

            _objectFactory = objectFactory;
        }

        public bool IsSuitableFor(JsonToken token, Type requestedType)
        {
            return token.JsonType == JsonTokenType.BeginOfObject;
        }

        public object ParseValue(JsonDeserializationContext context)
        {
            var deserializedChildValues = new Dictionary<string, object>();

            while (true)
            {
                var stringOrEndOfObjectToken = context.JsonReader.ReadNextToken();

                if (stringOrEndOfObjectToken.JsonType == JsonTokenType.EndOfObject)
                    break;

                if (stringOrEndOfObjectToken.JsonType != JsonTokenType.String)
                    throw new JsonDocumentException($"Expected JSON string or end of complex JSON object, but found {stringOrEndOfObjectToken}", stringOrEndOfObjectToken);

                var pairDelimiterToken = context.JsonReader.ReadNextToken();
                if (pairDelimiterToken.JsonType != JsonTokenType.PairDelimiter)
                    throw new JsonDocumentException($"Expected delimiter between label and value in complex JSON object, but found {pairDelimiterToken}", pairDelimiterToken);

                var valueToken = context.JsonReader.ReadNextToken();
                var value = context.DeserializeToken(valueToken, _objectType);

                deserializedChildValues.Add(stringOrEndOfObjectToken.ToString(), value);

                var valueDelimiterOrEndOfObjectToken = context.JsonReader.ReadNextToken();
                if (valueDelimiterOrEndOfObjectToken.JsonType == JsonTokenType.EndOfObject)
                    break;

                if (valueDelimiterOrEndOfObjectToken.JsonType != JsonTokenType.ValueDelimiter)
                    throw new JsonDocumentException($"Expected value delimiter or end of complex JSON object, but found {valueDelimiterOrEndOfObjectToken}", valueDelimiterOrEndOfObjectToken);
            }

            return _objectFactory.Create(context.RequestedType, deserializedChildValues);
        }
    }
}