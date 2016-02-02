using System;

namespace Light.Serialization.Json.LowLevelReading
{
    public sealed class SingleBufferJsonReaderFactory : IJsonReaderFactory
    {
        private JsonReaderSymbols _readerSymbols = new JsonReaderSymbols();

        public JsonReaderSymbols ReaderSymbols
        {
            get { return _readerSymbols; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _readerSymbols = value;
            }
        }

        public IJsonReader CreateFromString(string json)
        {
            return new SingleBufferJsonReader(json.ToCharArray(), _readerSymbols);
        }
    }
}