using System;
using System.Collections.Generic;
using System.Linq;
using Light.Serialization.Json.PrimitiveTypeFormatters;
using Light.Serialization.Json.TypeSerializers;

namespace Light.Serialization.Json
{
    public class JsonSerializerBuilder
    {
        private readonly IDocumentWriter _documentWriter;
        private readonly IList<ITypeSerializer> _typeSerializers;

        public JsonSerializerBuilder()
        {
            // TODO: the creation of this is lame, I have to change the dependency of IDocumentWriter
            var documentWriter = new JsonDocumentWriter();
            _documentWriter = documentWriter;
            var primitiveTypeFormatters = new List<IPrimitiveTypeFormatter>
                                          {
                                              new ToStringPrimitiveTypeFormatter<int>(),
                                              new BooleanFormatter(),
                                              new ToStringWithQuotationMarksFormatter<Guid>(),
                                              new ToStringPrimitiveTypeFormatter<long>(),
                                              new ToStringPrimitiveTypeFormatter<short>(),
                                              new ToStringPrimitiveTypeFormatter<byte>(),
                                              new ToStringPrimitiveTypeFormatter<uint>(),
                                              new ToStringPrimitiveTypeFormatter<ulong>(),
                                              new ToStringPrimitiveTypeFormatter<ushort>(),
                                              new ToStringPrimitiveTypeFormatter<sbyte>()
                                          };
            _typeSerializers = new List<ITypeSerializer>
                               {
                                   new JsonPrimitiveTypeSerializer(documentWriter, primitiveTypeFormatters.ToDictionary(f => f.TargetType))
                               };
        }

        public ISerializer Build()
        {
            return new Serializer(_documentWriter, _typeSerializers);
        }
    }
}
