using Light.Core;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Light.Serialization.Json.JsonValueParsers
{
    public sealed class ArrayToGenericCollectionParser : IJsonValueParser
    {
        private readonly Type _iEnumerableType = typeof (IEnumerable<>);
        private readonly ICollectionFactory _collectionFactory;
        private readonly MethodInfo _populateGenericCollectionMethodInfo;
        private readonly object[] _populateGenericCollectionParameters = new object[2];

        public ArrayToGenericCollectionParser(ICollectionFactory collectionFactory)
        {
            if (collectionFactory == null) throw new ArgumentNullException(nameof(collectionFactory));

            _collectionFactory = collectionFactory;
            _populateGenericCollectionMethodInfo = GetType().GetMethod(nameof(PopulateGenericCollection), BindingFlags.Static | BindingFlags.NonPublic);
        }

        public bool IsSuitableFor(JsonCharacterBuffer buffer, Type requestedType)
        {
            return buffer.JsonType == JsonType.Array &&
                   requestedType.ImplementsGenericInterface(_iEnumerableType);
        }

        public object ParseValue(JsonDeserializationContext context)
        {
            var collection = _collectionFactory.CreateCollection(context.RequestedType);

            if (context.JsonReader.CheckEndOfCollection())
                return collection;

            var specificEnumerableType = context.RequestedType.GetSpecificTypeThatCorrespondsToGenericInterface(_iEnumerableType);
            var specificPopulateGenericCollectionMethod = _populateGenericCollectionMethodInfo.MakeGenericMethod(specificEnumerableType.GetGenericArguments());

            _populateGenericCollectionParameters[0] = collection;
            _populateGenericCollectionParameters[1] = context;

            specificPopulateGenericCollectionMethod.Invoke(null, _populateGenericCollectionParameters);
            return collection;
        }

        private static void PopulateGenericCollection<T>(ICollection<T> collection, JsonDeserializationContext context)
        {
            var itemType = typeof (T);
            do
            {
                var nextValue = (T) context.DeserializeChildValue(context.JsonReader, itemType);
                collection.Add(nextValue);
            } while (context.JsonReader.CheckEndOfCollection() == false);
        }
    }
}