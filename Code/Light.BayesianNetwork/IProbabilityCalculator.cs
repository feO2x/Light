using System.Collections.Generic;

namespace Light.BayesianNetwork
{
    public interface IProbabilityCalculator
    {
        void CalculateParentProbabilityFromEvidence(Outcome outcomeToSetEvidenceOn);
        void CalculateObservedProbabilitiesFromParentProbability(IList<IRandomVariableNode> childNodes);
        void CalculateOutcomeProbabilityForSpecificChildNodesOutcome(Outcome childNodeOutcome);
    }
}