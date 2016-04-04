using Light.GuardClauses;

namespace Light.BayesianNetwork
{
    public class RandomVariableNodeBuilderFactory
    {
        public RandomVariableNodeBuilder Create(BayesianNetwork network)
        {
            network.MustNotBeNull(nameof(network));

            return new RandomVariableNodeBuilder(network);
        }
    }
}