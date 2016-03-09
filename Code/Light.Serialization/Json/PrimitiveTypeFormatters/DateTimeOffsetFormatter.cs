using System;
using System.Text;
using Light.Serialization.FrameworkExtensions;

namespace Light.Serialization.Json.PrimitiveTypeFormatters
{
    public sealed class DateTimeOffsetFormatter : BasePrimitiveTypeFormatter<DateTimeOffset>, IPrimitiveTypeFormatter
    {
        public DateTimeOffsetFormatter() : base(false) { }

        public string FormatPrimitiveType(object @object)
        {
            var dateTimeOffset = (DateTimeOffset) @object;

            // Append the date in any case
            var stringBuilder = new StringBuilder(23);
            stringBuilder.AppendFormat("\"{0:D4}-{1:D2}-{2:D2}", dateTimeOffset.Year, dateTimeOffset.Month, dateTimeOffset.Day);

            bool isHourZero = dateTimeOffset.Hour == 0,
                 isMinuteZero = dateTimeOffset.Minute == 0,
                 isSecondZero = dateTimeOffset.Second == 0,
                 isMillisecondZero = dateTimeOffset.Millisecond == 0,
                 isOffsetZero = dateTimeOffset.Offset == TimeSpan.Zero;

            // Omit the time part if possible
            if (isHourZero && isMinuteZero && isSecondZero && isMillisecondZero && isOffsetZero)
                return stringBuilder.CompleteJsonStringWithQuotationMark();

            // Otherwise append hour and minute at least
            stringBuilder.Append('T');
            stringBuilder.AppendFormat("{0:D2}:{1:D2}", dateTimeOffset.Hour, dateTimeOffset.Minute);

            // Append second and milliseconds if necessary
            if (isSecondZero == false || isMillisecondZero == false)
            {
                stringBuilder.Append(':');
                stringBuilder.Append(dateTimeOffset.Second.ToString("D2"));
                if (isMillisecondZero == false)
                {
                    stringBuilder.Append('.');
                    stringBuilder.Append(dateTimeOffset.Millisecond.ToString("D3"));
                }
            }

            // Append the offset
            if (isOffsetZero)
                stringBuilder.Append('Z');
            else
            {
                var offset = dateTimeOffset.Offset;
                if (offset.Hours >= 0)
                    stringBuilder.Append('+');
                stringBuilder.Append(offset.Hours.ToString("D2"));
                if (offset.Minutes != 0)
                {
                    stringBuilder.Append(':');
                    stringBuilder.Append(Math.Abs(offset.Minutes).ToString("D2"));
                }
            }

            return stringBuilder.CompleteJsonStringWithQuotationMark();
        }
    }
}