using System;
using System.Collections.Generic;
using Light.Serialization.Json.ComplexTypeDecomposition;
using Light.Serialization.Json.PrimitiveTypeFormatters;
using Light.Serialization.Json.WriterInstructors;

namespace Light.Serialization.Json
{
    public static class JsonSerializerBuildExtensions
    {
        public static IList<IPrimitiveTypeFormatter> AddDefaultPrimitiveTypeFormatters(this IList<IPrimitiveTypeFormatter> targetList, ICharacterEscaper characterEscaper)
        {
            if (targetList == null) throw new ArgumentNullException(nameof(targetList));

            targetList.Add(new ToStringPrimitiveTypeFormatter<int>());
            targetList.Add(new StringFormatter(characterEscaper));
            targetList.Add(new DoubleFormatter());
            targetList.Add(new DateTimeFormatter());
            targetList.Add(new ToStringWithQuotationMarksFormatter<Guid>());
            targetList.Add(new BooleanFormatter());
            targetList.Add(new DecimalFormatter());
            targetList.Add(new ToStringPrimitiveTypeFormatter<long>());
            targetList.Add(new FloatFormatter());
            targetList.Add(new CharFormatter(characterEscaper));
            targetList.Add(new ToStringPrimitiveTypeFormatter<short>());
            targetList.Add(new ToStringPrimitiveTypeFormatter<byte>());
            targetList.Add(new ToStringPrimitiveTypeFormatter<uint>());
            targetList.Add(new ToStringPrimitiveTypeFormatter<ulong>());
            targetList.Add(new ToStringPrimitiveTypeFormatter<ushort>());
            targetList.Add(new ToStringPrimitiveTypeFormatter<sbyte>());

            return targetList;
        }

        public static IList<IJsonWriterInstructor> AddDefaultWriterInstructors(this IList<IJsonWriterInstructor> targetList,
                                                                               IDictionary<Type, IPrimitiveTypeFormatter> primitiveTypeToFormattersMapping,
                                                                               IReadableValuesTypeAnalyzer readableValuesTypeAnalyzer)
        {
            if (targetList == null) throw new ArgumentNullException(nameof(targetList));

            targetList.Add(new PrimitiveWriterInstructor(primitiveTypeToFormattersMapping));
            targetList.Add(new EnumerationToStringInstructor());
            targetList.Add(new DictionaryInstructor(primitiveTypeToFormattersMapping));
            targetList.Add(new CollectionInstructor());
            targetList.Add(new ComplexObjectInstructor(readableValuesTypeAnalyzer));

            return targetList;
        }
    }
}