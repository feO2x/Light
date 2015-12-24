using System;
using System.Collections.Generic;
using System.Linq;
using Light.Serialization.Json.ComplexTypeDecomposition;
using Light.Serialization.Json.PrimitiveTypeFormatters;
using Light.Serialization.Json.WriterInstructors;

namespace Light.Serialization.Json
{
    public class JsonSerializerBuilder
    {
        private readonly IList<IJsonWriterInstructor> _typeSerializers;
        private readonly IJsonWriterFactory _writerFactory;

        public JsonSerializerBuilder()
        {
            var characterEscaper = new DefaultCharacterEscaper();
            var primitiveTypeToFormattersMapping = new IPrimitiveTypeFormatter[]
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
                                                   }.ToDictionary(f => f.TargetType);

            _typeSerializers = new List<IJsonWriterInstructor>
                               {
                                   new PrimitiveWriterInstructor(primitiveTypeToFormattersMapping),
                                   new EnumerationToStringInstructor(),
                                   new DictionaryInstructor(primitiveTypeToFormattersMapping),
                                   new CollectionInstructor(),
                                   new ComplexWriterInstructor(new PublicPropertiesAndFieldsAnalyzer())
                               };

            _writerFactory = new JsonWriterFactory();
        }

        public ISerializer Build()
        {
            return new JsonSerializer(_typeSerializers, _writerFactory);
        }
    }
}