using System.Collections.Generic;

namespace Light.BayesianNetwork
{
    public interface IProbabilityCalculator
    {
        void CalculateParentProbabilityFromEvidence(RandomVariableNode node);
        void CalculateObservedProbabilitiesFromParentProbability(IReadOnlyList<RandomVariableNode> childNodes);
    }
}