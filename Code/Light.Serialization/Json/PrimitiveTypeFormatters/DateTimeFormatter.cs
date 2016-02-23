using System;
using System.Text;
using Light.Serialization.FrameworkExtensions;

namespace Light.Serialization.Json.PrimitiveTypeFormatters
{
    public sealed class DateTimeFormatter : BasePrimitiveTypeFormatter<DateTime>, IPrimitiveTypeFormatter
    {
        private readonly StringBuilder _stringBuilder = new StringBuilder(23);

        private TimeZoneInfo _timeZoneInfo = TimeZoneInfo.Local;

        public DateTimeFormatter() : base(false)
        {
            
        }

        public TimeZoneInfo TimeZoneInfo
        {
            get { return _timeZoneInfo;}
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _timeZoneInfo = value;
            }
        }

        public string FormatPrimitiveType(object @object)
        {
            var dateTime = (DateTime) @object;

            // Append the date in any case
            _stringBuilder.AppendFormat("\"{0:D4}-{1:D2}-{2:D2}", dateTime.Year, dateTime.Month, dateTime.Day);

            bool isHourZero = dateTime.Hour == 0,
                 isMinuteZero = dateTime.Minute == 0,
                 isSecondZero = dateTime.Second == 0,
                 isMillisecondZero = dateTime.Millisecond == 0;

            // Omit the time part if possible
            if (isHourZero && isMinuteZero && isSecondZero && isMillisecondZero)
                return _stringBuilder.CompleteJsonStringWithQuotationMarkAndClearBuilder();

            // Otherwise append hour and minute at least
            _stringBuilder.Append('T');
            _stringBuilder.AppendFormat("{0:D2}:{1:D2}", dateTime.Hour, dateTime.Minute);

            // Append second and milliseconds if necessary
            if (isSecondZero == false || isMillisecondZero == false)
            {
                _stringBuilder.Append(':');
                _stringBuilder.Append(dateTime.Second.ToString("D2"));
                if (isMillisecondZero == false)
                {
                    _stringBuilder.Append('.');
                    _stringBuilder.Append(dateTime.Millisecond.ToString("D3"));
                }
            }

            // Append the offset
            switch (dateTime.Kind)
            {
                // UTC just has a Z appended
                case DateTimeKind.Utc:
                    _stringBuilder.Append('Z');
                    break;
                // For local DateTimes, the offset is appended
                case DateTimeKind.Local:
                    var offset = TimeZoneInfo.Local.GetUtcOffset(dateTime);
                    if (offset.Hours >= 0)
                        _stringBuilder.Append('+');
                    _stringBuilder.Append(offset.Hours.ToString("D2"));
                    if (offset.Minutes != 0)
                    {
                        _stringBuilder.Append(':');
                        _stringBuilder.Append(offset.Minutes.ToString("D2"));
                    }
                    break;
                // DateTimes with kind "Unspecified" have no extension
                case DateTimeKind.Unspecified:
                    break;
            }
            
            return _stringBuilder.CompleteJsonStringWithQuotationMarkAndClearBuilder();
        }
    }
}