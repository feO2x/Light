using Light.GuardClauses;

namespace Light.Serialization.Json.LowLevelWriting
{
    public class JsonWriterSymbols
    {
        public char BeginOfArray = JsonSymbols.BeginOfArray;
        public char BeginOfObject = JsonSymbols.BeginOfObject;
        public char EndOfArray = JsonSymbols.EndOfArray;
        public char EndOfObject = JsonSymbols.EndOfObject;
        private string _null = JsonSymbols.Null;
        public char PairDelimiter = JsonSymbols.PairDelimiter;
        public char ValueDelimiter = JsonSymbols.ValueDelimiter;

        public string Null
        {
            get { return _null; }
            set
            {
                value.MustNotBeNull(nameof(value));
                _null = value;
            }
        }
    }
}