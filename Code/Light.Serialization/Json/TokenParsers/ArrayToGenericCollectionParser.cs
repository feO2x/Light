using System;
using System.Collections.Generic;
using System.Reflection;
using Light.GuardClauses;
using Light.Serialization.FrameworkExtensions;
using Light.Serialization.Json.ComplexTypeConstruction;

namespace Light.Serialization.Json.TokenParsers
{
    public sealed class ArrayToGenericCollectionParser : IJsonTokenParser
    {
        private readonly ICollectionFactory _collectionFactory;
        private readonly MethodInfo _populateGenericCollectionMethodInfo;
        private readonly object[] _methodParameters = new object[3];

        public ArrayToGenericCollectionParser(ICollectionFactory collectionFactory)
        {
            collectionFactory.MustNotBeNull(nameof(collectionFactory));

            _collectionFactory = collectionFactory;
            _populateGenericCollectionMethodInfo = GetType().GetTypeInfo().GetDeclaredMethod(nameof(PopulateGenericCollection));
        }

        public bool CanBeCached => false;

        public bool IsSuitableFor(JsonToken token, Type requestedType)
        {
            return token.JsonType == JsonTokenType.BeginOfArray &&
                   requestedType.GetTypeInfo().ImplementsGenericInterface(typeof(IEnumerable<>).GetTypeInfo());
        }

        public object ParseValue(JsonDeserializationContext context)
        {
            var collection = _collectionFactory.CreateCollection(context.RequestedType);

            var firstCollectionToken = context.JsonReader.ReadNextToken();

            if (firstCollectionToken.JsonType == JsonTokenType.EndOfArray)
                return collection;

            var specificEnumerableType = context.RequestedType.GetTypeInfo().GetSpecificTypeInfoThatCorrespondsToGenericInterface(typeof(IEnumerable<>).GetTypeInfo());
            var specificPopulateGenericCollectionMethod = _populateGenericCollectionMethodInfo.MakeGenericMethod(specificEnumerableType.GenericTypeArguments);

            _methodParameters[0] = firstCollectionToken;
            _methodParameters[1] = collection;
            _methodParameters[2] = context;

            specificPopulateGenericCollectionMethod.Invoke(null, _methodParameters);

            ClearObjectArray();

            return collection;
        }

        private void ClearObjectArray()
        {
            _methodParameters[0] = _methodParameters[1] = _methodParameters[2] = null;
        }

        private static void PopulateGenericCollection<T>(JsonToken nextToken, ICollection<T> collection, JsonDeserializationContext context)
        {
            while (true)
            {
                nextToken.ExpectBeginOfValue();
                var nextValue = context.DeserializeToken<T>(nextToken);
                collection.Add(nextValue);

                nextToken = context.JsonReader.ReadNextToken();
                switch (nextToken.JsonType)
                {
                    case JsonTokenType.ValueDelimiter:
                        nextToken = context.JsonReader.ReadNextToken();
                        continue;
                    case JsonTokenType.EndOfObject:
                        nextToken = context.JsonReader.ReadNextToken();
                        if (nextToken.JsonType == JsonTokenType.ValueDelimiter)
                            continue;
                        return; //expect end of array
                    case JsonTokenType.EndOfArray:
                        return;
                    default:
                        throw new JsonDocumentException($"Expected value delimiter or end of array in JSON document, but found {nextToken}.", nextToken);
                }
            }
        }
    }
}