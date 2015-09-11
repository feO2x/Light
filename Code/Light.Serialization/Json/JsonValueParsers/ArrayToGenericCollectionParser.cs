using System;
using System.Collections.Generic;
using System.Reflection;
using Light.Core;

namespace Light.Serialization.Json.JsonValueParsers
{
    public sealed class ArrayToGenericCollectionParser : IJsonValueParser
    {
        private readonly Type _iEnumerableType = typeof (IEnumerable<>);
        private readonly ICollectionFactory _collectionFactory;
        private readonly MethodInfo _populateGenericCollectionMethodInfo;

        public ArrayToGenericCollectionParser(ICollectionFactory collectionFactory)
        {
            if (collectionFactory == null) throw new ArgumentNullException(nameof(collectionFactory));

            _collectionFactory = collectionFactory;
            _populateGenericCollectionMethodInfo = GetType().GetMethod(nameof(PopulateGenericCollection));
        }

        public bool IsSuitableFor(JsonCharacterBuffer buffer, Type requestedType)
        {
            return buffer.JsonType == JsonType.Array &&
                   requestedType.ImplementsGenericInterface(_iEnumerableType);
        }

        public object ParseValue(JsonDeserializationContext context)
        {
            // I have to create a collection that implements
            var collection = _collectionFactory.CreateCollection(context.RequestedType);
            
            throw new NotImplementedException();
        }

        private void PopulateGenericCollection<T>(ICollection<T> collection, JsonDeserializationContext context)
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