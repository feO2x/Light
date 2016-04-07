using System;
using Light.BayesianNetwork.FrameworkExtensions;
using Light.GuardClauses;

namespace Light.BayesianNetwork
{
    public class Outcome : EntityWithName
    {
        private OutcomeProbabilityKind _probabilityKind;
        private OutcomeProbability _currentProbabilityValue;
        private OutcomeProbability _previousProbabilityValue;

        public Outcome(Guid id, IRandomVariableNode node, double standardProbabilityValue, OutcomeProbabilityKind probabilityKind = OutcomeProbabilityKind.CalculatedValue) : base(id)
        {
            node.MustNotBeNull(nameof(node));

            Node = node;
            _probabilityKind = probabilityKind;
            _previousProbabilityValue = OutcomeProbability.FromValue(standardProbabilityValue);
            _currentProbabilityValue = _previousProbabilityValue;
        }

        public OutcomeProbability CurrentProbabilityValue
        {
            get { return _currentProbabilityValue; }
            set { _currentProbabilityValue = value; }
        }

        public OutcomeProbability PreviousProbabilityValue => _previousProbabilityValue;

        public OutcomeProbabilityKind ProbabilityKind => _probabilityKind;

        public IRandomVariableNode Node { get; }

        public void SetEvidence()
        {
            _previousProbabilityValue = _currentProbabilityValue;
            _probabilityKind = OutcomeProbabilityKind.Evidence;
            _currentProbabilityValue = OutcomeProbability.DefaultMax;

            EvidenceSet?.Invoke(this);
        }

        public void RemoveEvidence()
        {
            _probabilityKind = OutcomeProbabilityKind.CalculatedValue;

            EvidenceRemove?.Invoke(this);
        }

        public void UpdateEvidenceRelatedToEvidenceChangeInNode()
        {
            _previousProbabilityValue = _currentProbabilityValue;
            _probabilityKind = OutcomeProbabilityKind.Evidence;
            _currentProbabilityValue = OutcomeProbability.DefaultMin;
        }

        public event Action<Outcome> EvidenceSet;
        public event Action<Outcome> EvidenceRemove;
    }
}