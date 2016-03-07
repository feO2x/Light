using System;
using System.Text;
using Light.Serialization.FrameworkExtensions;

namespace Light.Serialization.Json.PrimitiveTypeFormatters
{
    public sealed class TimeSpanFormatter : BasePrimitiveTypeFormatter<TimeSpan>, IPrimitiveTypeFormatter
    {
        public TimeSpanFormatter() : base(false) { }

        public string FormatPrimitiveType(object @object)
        {
            var timeSpan = (TimeSpan) @object;

            var stringBuilder = new StringBuilder();
            stringBuilder.Append('"');
            stringBuilder.Append('P');

            bool isDaysZero = timeSpan.Days == 0,
                 isHoursZero = timeSpan.Hours == 0,
                 isMinutesZero = timeSpan.Minutes == 0,
                 isSecondsZero = timeSpan.Seconds == 0,
                 isMillisecondsZero = timeSpan.Milliseconds == 0;

            // Check if days should be serialized
            if (isDaysZero == false ||
                isHoursZero && isMinutesZero && isSecondsZero && isMillisecondsZero)
            {
                AppendDurationSection(stringBuilder, 'D', timeSpan.Days);
                if (isHoursZero && isMinutesZero && isSecondsZero && isMillisecondsZero)
                    return stringBuilder.CompleteJsonStringWithQuotationMark();
            }

            // Append identifier for time part of the duration
            stringBuilder.Append('T');

            // Append hours and minutes if necessary
            AppendTimespanSectionIfNecessary(stringBuilder, 'H', timeSpan.Hours);
            AppendTimespanSectionIfNecessary(stringBuilder, 'M', timeSpan.Minutes);

            // Append seconds and milliseconds if necessary
            if (isSecondsZero == false || isMillisecondsZero == false)
            {
                stringBuilder.Append(timeSpan.Seconds);
                if (isMillisecondsZero == false)
                {
                    stringBuilder.Append('.');
                    stringBuilder.Append(timeSpan.Milliseconds.ToString("D3"));
                }
                stringBuilder.Append('S');
            }
            return stringBuilder.CompleteJsonStringWithQuotationMark();
        }

        private static void AppendTimespanSectionIfNecessary(StringBuilder stringBuilder, char identifier, int duration)
        {
            if (duration == 0)
                return;

            AppendDurationSection(stringBuilder, identifier, duration);
        }

        private static void AppendDurationSection(StringBuilder stringBuilder, char identifier, int duration)
        {
            stringBuilder.Append(duration);
            stringBuilder.Append(identifier);
        }
    }
}