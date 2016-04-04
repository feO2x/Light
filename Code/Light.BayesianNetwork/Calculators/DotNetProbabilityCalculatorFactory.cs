using Light.GuardClauses;

namespace Light.BayesianNetwork.Calculators
{
    public class DotNetProbabilityCalculatorFactory : IProbabilityCalculatorFactory
    {
        public IProbabilityCalculator Create(BayesianNetwork network)
        {
            network.MustNotBeNull(nameof(network));

            return new DotNetProbabilityCalculator(network);
        }
    }
}