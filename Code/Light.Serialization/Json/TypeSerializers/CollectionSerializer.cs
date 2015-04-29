using System;
using System.Collections;

namespace Light.Serialization.Json.TypeSerializers
{
    public sealed class CollectionSerializer : ITypeSerializer
    {
        private readonly IJsonWriter _writer;

        public CollectionSerializer(IJsonWriter writer)
        {
            if (writer == null) throw new ArgumentNullException("writer");

            _writer = writer;
        }

        public bool AppliesToObject(object @object, Type actualType, Type referencedType)
        {
            return @object is IEnumerable;
        }

        public void Serialize(object @object, Type actualType, Type referencedType, Action<object, Type, Type> serializeChildObject)
        {
            var enumerable = (IEnumerable) @object;

            var enumerator = enumerable.GetEnumerator();
            if (enumerator.MoveNext() == false)
                throw new NotImplementedException("What should happen if the collection is empty?");


            _writer.BeginCollection();
            while (true)
            {
                var currentChildObject = enumerator.Current;
                if (currentChildObject == null)
                    _writer.WriteNull();
                else
                {
                    var childType = currentChildObject.GetType();
                    serializeChildObject(currentChildObject, childType, childType);
                }
                if (enumerator.MoveNext())
                    _writer.WriteDelimiter();
                else
                    break;
            }
            _writer.EndCollection();
        }
    }
}
