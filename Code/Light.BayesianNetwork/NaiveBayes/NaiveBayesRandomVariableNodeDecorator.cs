using System;
using System.Collections.Generic;
using Light.GuardClauses;

namespace Light.BayesianNetwork.NaiveBayes
{
    public class NaiveBayesRandomVariableNodeDecorator : IRandomVariableNode
    {
        public IReadOnlyList<IRandomVariableNode> ChildNodes => _decoratedNode.ChildNodes;
        public BayesianNetwork Network => _decoratedNode.Network;
        public IReadOnlyList<Outcome> Outcomes => _decoratedNode.Outcomes;
        public IDictionary<OutcomeCombination, double> ProbabilityTable => _decoratedNode.ProbabilityTable;
        private readonly IRandomVariableNode _decoratedNode;

        public NaiveBayesRandomVariableNodeDecorator(IRandomVariableNode decoratedNode)
        {
            decoratedNode.MustNotBeNull(nameof(decoratedNode));

            _decoratedNode = decoratedNode;
        }

        public void AddOutcomes(IReadOnlyList<Outcome> outcomes)
        {
            _decoratedNode.AddOutcomes(outcomes);
        }

        public void ConnectChild(IRandomVariableNode childNode)
        {
            childNode.MustNotBeNull(nameof(childNode));

            if(Network.NetworkParentNode != this)
                throw new ArgumentException($"Cannot add child {childNode} to node {this} because {this} is not the networks parent node.");

            _decoratedNode.ConnectChild(childNode);
        }

        public void ConnectParent(IRandomVariableNode parentNode)
        {
            parentNode.MustNotBeNull(nameof(parentNode));

            if(Network.NetworkParentNode != parentNode)
                throw new ArgumentException($"Cannot add node {parentNode} as parent to {this} because {parentNode} is not the networks parent node.");

            ParentNodes = new List<IRandomVariableNode>();

            _decoratedNode.ConnectParent(parentNode);
        }

        public IList<IRandomVariableNode> ParentNodes {
            get { return _decoratedNode.ParentNodes; }
            set
            {
                value.MustNotBeNull(nameof(value));

                _decoratedNode.ParentNodes = value;
            }
        }

        public void DisconnectChild(IRandomVariableNode childNode)
        {
            _decoratedNode.DisconnectChild(childNode);
        }

        public void DisconnectChildAt(int index)
        {
            _decoratedNode.DisconnectChildAt(index);
        }

        public void DisconnectParent(IRandomVariableNode parentNode)
        {
            _decoratedNode.DisconnectParent(parentNode);
        }

        public void DisconnectParentAt(int index)
        {
            _decoratedNode.DisconnectParentAt(index);
        }

        public OutcomeProbabilityKind ProbabilityKind()
        {
            return _decoratedNode.ProbabilityKind();
        }

        public void RemoveOutcome(Outcome existingOutcome)
        {
            _decoratedNode.RemoveOutcome(existingOutcome);
        }

        public void RemoveOutcomeAt(int index)
        {
            _decoratedNode.RemoveOutcomeAt(index);
        }
    }
}
