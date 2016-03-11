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
        private readonly TypeInfo _iEnumerableTypeInfo = typeof (IEnumerable<>).GetTypeInfo();
        private readonly MethodInfo _populateGenericCollectionMethodInfo;
        private readonly object[] _populateGenericCollectionParameters = new object[3];

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
                   requestedType.GetTypeInfo().ImplementsGenericInterface(_iEnumerableTypeInfo);
        }

        public object ParseValue(JsonDeserializationContext context)
        {
            var collection = _collectionFactory.CreateCollection(context.RequestedType);

            var firstCollectionToken = context.JsonReader.ReadNextToken();

            if (firstCollectionToken.JsonType == JsonTokenType.EndOfArray)
                return collection;

            var specificEnumerableType = context.RequestedType.GetTypeInfo().GetSpecificTypeInfoThatCorrespondsToGenericInterface(_iEnumerableTypeInfo);
            var specificPopulateGenericCollectionMethod = _populateGenericCollectionMethodInfo.MakeGenericMethod(specificEnumerableType.GenericTypeArguments);

            _populateGenericCollectionParameters[0] = firstCollectionToken;
            _populateGenericCollectionParameters[1] = collection;
            _populateGenericCollectionParameters[2] = context;

            specificPopulateGenericCollectionMethod.Invoke(null, _populateGenericCollectionParameters);
            return collection;
        }

        private static void PopulateGenericCollection<T>(JsonToken nextToken, ICollection<T> collection, JsonDeserializationContext context)
        {
            var itemType = typeof (T);

            while (true)
            {
                var nextValue = (T) context.DeserializeToken(nextToken, itemType);
                collection.Add(nextValue);

                nextToken = context.JsonReader.ReadNextToken();
                switch (nextToken.JsonType)
                {
                    case JsonTokenType.ValueDelimiter:
                        nextToken = context.JsonReader.ReadNextToken();
                        continue;
                    case JsonTokenType.EndOfArray:
                        return;
                    default:
                        throw new JsonDocumentException($"Expected value delimiter or end of array in JSON document, but found {nextToken}", nextToken);
                }
            }
        }
    }
}