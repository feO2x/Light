using System.Collections.Generic;
using Light.Serialization.Json.ComplexTypeDecomposition;
using Light.Serialization.Json.SerializationRules;

namespace Light.Serialization.Json.ObjectReferencePreservation
{
    public sealed class PreservedObjectsRuleBuilder : IRuleBuilder
    {
        public Rule<T> CreateRule<T>(IReadableValuesTypeAnalyzer typeAnalyzer)
        {
            return new RuleObjectPreserverDecorator<T>(typeAnalyzer,
                new ObjectSerializationReferencePreserver(
                    new Dictionary<object, uint>()));
        }
    }
}