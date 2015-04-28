﻿using Light.Serialization.Json.PrimitiveTypeFormatters;
using Light.Serialization.Json.TypeSerializers;
using System;
using System.Collections.Generic;
using System.Linq;

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
                                              new DoubleFormatter(),
                                              new ToStringWithQuotationMarksFormatter<Guid>(),
                                              new BooleanFormatter(),
                                              new DecimalFormatter(),
                                              new ToStringPrimitiveTypeFormatter<long>(),
                                              new FloatFormatter(),
                                              new ToStringWithQuotationMarksFormatter<char>(),
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
