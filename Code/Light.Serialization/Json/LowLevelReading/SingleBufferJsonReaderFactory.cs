using Light.GuardClauses;

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
                value.MustNotBeNull(nameof(value));
                _readerSymbols = value;
            }
        }

        public IJsonReader CreateFromString(string json)
        {
            return new SingleBufferJsonReader(json.ToCharArray(), _readerSymbols);
        }
    }
}