using System.Collections.Generic;

namespace Light.BayesianNetwork
{
    public interface IReasonerFactory
    {
        IReasoner Create(IReadOnlyList<RandomVariableNode> networkNodes, IProbabilityCalculator probabilityCalculator);
    }
}