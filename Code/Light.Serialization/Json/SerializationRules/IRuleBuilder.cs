using Light.Serialization.Json.ComplexTypeDecomposition;

namespace Light.Serialization.Json.SerializationRules
{
    public interface IRuleBuilder
    {
        Rule<T> CreateRule<T>(IReadableValuesTypeAnalyzer typeAnalyzer);
    }
}