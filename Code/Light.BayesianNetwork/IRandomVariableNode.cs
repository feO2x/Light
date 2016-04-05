using System.Collections.Generic;

namespace Light.BayesianNetwork
{
    public interface IRandomVariableNode
    {
        IReadOnlyList<IRandomVariableNode> ChildNodes { get; }
        BayesianNetwork Network { get; }
        IReadOnlyList<Outcome> Outcomes { get; }
        IList<IRandomVariableNode> ParentNodes { get; set; }
        IDictionary<OutcomeCombination, double> ProbabilityTable { get; }

        void AddOutcome(Outcome newOutcome);
        void ConnectChild(IRandomVariableNode childNode);
        void ConnectParent(IRandomVariableNode parentNode);
        void DisconnectChild(IRandomVariableNode childNode);
        void DisconnectChildAt(int index);
        void DisconnectParent(IRandomVariableNode parentNode);
        void DisconnectParentAt(int index);
        OutcomeProbabilityKind ProbabilityKind();
        void RemoveOutcome(Outcome existingOutcome);
        void RemoveOutcomeAt(int index);
    }
}