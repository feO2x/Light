using System;
using Light.BayesianNetwork.FrameworkExtensions;
using Light.GuardClauses;

namespace Light.BayesianNetwork
{
    public class Outcome : EntityWithName
    {
        private OutcomeProbabilityKind _probabilityKind;
        private OutcomeProbability _currentProbability = OutcomeProbability.DefaultMin;

        public Outcome(Guid id, IRandomVariableNode node, OutcomeProbabilityKind probabilityKind = OutcomeProbabilityKind.CalculatedValue) : base(id)
        {
            _probabilityKind = probabilityKind;
            node.MustNotBeNull(nameof(node));

            Node = node;
            _probabilityKind = probabilityKind;
        }

        public OutcomeProbability CurrentProbability
        {
            get { return _currentProbability; }
            set { _currentProbability = value; }
        }

        public OutcomeProbabilityKind ProbabilityKind => _probabilityKind;

        public IRandomVariableNode Node { get; }

        public void SetEvidence()
        {
            _probabilityKind = OutcomeProbabilityKind.Evidence;
            _currentProbability = OutcomeProbability.DefaultMax;

            EvidenceSet?.Invoke(this);
        }

        public void RemoveEvidence()
        {
            _probabilityKind = OutcomeProbabilityKind.CalculatedValue;

            EvidenceRemove?.Invoke(this);
        }

        public void UpdateEvidenceRelatedToEvidenceChangeInNode()
        {
            _probabilityKind = OutcomeProbabilityKind.Evidence;
            _currentProbability = OutcomeProbability.DefaultMin;
        }

        public event Action<Outcome> EvidenceSet;
        public event Action<Outcome> EvidenceRemove;
    }
}