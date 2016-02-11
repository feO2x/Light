using System.Collections;
using System.Collections.Generic;
using Light.GuardClauses;

namespace Light.BayesianNetwork
{
    public interface INode
    {
        IBayesianNetwork Network { get; }
        IDictionary<OutcomeCombination, double> PropabilityTable { get; }
        IReadOnlyList<IOutcome> Outcome { get; }

        IReadOnlyList<INode> ParentNodes { get; }
        IReadOnlyList<INode> ChildNodes { get; }

        void AddParentNode(INode parentNode);
        void AddChildNode(INode childNode);
        void AddOutcome(IOutcome outcome);
        void RemoveParentNode(INode parentNode);
        void RemoveChildNOde(INode childNode);
        void RemoveOutcome(IOutcome outcome);
        void SetEvidence(IOutcome outcome);
    }
}