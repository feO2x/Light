using System.Collections.Generic;

namespace Light.BayesianNetwork
{
    public interface IReasonerFactory
    {
        IReasoner Create(BayesianNetwork network, IProbabilityCalculator probabilityCalculator);
    }
}