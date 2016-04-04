using Light.GuardClauses;

namespace Light.BayesianNetwork.NaiveBayes
{
    public class NaiveBayesReasonerFactory : IReasonerFactory
    {
        public IReasoner Create(BayesianNetwork network, IProbabilityCalculator probabilityCalculator)
        {
            network.MustNotBeNull(nameof(network));
            probabilityCalculator.MustNotBeNull(nameof(probabilityCalculator));

            return new NaiveBayesReasoner(network, probabilityCalculator);
        }
    }
}