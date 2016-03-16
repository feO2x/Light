using System;
using System.Collections.Generic;
using Light.GuardClauses;
using Light.Serialization.Json.ComplexTypeDecomposition;
using Light.Serialization.Json.ObjectReferencePreservation;
using Light.Serialization.Json.PrimitiveTypeFormatters;
using Light.Serialization.Json.WriterInstructors;

namespace Light.Serialization.Json
{
    public static class JsonSerializerBuildExtensions
    {
        public static IList<IPrimitiveTypeFormatter> AddDefaultPrimitiveTypeFormatters(this IList<IPrimitiveTypeFormatter> targetList, ICharacterEscaper characterEscaper)
        {
            targetList.MustNotBeNull(nameof(targetList));

            targetList.Add(new ToStringPrimitiveTypeFormatter<int>(false));
            targetList.Add(new StringFormatter(characterEscaper));
            targetList.Add(new DoubleFormatter());
            targetList.Add(new DateTimeFormatter());
            targetList.Add(new DateTimeOffsetFormatter());
            targetList.Add(new TimeSpanFormatter());
            targetList.Add(new ToStringWithQuotationMarksFormatter<Guid>(false));
            targetList.Add(new BooleanFormatter());
            targetList.Add(new DecimalFormatter());
            targetList.Add(new ToStringPrimitiveTypeFormatter<long>(false));
            targetList.Add(new FloatFormatter());
            targetList.Add(new CharFormatter(characterEscaper));
            targetList.Add(new ToStringPrimitiveTypeFormatter<short>(false));
            targetList.Add(new ToStringPrimitiveTypeFormatter<byte>(false));
            targetList.Add(new ToStringPrimitiveTypeFormatter<uint>(false));
            targetList.Add(new ToStringPrimitiveTypeFormatter<ulong>(false));
            targetList.Add(new ToStringPrimitiveTypeFormatter<ushort>(false));
            targetList.Add(new ToStringPrimitiveTypeFormatter<sbyte>(false));

            return targetList;
        }

        public static IList<IJsonWriterInstructor> AddDefaultWriterInstructors(this IList<IJsonWriterInstructor> targetList,
                                                                               IDictionary<Type, IPrimitiveTypeFormatter> primitiveTypeToFormattersMapping,
                                                                               IReadableValuesTypeAnalyzer readableValuesTypeAnalyzer)
        {
            targetList.MustNotBeNull(nameof(targetList));

            var preserver = new ObjectReferencePreserver(new Dictionary<object, uint>());

            targetList.Add(new PrimitiveWriterInstructor(primitiveTypeToFormattersMapping));
            targetList.Add(new EnumerationToStringInstructor());
            targetList.Add(new PreserveObjectReferencesDecorator(
                new DictionaryInstructor(primitiveTypeToFormattersMapping),
                preserver));
            targetList.Add(new CollectionInstructor());
            targetList.Add(new PreserveObjectReferencesDecorator(
                new ComplexObjectInstructor(readableValuesTypeAnalyzer),
                preserver));

            return targetList;
        }
    }
}