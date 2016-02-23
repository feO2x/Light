using System;
using System.Text;
using Light.Serialization.FrameworkExtensions;

namespace Light.Serialization.Json.PrimitiveTypeFormatters
{
    public sealed class TimeSpanFormatter : BasePrimitiveTypeFormatter<TimeSpan>, IPrimitiveTypeFormatter
    {
        private readonly StringBuilder _stringBuilder = new StringBuilder();

        public TimeSpanFormatter() : base(false)
        {
        }

        public string FormatPrimitiveType(object @object)
        {
            var timeSpan = (TimeSpan) @object;

            _stringBuilder.Append('"');
            _stringBuilder.Append('P');

            bool isDaysZero = timeSpan.Days == 0,
                 isHoursZero = timeSpan.Hours == 0,
                 isMinutesZero = timeSpan.Minutes == 0,
                 isSecondsZero = timeSpan.Seconds == 0,
                 isMillisecondsZero = timeSpan.Milliseconds == 0;

            // Check if days should be serialized
            if (isDaysZero == false ||
                isHoursZero && isMinutesZero && isSecondsZero && isMillisecondsZero)
            {
                AppendDurationSection('D', timeSpan.Days);
                if (isHoursZero && isMinutesZero && isSecondsZero && isMillisecondsZero)
                    return _stringBuilder.CompleteJsonStringWithQuotationMarkAndClearBuilder();
            }

            // Append identifier for time part of the duration
            _stringBuilder.Append('T');

            // Append hours and minutes if necessary
            AppendTimespanSectionIfNecessary('H', timeSpan.Hours);
            AppendTimespanSectionIfNecessary('M', timeSpan.Minutes);

            // Append seconds and milliseconds if necessary
            if (isSecondsZero == false || isMillisecondsZero == false)
            {
                _stringBuilder.Append(timeSpan.Seconds);
                if (isMillisecondsZero == false)
                {
                    _stringBuilder.Append('.');
                    _stringBuilder.Append(timeSpan.Milliseconds.ToString("D3"));
                }
                _stringBuilder.Append('S');
            }
            return _stringBuilder.CompleteJsonStringWithQuotationMarkAndClearBuilder();
        }

        private void AppendTimespanSectionIfNecessary(char identifier, int duration)
        {
            if (duration == 0)
                return;

            AppendDurationSection(identifier, duration);
        }

        private void AppendDurationSection(char identifier, int duration)
        {
            _stringBuilder.Append(duration);
            _stringBuilder.Append(identifier);
        }
    }
}