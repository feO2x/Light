using Light.GuardClauses;

namespace Light.BayesianNetwork
{
    public struct OutcomeProbability
    {
        public readonly double Value;

        private OutcomeProbability(double value)
        {
            Value = value;
        }

        public static OutcomeProbability DefaultMin => new OutcomeProbability(0.0);
        public static OutcomeProbability DefaultMax => new OutcomeProbability(1.0);


        public static OutcomeProbability FromValue(double value)
        {
            value.MustNotBeLessThan(0.0, nameof(value));
            value.MustNotBeGreaterThan(1.0, nameof(value));

            return new OutcomeProbability(value);
        }
    }
}