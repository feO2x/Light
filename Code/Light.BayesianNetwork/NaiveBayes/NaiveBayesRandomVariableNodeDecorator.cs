using System;
using System.Collections.Generic;
using System.Linq;
using Light.BayesianNetwork.FrameworkExtensions;
using Light.GuardClauses;

namespace Light.BayesianNetwork.NaiveBayes
{
    public class NaiveBayesRandomVariableNodeDecorator : EntityWithName, IRandomVariableNode
    {
        public BayesianNetwork Network => _decoratedNode.Network;
        public IReadOnlyList<Outcome> Outcomes => _decoratedNode.Outcomes;
        public IDictionary<OutcomeCombination, float> ProbabilityTable => _decoratedNode.ProbabilityTable;
        private readonly IRandomVariableNode _decoratedNode;

        public NaiveBayesRandomVariableNodeDecorator(IRandomVariableNode decoratedNode) : base(decoratedNode.Id)
        {
            decoratedNode.MustNotBeNull(nameof(decoratedNode));

            _decoratedNode = decoratedNode;
        }

        public void AddOutcomes(IReadOnlyList<Outcome> outcomes)
        {
            _decoratedNode.AddOutcomes(outcomes);
        }

        public IList<IRandomVariableNode> ChildNodes
        {
            get { return _decoratedNode.ChildNodes; }
            set { _decoratedNode.ChildNodes = value; }
        }

        public void ConnectChild(IRandomVariableNode childNode)
        {
            childNode.MustNotBeNull(nameof(childNode));

            if (TryAddChild(childNode))
                childNode.ConnectParent(this);
        }

        public void ConnectParent(IRandomVariableNode parentNode)
        {
            parentNode.MustNotBeNull(nameof(parentNode));

            if (TryAddParent(parentNode))
                parentNode.ConnectChild(this);
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

        private bool TryAddParent(IRandomVariableNode newParent)
        {
            if (_decoratedNode.ParentNodes.Contains(newParent))
                return false;

            _decoratedNode.ParentNodes.Add(newParent);
            return true;
        }

        private bool TryAddChild(IRandomVariableNode newChild)
        {
            if (_decoratedNode.ChildNodes.Contains(newChild))
                return false;

            _decoratedNode.ChildNodes.Add(newChild);
            return true;
        }
    }
}
