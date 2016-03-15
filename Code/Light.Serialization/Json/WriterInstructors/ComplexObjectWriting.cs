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

            context.Writer.BeginObject();

            WriteInnerValues(context, valueProvidersForComplexObject);

            context.Writer.EndObject();
        }

        public static void WriteInnerValues(JsonSerializationContext context, IList<IValueProvider> valueProvidersForComplexObject)
        {
            for (var i = 0; i < valueProvidersForComplexObject.Count; i++)
            {
                var valueProvider = valueProvidersForComplexObject[i];
                var childValue = valueProvider.GetValue(context.ObjectToBeSerialized);

                context.Writer.WriteKey(valueProvider.Name); //todo: normalize or not?
                if (childValue == null)
                    context.Writer.WriteNull();
                else
                {
                    // TODO: This might end up in an endless loop if e.g. a property returns a reference to the object itself. Can be solved with a dictionary that contains all objects being serialized (what I wanted to integrate in the first place).
                    var childValueType = childValue.GetType();
                    context.SerializeChildObject(childValue, childValueType, valueProvider.ReferenceType);
                }

                if (i < valueProvidersForComplexObject.Count - 1)
                    context.Writer.WriteDelimiter();
            }
        }
    }
}
