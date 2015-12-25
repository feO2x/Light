using System;

namespace Light.Serialization.Json
{
    public class JsonWriterTokens
    {
        private string _beginCollectionToken = "[";
        private string _beginComplexObjectToken = "{";
        private string _endCollectionToken = "]";
        private string _endComplexObjectToken = "}";
        private string _null = "null";
        private string _keyValueDelimiter = ":";
        private string _valueDelimiter = ",";

        public string Null
        {
            get { return _null; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _null = value;
            }
        }

        public string BeginCollectionToken
        {
            get { return _beginCollectionToken; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _beginCollectionToken = value;
            }
        }

        public string EndCollectionToken
        {
            get { return _endCollectionToken; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _endCollectionToken = value;
            }
        }

        public string BeginComplexObjectToken
        {
            get { return _beginComplexObjectToken; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _beginComplexObjectToken = value;
            }
        }

        public string EndComplexObjectToken
        {
            get { return _endComplexObjectToken; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _endComplexObjectToken = value;
            }
        }

        public string KeyValueDelimiter
        {
            get { return _keyValueDelimiter; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _keyValueDelimiter = value;
            }
        }

        public string ValueDelimiter
        {
            get { return _valueDelimiter; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _valueDelimiter = value;
            }
        }
    }
}