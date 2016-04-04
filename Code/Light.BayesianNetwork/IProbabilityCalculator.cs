using System.Collections.Generic;

namespace Light.BayesianNetwork
{
    public interface IProbabilityCalculator
    {
        void CalculateParentProbabilityFromEvidence(Outcome outcomeToSetEvidenceOn);
        void CalculateObservedProbabilitiesFromParentProbability(IReadOnlyList<RandomVariableNode> childNodes);
        void CalculateOutcomeProbabilityForSpecificOutcome(Outcome outcome);
    }
}