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

        public void Serialize(JsonSerializationContext serializationContext)
        {
            var enumerable = (IEnumerable)serializationContext.@ObjectToBeSerialized;

            var writer = serializationContext.Writer;
            var enumerator = enumerable.GetEnumerator();
            if (enumerator.MoveNext() == false)
            {
                writer.BeginArray();
                writer.EndArray();
                return;
            }

            writer.BeginArray();
            while (true)
            {
                var currentChildObject = enumerator.Current;
                if (currentChildObject == null)
                    writer.WriteNull();
                else
                {
                    var childType = currentChildObject.GetType();
                    serializationContext.SerializeChildObject(currentChildObject, childType, childType);
                }
                if (enumerator.MoveNext())
                    writer.WriteDelimiter();
                else
                    break;
            }
            writer.EndArray();
        }
    }
}
