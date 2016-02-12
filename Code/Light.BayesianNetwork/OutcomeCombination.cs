using System;
using System.Linq;
using Light.GuardClauses;
using Light.GuardClauses.FrameworkExtensions;

namespace Light.BayesianNetwork
{
    public struct OutcomeCombination : IEquatable<OutcomeCombination>
    {
        public readonly Outcome ParentOutcome;
        public readonly Outcome ChildOutcome;

        public OutcomeCombination(Outcome parentOutcome, Outcome childOutcome)
        {
            parentOutcome.MustNotBeNull(nameof(parentOutcome));
            childOutcome.MustNotBeNull(nameof(childOutcome));
            Guard.Against(childOutcome.Node.ParentNodes.Contains(parentOutcome.Node),
                          () => new ArgumentException($"The node {childOutcome.Node} of the specified child outcome {childOutcome} is no child of the parent node {parentOutcome.Node}."));

            ParentOutcome = parentOutcome;
            ChildOutcome = childOutcome;
        }

        public bool Equals(OutcomeCombination other)
        {
            return other.ParentOutcome.EqualsWithHashCode(other.ParentOutcome) && other.ChildOutcome.EqualsWithHashCode(other.ChildOutcome);
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
            return EqualityHelper.CreateHashCode(ParentOutcome, ChildOutcome);
        }
    }
}