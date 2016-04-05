using Light.GuardClauses;

namespace Light.BayesianNetwork.NaiveBayes
{
    public class NaiveBayesRandomVariableNodeBuilder
    {
        private readonly BayesianNetwork _network;

        public NaiveBayesRandomVariableNodeBuilder(BayesianNetwork network)
        {
            _network = network;
            network.MustNotBeNull(nameof(network));
        }

        public NaiveBayesRandomVariableNodeDecorator Build(IRandomVariableNode node = null)
        {
            if(node == null)
                node = new RandomVariableNodeBuilder(_network).Build();

            return new NaiveBayesRandomVariableNodeDecorator(node);
        }
    }
}
