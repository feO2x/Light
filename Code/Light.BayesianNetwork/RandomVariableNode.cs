using System;
using System.Collections.Generic;
using Light.BayesianNetwork.FrameworkExtensions;
using Light.GuardClauses;

namespace Light.BayesianNetwork
{
    public class RandomVariableNode : EntityWithName
    {
        private readonly IList<RandomVariableNode> _childNodes;
        private readonly IReadOnlyList<RandomVariableNode> _childNodesAsReadOnlyList;
        private readonly IList<Outcome> _outcomes;
        private readonly IReadOnlyList<Outcome> _outcomesAsReadOnlyList;
        private readonly IList<RandomVariableNode> _parentNodes;
        private readonly IReadOnlyList<RandomVariableNode> _parentNodesAsReadOnlyList;

        public RandomVariableNode(Guid id, BayesianNetwork network)
            : base(id)
        {
            network.MustNotBeNull(nameof(network));

            Network = network;
            network.CollectionFactory.InitializeListFields(out _parentNodes, out _parentNodesAsReadOnlyList);
            network.CollectionFactory.InitializeListFields(out _childNodes, out _childNodesAsReadOnlyList);
            network.CollectionFactory.InitializeListFields(out _outcomes, out _outcomesAsReadOnlyList);
            ProbabilityTable = network.CollectionFactory.CreateDictionary<OutcomeCombination, double>();
        }

        public BayesianNetwork Network { get; }

        public IReadOnlyList<RandomVariableNode> ParentNodes => _parentNodesAsReadOnlyList;
        public IReadOnlyList<RandomVariableNode> ChildNodes => _childNodesAsReadOnlyList;
        public IReadOnlyList<Outcome> Outcomes => _outcomesAsReadOnlyList;
        public IDictionary<OutcomeCombination, double> ProbabilityTable { get; }

        public void ConnectChild(RandomVariableNode childNode)
        {
            childNode.MustNotBeNull(nameof(childNode));

            _childNodes.Add(childNode);

            // TODO: hook up to events of the child node
        }

        public void DisconnectChild(RandomVariableNode childNode)
        {
            childNode.MustNotBeNull(nameof(childNode));

            if (_childNodes.Remove(childNode) == false)
                throw new ArgumentException($"The node {childNode} is no child node of {this}.", nameof(childNode));

            // TODO: unhook from events of removed node
        }

        public void DisconnectChildAt(int index)
        {
            index.MustNotBeLessThan(0, nameof(index));
            index.MustNotBeGreaterThanOrEqualTo(_childNodes.Count, nameof(index));

            _childNodes.RemoveAt(index);

            // TODO: unhook from events of removed node
        }

        public void ConnectParent(RandomVariableNode parentNode)
        {
            parentNode.MustNotBeNull(nameof(parentNode));

            _parentNodes.Add(parentNode);

            // TODO: hook up to events of the parent node
        }

        public void DisconnectParent(RandomVariableNode parentNode)
        {
            parentNode.MustNotBeNull(nameof(parentNode));

            if (_parentNodes.Remove(parentNode) == false)
                throw new ArgumentException($"The node {parentNode} is no parent node of {this}.", nameof(parentNode));

            // TODO: unhook from events of removed node
        }

        public void DisconnectParentAt(int index)
        {
            index.MustNotBeLessThan(0, nameof(index));
            index.MustNotBeGreaterThanOrEqualTo(_parentNodes.Count, nameof(index));

            _parentNodes.RemoveAt(index);

            // TODO: unhook from events of removed node
        }

        public void AddOutcome(Outcome newOutcome)
        {
            newOutcome.MustNotBeNull(nameof(newOutcome));
            newOutcome.MustNotBeOneOf(_outcomesAsReadOnlyList, nameof(newOutcome));

            _outcomes.Add(newOutcome);
            newOutcome.EvidenceSet += OnEvidenceSet;
        }

        public void RemoveOutcome(Outcome existingOutcome)
        {
            existingOutcome.MustNotBeNull(nameof(existingOutcome));

            if (_outcomes.Remove(existingOutcome) == false)
                throw new ArgumentException($"The outcome {existingOutcome} is not registered with node {this}", nameof(existingOutcome));

            existingOutcome.EvidenceSet -= OnEvidenceSet;
        }

        public void RemoveOutcomeAt(int index)
        {
            index.MustNotBeLessThan(0, nameof(index));
            index.MustNotBeGreaterThan(_outcomes.Count, nameof(index));

            var outcomeToBeRemoved = _outcomes[index];
            _outcomes.RemoveAt(index);

            outcomeToBeRemoved.EvidenceSet -= OnEvidenceSet;
        }

        private void OnEvidenceSet(Outcome evidenceOutcome)
        {
            foreach (var outcome in _outcomes)
            {
                if (evidenceOutcome == outcome)
                    continue;

                outcome.CurrentProbability = OutcomeProbability.ValueIsNotEvidence;
            }

            throw new NotImplementedException("Whom do we have to notify that our outcome values have changed?");
        }
    }
}