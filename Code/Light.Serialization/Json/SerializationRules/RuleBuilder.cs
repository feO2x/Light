using Light.Serialization.Json.ComplexTypeDecomposition;

namespace Light.Serialization.Json.SerializationRules
{
    public sealed class RuleBuilder : IRuleBuilder
    {
        public Rule<T> CreateRule<T>(IReadableValuesTypeAnalyzer typeAnalyzer)
        {
            return new Rule<T>(typeAnalyzer);
        }
    }
}