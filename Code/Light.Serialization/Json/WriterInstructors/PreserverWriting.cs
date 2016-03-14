using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Light.Serialization.Json.WriterInstructors
{
    public class PreserverWriting : IPreserverWriting
    {
        private static string _referenceKey = "$ref";
        private static string _identifierKey = "$id";

        public void WriteReferenceKey(JsonSerializationContext context, string id)
        {
            var writer = context.Writer;

            writer.WriteKey(_referenceKey);
            writer.WritePrimitiveValue(id);
            writer.EndObject(); //todo: check if needed; what happens if json human readability set?
        }

        public void WriteIdentifierKey(JsonSerializationContext context, string id)
        {
            var writer = context.Writer;

            writer.WriteKey(_identifierKey);
            writer.WritePrimitiveValue(id);
            writer.WriteDelimiter();
        }
    }
}
