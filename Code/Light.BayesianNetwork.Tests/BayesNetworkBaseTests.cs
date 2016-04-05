namespace Light.BayesianNetwork.Tests
{
    public class BayesNetworkBaseTests
    {
        private BayesianNetwork _network;
        private RandomVariableNodeBuilder _nodeBuilder;

        public BayesianNetwork GetNaiveBayesianNetwork()
        {
            if (_network == null)
                _network = new BayesNetworkBuilder().Build();

            _nodeBuilder = new RandomVariableNodeBuilderFactory().Create(_network);

            return _network;
        }

        public RandomVariableNode NewNaiveBayesRandomVariableNode()
        {
            return _nodeBuilder.Build();
        }
    }
}
