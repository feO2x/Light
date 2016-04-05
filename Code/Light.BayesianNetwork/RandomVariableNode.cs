using System;
using System.Collections.Generic;
using System.Linq;
using Light.BayesianNetwork.FrameworkExtensions;
using Light.GuardClauses;

namespace Light.BayesianNetwork
{
    public class RandomVariableNode : EntityWithName, IRandomVariableNode
    {
        private readonly IList<IRandomVariableNode> _childNodes;
        private readonly IReadOnlyList<IRandomVariableNode> _childNodesAsReadOnlyList;
        private readonly IList<Outcome> _outcomes;
        private readonly IReadOnlyList<Outcome> _outcomesAsReadOnlyList;
        private IList<IRandomVariableNode> _parentNodes;
        private readonly IReadOnlyList<IRandomVariableNode> _parentNodesAsReadOnlyList;
        private OutcomeProbabilityKind _probabilityKind;
        private readonly IProbabilityCalculator _probabilityCalculator;

        public RandomVariableNode(Guid id, BayesianNetwork network, OutcomeProbabilityKind probabilityKind = OutcomeProbabilityKind.CalculatedValue)
            : base(id)
        {
            network.MustNotBeNull(nameof(network));

            Network = network;
            _probabilityCalculator = network.ProbabilityCalculator;
            _probabilityKind = probabilityKind;
            network.CollectionFactory.InitializeListFields(out _parentNodes, out _parentNodesAsReadOnlyList);
            network.CollectionFactory.InitializeListFields(out _childNodes, out _childNodesAsReadOnlyList);
            network.CollectionFactory.InitializeListFields(out _outcomes, out _outcomesAsReadOnlyList);
            ProbabilityTable = network.CollectionFactory.CreateDictionary<OutcomeCombination, double>();
        }

        public BayesianNetwork Network { get; }

        public IList<IRandomVariableNode> ParentNodes
        {
            get { return _parentNodes; }
            set
            {
                value.MustNotBeNull(nameof(value));

                _parentNodes = value;
            }
        }
        public IReadOnlyList<IRandomVariableNode> ChildNodes => _childNodesAsReadOnlyList;
        public IReadOnlyList<Outcome> Outcomes => _outcomesAsReadOnlyList;
        public IDictionary<OutcomeCombination, double> ProbabilityTable { get; }

        public void ConnectChild(IRandomVariableNode childNode)
        {
            childNode.MustNotBeNull(nameof(childNode));

            _childNodes.Add(childNode);

            // TODO: hook up to events of the child node
        }

        public void DisconnectChild(IRandomVariableNode childNode)
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

        public void ConnectParent(IRandomVariableNode parentNode)
        {
            parentNode.MustNotBeNull(nameof(parentNode));

            _parentNodes.Add(parentNode);

            // TODO: hook up to events of the parent node
        }

        public void DisconnectParent(IRandomVariableNode parentNode)
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

        public void AddOutcomes(IReadOnlyList<Outcome> outcomes)
        {
            if(outcomes.Count < 2) throw new ArgumentException($"{outcomes} must include at least 2 outcomes but has {outcomes.Count}.");

            var outcomeProbabilitySum = outcomes.Sum(outcome => outcome.CurrentProbability.Value);
            if (Math.Abs(outcomeProbabilitySum - 1.0) > 0.01)
                throw new ArgumentException($"The sum of all nodes outcomes must be 1 but is {outcomeProbabilitySum}");

            foreach (var outcome in outcomes)
            {
                AddOutcome(outcome);
            }
        }

        private void AddOutcome(Outcome newOutcome)
        {
            newOutcome.MustNotBeOneOf(_outcomesAsReadOnlyList, nameof(newOutcome));

            _outcomes.Add(newOutcome);
            newOutcome.EvidenceSet += OnEvidenceSet;
            newOutcome.EvidenceRemove += OnEvidenceRemove;
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

        public OutcomeProbabilityKind ProbabilityKind() => _probabilityKind;

        private void OnEvidenceSet(Outcome evidenceOutcome)
        {
            _probabilityKind = OutcomeProbabilityKind.Evidence;

            foreach (var outcome in _outcomes)
            {
                if (evidenceOutcome == outcome)
                    continue;

                outcome.UpdateEvidenceRelatedToEvidenceChangeInNode();
            }

            Network.Reasoner.PropagateNewEvidence(evidenceOutcome);
        }

        private void OnEvidenceRemove(Outcome evidenceOutcome)
        {
            _probabilityKind = OutcomeProbabilityKind.CalculatedValue;

            foreach (var outcome in _outcomes)
            {
                _probabilityCalculator.CalculateOutcomeProbabilityForSpecificOutcome(outcome);

                if(evidenceOutcome == outcome)
                    continue;

                outcome.RemoveEvidence();
            }
        }
    }
}