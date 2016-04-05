using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Light.BayesianNetwork.NaiveBayes;

namespace Light.BayesianNetwork.Tests
{
    public class NaiveBayesNetworkBaseTests
    {
        private BayesianNetwork _network;
        private NaiveBayesRandomVariableNodeBuilder _nodeBuilder;

        public BayesianNetwork GetRawNaiveBayesianNetwork()
        {
            if(_network == null)
                _network = new NaiveBayesNetworkBuilder().Build();

            _nodeBuilder = new NaiveBayesRandomVariableNodeBuilderFactory().Create(_network);

            return _network;
        }

        public BayesianNetwork GetSimplePreconfiguredBayesianNetwork()
        {
            var network = GetRawNaiveBayesianNetwork();
            var parentNode = AddNetworkParentIncludingThreeOutcomes();
            var childNode = AddNetworkChildIncludingTwoOutcomes();

            childNode.ProbabilityTable.Add(new OutcomeCombination(parentNode.Outcomes[0], childNode.Outcomes[0]), 0.15);
            childNode.ProbabilityTable.Add(new OutcomeCombination(parentNode.Outcomes[0], childNode.Outcomes[1]), 0.85);

            childNode.ProbabilityTable.Add(new OutcomeCombination(parentNode.Outcomes[1], childNode.Outcomes[0]), 0.2);
            childNode.ProbabilityTable.Add(new OutcomeCombination(parentNode.Outcomes[1], childNode.Outcomes[1]), 0.8);

            childNode.ProbabilityTable.Add(new OutcomeCombination(parentNode.Outcomes[2], childNode.Outcomes[0]), 0.7);
            childNode.ProbabilityTable.Add(new OutcomeCombination(parentNode.Outcomes[2], childNode.Outcomes[1]), 0.3);

            return network;
        }

        public NaiveBayesRandomVariableNodeDecorator AddNetworkParentIncludingThreeOutcomes()
        {
            var networkParentNode = NewNaiveBayesRandomVariableNode();
            var nodeOutcomes = new List<Outcome> {new Outcome(Guid.NewGuid(), networkParentNode, 0.2)
            {
                Name = "is"
            }, new Outcome(Guid.NewGuid(), networkParentNode, 0.08)
            {
                Name = "h"
            }, new Outcome(Guid.NewGuid(), networkParentNode, 0.72)
            {
                Name = "m"
            }};

            networkParentNode.AddOutcomes(nodeOutcomes);

            if (_network == null)
                _network = new NaiveBayesNetworkBuilder().Build();

            _network.NetworkParentNode = networkParentNode;

            return networkParentNode;
        }

        public NaiveBayesRandomVariableNodeDecorator AddNetworkChildIncludingTwoOutcomes()
        {
            var networkChildNode = NewNaiveBayesRandomVariableNode();
            var nodeOutcomes = new List<Outcome> { new Outcome(Guid.NewGuid(), networkChildNode, 0.55) { Name = "in"},new Outcome(Guid.NewGuid(), networkChildNode, 0.45) { Name = "n"}};

            networkChildNode.AddOutcomes(nodeOutcomes);

            if (_network == null)
                _network = new NaiveBayesNetworkBuilder().Build();

            _network.NetworkParentNode.ConnectChild(networkChildNode);

            return networkChildNode;
        }

        public NaiveBayesRandomVariableNodeDecorator NewNaiveBayesRandomVariableNode()
        {
            return _nodeBuilder.Build();
        }
    }
}
