using System;
using System.Collections.Generic;
using System.Linq;
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
        private OutcomeProbabilityKind _probabilityKind;
        private IProbabilityCalculator _probabilityCalculator;

        public RandomVariableNode(Guid id, BayesianNetwork network, IProbabilityCalculatorFactory probabilityCalculatorFactory, OutcomeProbabilityKind probabilityKind = OutcomeProbabilityKind.CalculatedValue)
            : base(id)
        {
            network.MustNotBeNull(nameof(network));
            probabilityCalculatorFactory.MustNotBeNull(nameof(probabilityCalculatorFactory));

            Network = network;
            _probabilityCalculator = probabilityCalculatorFactory.Create();
            _probabilityKind = probabilityKind;
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

        public OutcomeProbabilityKind ProbabilityKind()
        {
            if(_outcomesAsReadOnlyList.Count > 0)
            {
                var probabilityKindOfFirstElement = _outcomesAsReadOnlyList.First().ProbabilityKind;
                if(_outcomesAsReadOnlyList.Any(o => o.ProbabilityKind == probabilityKindOfFirstElement))
                    return probabilityKindOfFirstElement;

                throw new Exception("All outcome probabilty kinds of one random variable node must be the same.");
            }

            throw new Exception("A random variable node must contain at least one outcome.");
        }

        private void OnEvidenceSet(Outcome evidenceOutcome)
        {
            foreach (var outcome in _outcomes)
            {
                if (evidenceOutcome == outcome)
                    continue;

                outcome.UpdateEvidenceRelatedToEvidenceChangeInNode();
            }

            Network.Reasoner.PropagateNewEvidence(evidenceOutcome.Node);
        }

        private void OnEvidenceRemove(Outcome evidenceOutcome)
        {
            foreach (var outcome in _outcomes)
            {
                outcome.CurrentProbability = OutcomeProbability.FromValue(_probabilityCalculator.CalculateOutcomeProbabilityForSpecificOutcome(outcome));

                if(evidenceOutcome == outcome)
                    continue;

                outcome.RemoveEvidence();
            }
        }
    }
}