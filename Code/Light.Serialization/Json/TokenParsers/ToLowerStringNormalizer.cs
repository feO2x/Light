namespace Light.Serialization.Json.TokenParsers
{
    public sealed class ToLowerStringNormalizer : IInjectableValueNameNormalizer
    {
        public string Normalize(string name)
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var character in name)
            {
                if (char.IsLower(character))
                    continue;

                return name.ToLower();
            }

            return name;
        }
    }
}