using System;

namespace Light.Serialization.Json.LowLevelWriting
{
    public class JsonWriterSymbols
    {
        public char BeginOfArray = DefaultJsonSymbols.BeginOfArray;
        public char BeginOfObject = DefaultJsonSymbols.BeginOfObject;
        public char EndOfArray = DefaultJsonSymbols.EndOfArray;
        public char EndOfObject = DefaultJsonSymbols.EndOfObject;
        private string _null = DefaultJsonSymbols.Null;
        public char PairDelimiter = DefaultJsonSymbols.PairDelimiter;
        public char ValueDelimiter = DefaultJsonSymbols.ValueDelimiter;

        public string Null
        {
            get { return _null; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _null = value;
            }
        }
    }
}