using System.Collections.Generic;
using Light.GuardClauses;

namespace Light.BayesianNetwork
{
    public class NaiveBayesReasonerFactory : IReasonerFactory
    {
        public IReasoner Create(IReadOnlyList<RandomVariableNode> networkNodes, IProbabilityCalculator probabilityCalculator)
        {
            networkNodes.MustNotBeNull(nameof(networkNodes));
            probabilityCalculator.MustNotBeNull(nameof(probabilityCalculator));

            return new NaiveBayesReasoner(networkNodes, probabilityCalculator);
        }
    }
}