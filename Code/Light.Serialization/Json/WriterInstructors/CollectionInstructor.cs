using System;
using System.Collections;

namespace Light.Serialization.Json.WriterInstructors
{
    public sealed class CollectionInstructor : IJsonWriterInstructor
    {
        public bool AppliesToObject(object @object, Type actualType, Type referencedType)
        {
            return @object is IEnumerable;
        }

        public bool Serialize(JsonSerializationContext serializationContext)
        {
            var enumerable = (IEnumerable)serializationContext.@ObjectToBeSerialized;
            var decreaseIndentAfterSerialization = true;
            var writer = serializationContext.Writer;
            var enumerator = enumerable.GetEnumerator();
            if (enumerator.MoveNext() == false)
            {
                writer.BeginArray();
                writer.EndArray();
                return decreaseIndentAfterSerialization;
            }

            writer.BeginArray();
            var collectionIndex = 0;
            while (true)
            {
                var firstObjectInCollection = false;
                var currentChildObject = enumerator.Current;
                if (currentChildObject == null)
                    writer.WriteNull();
                else
                {
                    var childType = currentChildObject.GetType();

                    if (collectionIndex == 0)
                        firstObjectInCollection = true;
                    serializationContext.SerializeChildObject(currentChildObject, childType, childType, firstObjectInCollection);
                }
                if (enumerator.MoveNext())
                    writer.WriteDelimiter();
                else
                    break;
                collectionIndex++;
            }
            writer.EndArray();

            return decreaseIndentAfterSerialization;
        }
    }
}
