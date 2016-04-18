using System;

namespace Light.BayesianNetwork
{
    public struct OutcomeProbability
    {
        public readonly float Value;

        private OutcomeProbability(float value)
        {
            Value = value;
        }

        public static OutcomeProbability DefaultMin => new OutcomeProbability((float) 0.0);
        public static OutcomeProbability DefaultMax => new OutcomeProbability((float) 1.0);


        public static OutcomeProbability FromValue(float value)
        {
            if(value < 0.0) throw new ArgumentException($"Value {value} is not allowed to be less than 0.0.");
            if(value > 1.0) throw new ArgumentException($"Value {value} is not allowed to be greater than 1.0.");

            return new OutcomeProbability(value);
        }
    }
}