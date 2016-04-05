using Light.GuardClauses;

namespace Light.BayesianNetwork.NaiveBayes
{
    public class NaiveBayesRandomVariableNodeBuilderFactory
    {
        public NaiveBayesRandomVariableNodeBuilder Create(BayesianNetwork network)
        {
            network.MustNotBeNull(nameof(network));

            return new NaiveBayesRandomVariableNodeBuilder(network);
        }
    }
}