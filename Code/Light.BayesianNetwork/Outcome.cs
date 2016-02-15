using System;
using Light.BayesianNetwork.FrameworkExtensions;
using Light.GuardClauses;

namespace Light.BayesianNetwork
{
    public class Outcome : EntityWithName
    {
        private OutcomeProbability _currentProbability = OutcomeProbability.Default;

        public Outcome(Guid id, RandomVariableNode node) : base(id)
        {
            node.MustNotBeNull(nameof(node));

            Node = node;
        }

        public OutcomeProbability CurrentProbability
        {
            get { return _currentProbability; }
            set { _currentProbability = value; }
        }

        public RandomVariableNode Node { get; }

        public void SetEvidence()
        {
            if (_currentProbability.Kind == OutcomeProbabilityKind.SelectedEvidence)
                return;

            _currentProbability = OutcomeProbability.ValueIsEvidence;
            EvidenceSet?.Invoke(this);
        }

        public event Action<Outcome> EvidenceSet;
    }
}