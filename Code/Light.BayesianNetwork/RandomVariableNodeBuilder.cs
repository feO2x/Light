using System;
using Light.GuardClauses;

namespace Light.BayesianNetwork
{
    public class RandomVariableNodeBuilder
    {
        private readonly BayesianNetwork _network;
        private Guid _guid = Guid.NewGuid();

        public RandomVariableNodeBuilder(BayesianNetwork network)
        {
            network.MustNotBeNull(nameof(network));

            _network = network;
        }

        public RandomVariableNodeBuilder WithGuid(Guid guid)
        {
            _guid = guid;
            return this;
        }

        public RandomVariableNode Build()
        {
            return new RandomVariableNode(_guid, _network);
        }
    }
}
