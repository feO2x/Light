using Light.GuardClauses;
using Light.Serialization.FrameworkExtensions;

namespace Light.Serialization.Json.ComplexTypeDecomposition
{
    public sealed class ToLowerWithoutSpecialCharactersNormalizer : IInjectableValueNameNormalizer
    {
        public string Normalize(string name)
        {
            return name.ToLowerAndRemoveAllSpecialCharacters();
        }
    }
}