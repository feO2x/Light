using System;
using Light.GuardClauses;

namespace Light.BayesianNetwork.NaiveBayes
{
    public class NaiveBayesRandomVariableNodeBuilder
    {
        private IRandomVariableNode _node;

        public NaiveBayesRandomVariableNodeBuilder(BayesianNetwork network)
        {
            network.MustNotBeNull(nameof(network));

            _node = new RandomVariableNodeBuilder(network).Build();
        }

        public NaiveBayesRandomVariableNodeBuilder WithDecorateRandomVariableNode(IRandomVariableNode node)
        {
            node.MustNotBeNull(nameof(node));
            _node = node;
            return this;
        }

        public NaiveBayesRandomVariableNodeDecorator Build()
        {
            return new NaiveBayesRandomVariableNodeDecorator(_node);
        }
    }
}
