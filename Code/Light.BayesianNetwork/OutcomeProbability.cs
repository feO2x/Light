using Light.GuardClauses;

namespace Light.BayesianNetwork
{
    public struct OutcomeProbability
    {
        public readonly double Value;
        public readonly OutcomeProbabilityKind Kind;

        private OutcomeProbability(double value, OutcomeProbabilityKind kind)
        {
            Value = value;
            Kind = kind;
        }

        public static OutcomeProbability Default => new OutcomeProbability(0.0, OutcomeProbabilityKind.CalculatedValue);

        public static OutcomeProbability FromValue(double value)
        {
            value.MustNotBeLessThan(0.0, nameof(value));
            value.MustNotBeGreaterThan(1.0, nameof(value));

            return new OutcomeProbability(value, OutcomeProbabilityKind.CalculatedValue);
        }

        public static OutcomeProbability ValueIsEvidence => new OutcomeProbability(1.0, OutcomeProbabilityKind.SelectedEvidence);
        
        public static OutcomeProbability ValueIsNotEvidence => new OutcomeProbability(0.0, OutcomeProbabilityKind.NotSelectedEvidence);
    }
}