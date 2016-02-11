using System;
using Light.GuardClauses.FrameworkExtensions;

namespace Light.BayesianNetwork
{
    public struct OutcomeCombination : IEquatable<OutcomeCombination>
    {
        public readonly IOutcome ParentOutcome;
        public readonly IOutcome NodeOutcome;

        public OutcomeCombination(IOutcome parentOutcome, IOutcome nodeOutcome)
        {
            ParentOutcome = parentOutcome;
            NodeOutcome = nodeOutcome;
        }

        public bool Equals(OutcomeCombination other)
        {
            return other.ParentOutcome.EqualsWithHashCode(other.ParentOutcome) && other.NodeOutcome.EqualsWithHashCode(other.NodeOutcome);
        }

        public override bool Equals(object obj)
        {
            try
            {
                return Equals((OutcomeCombination) obj);
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return EqualityHelper.CreateHashCode(ParentOutcome, NodeOutcome);
        }
    }
}