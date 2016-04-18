using System;
using Light.GuardClauses;

namespace Light.BayesianNetwork
{
    public class RandomVariableNodeBuilder
    {
        private readonly BayesianNetwork _network;

        public RandomVariableNodeBuilder(BayesianNetwork network)
        {
            network.MustNotBeNull(nameof(network));

            _network = network;
        }

        public RandomVariableNode Build()
        {
            return new RandomVariableNode(Guid.NewGuid(), _network);
        }
    }
}
