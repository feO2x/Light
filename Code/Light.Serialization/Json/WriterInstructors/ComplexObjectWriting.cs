using Light.Serialization.Json.ComplexTypeDecomposition;
using System.Collections.Generic;
using Light.GuardClauses;

namespace Light.Serialization.Json.WriterInstructors
{
    public class ComplexObjectWriting
    {
        public static void WriteValues(JsonSerializationContext context, IList<IValueProvider> valueProvidersForComplexObject)
        {
            valueProvidersForComplexObject.MustNotBeNull(nameof(valueProvidersForComplexObject));

            var writer = context.Writer;
            writer.BeginObject();
            for (var i = 0; i < valueProvidersForComplexObject.Count; i++)
            {
                var firstElementInComplexObject = false;
                var valueProvider = valueProvidersForComplexObject[i];
                var childValue = valueProvider.GetValue(context.ObjectToBeSerialized);

                writer.WriteKey(valueProvider.Name);
                if (childValue == null)
                    writer.WriteNull();
                else
                {
                    // TODO: This might end up in an endless loop if e.g. a property returns a reference to the object itself. Can be solved with a dictionary that contains all objects being serialized (what I wanted to integrate in the first place).
                    var childValueType = childValue.GetType();
                    if (i == 0)
                        firstElementInComplexObject = true;
                    context.SerializeChildObject(childValue, childValueType, valueProvider.ReferenceType, firstElementInComplexObject);
                }

                if (i < valueProvidersForComplexObject.Count - 1)
                    writer.WriteDelimiter();
            }
            writer.EndObject();
        }
    }
}
