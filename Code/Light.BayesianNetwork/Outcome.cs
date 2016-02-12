using System;
using Light.BayesianNetwork.FrameworkExtensions;
using Light.GuardClauses;

namespace Light.BayesianNetwork
{
    public class Outcome : EntityWithName
    {
        private double _currentProbability;

        public Outcome(Guid id, RandomVariableNode node) : base(id)
        {
            node.MustNotBeNull(nameof(node));

            Node = node;
        }

        public double CurrentProbability
        {
            get { return _currentProbability; }
            set
            {
                value.MustNotBeGreaterThan(1.0, nameof(value));
                value.MustNotBeLessThan(0.0, nameof(value));

                _currentProbability = value;
            }
        }

        public RandomVariableNode Node { get; }

        public void SetEvidence()
        {
            throw new NotImplementedException("TODO: set the current probability to 100% and raise observable event.");
        }
    }
}