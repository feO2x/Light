using Light.Serialization.Json.ComplexTypeDecomposition;
using Light.Serialization.Json.PrimitiveTypeFormatters;
using Light.Serialization.Json.TypeSerializers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Light.Serialization.Json
{
    public class JsonSerializerBuilder
    {
        private readonly IList<IJsonTypeSerializer> _typeSerializers;
        private readonly IJsonWriterFactory _writerFactory;

        public JsonSerializerBuilder()
        {
            var characterEscaper = new DefaultCharacterEscaper();
            var primitiveTypeFormatters = new List<IPrimitiveTypeFormatter>
                                          {
                                              new ToStringPrimitiveTypeFormatter<int>(),
                                              new StringFormatter(characterEscaper),
                                              new DoubleFormatter(),
                                              new ToStringWithQuotationMarksFormatter<Guid>(),
                                              new BooleanFormatter(),
                                              new DecimalFormatter(),
                                              new ToStringPrimitiveTypeFormatter<long>(),
                                              new FloatFormatter(),
                                              new CharFormatter(characterEscaper),
                                              new ToStringPrimitiveTypeFormatter<short>(),
                                              new ToStringPrimitiveTypeFormatter<byte>(),
                                              new ToStringPrimitiveTypeFormatter<uint>(),
                                              new ToStringPrimitiveTypeFormatter<ulong>(),
                                              new ToStringPrimitiveTypeFormatter<ushort>(),
                                              new ToStringPrimitiveTypeFormatter<sbyte>()
                                          };
            var primitiveTypeToFormattersMapping = primitiveTypeFormatters.ToDictionary(f => f.TargetType);

            _typeSerializers = new List<IJsonTypeSerializer>
                               {
                                   new PrimitiveJsonTypeSerializer(primitiveTypeToFormattersMapping),
                                   new EnumerationSerializer(),
                                   new DictionarySerializer(primitiveTypeToFormattersMapping),
                                   new CollectionSerializer(),
                                   new ComplexJsonTypeSerializer(new PublicPropertiesAndFieldsAnalyzer())
                               };

            _writerFactory = new JsonWriterFactory();
        }

        public ISerializer Build()
        {
            return new JsonSerializer(_typeSerializers, _writerFactory);
        }
    }
}