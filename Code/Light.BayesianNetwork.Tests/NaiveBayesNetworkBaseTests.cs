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

        public BayesianNetwork GetPreconfiguredBayesianNetworkWithOneParentAndOneChild()
        {
            var network = GetRawNaiveBayesianNetwork();
            var parentNode = AddNetworkParentIncludingThreeOutcomes();
            var childNode = AddNetworkChildIncludingTwoOutcomes();

            childNode.ProbabilityTable.Add(new OutcomeCombination(parentNode.Outcomes[0], childNode.Outcomes[0]), (float) 0.15);
            childNode.ProbabilityTable.Add(new OutcomeCombination(parentNode.Outcomes[0], childNode.Outcomes[1]), (float) 0.85);

            childNode.ProbabilityTable.Add(new OutcomeCombination(parentNode.Outcomes[1], childNode.Outcomes[0]), (float) 0.2);
            childNode.ProbabilityTable.Add(new OutcomeCombination(parentNode.Outcomes[1], childNode.Outcomes[1]), (float) 0.8);

            childNode.ProbabilityTable.Add(new OutcomeCombination(parentNode.Outcomes[2], childNode.Outcomes[0]), (float) 0.7);
            childNode.ProbabilityTable.Add(new OutcomeCombination(parentNode.Outcomes[2], childNode.Outcomes[1]), (float) 0.3);

            return network;
        }

        public BayesianNetwork GetPreconfiguredBayesianNetworkWithOneParentAndTwoChild()
        {
            var network = GetPreconfiguredBayesianNetworkWithOneParentAndOneChild();
            var parentNode = network.NetworkParentNode;
            var secondChildNode = AddNetworkChildIncludingThreeOutcomes();

            secondChildNode.ProbabilityTable.Add(new OutcomeCombination(parentNode.Outcomes[0], secondChildNode.Outcomes[0]), (float) 0.1);
            secondChildNode.ProbabilityTable.Add(new OutcomeCombination(parentNode.Outcomes[0], secondChildNode.Outcomes[1]), (float) 0.2);
            secondChildNode.ProbabilityTable.Add(new OutcomeCombination(parentNode.Outcomes[0], secondChildNode.Outcomes[2]), (float) 0.7);

            secondChildNode.ProbabilityTable.Add(new OutcomeCombination(parentNode.Outcomes[1], secondChildNode.Outcomes[0]), (float) 0.25);
            secondChildNode.ProbabilityTable.Add(new OutcomeCombination(parentNode.Outcomes[1], secondChildNode.Outcomes[1]), (float) 0.25);
            secondChildNode.ProbabilityTable.Add(new OutcomeCombination(parentNode.Outcomes[1], secondChildNode.Outcomes[2]), (float) 0.5);

            secondChildNode.ProbabilityTable.Add(new OutcomeCombination(parentNode.Outcomes[2], secondChildNode.Outcomes[0]), (float) 0.4);
            secondChildNode.ProbabilityTable.Add(new OutcomeCombination(parentNode.Outcomes[2], secondChildNode.Outcomes[1]), (float) 0.4);
            secondChildNode.ProbabilityTable.Add(new OutcomeCombination(parentNode.Outcomes[2], secondChildNode.Outcomes[2]), (float) 0.2);

            return network;
        }

        public NaiveBayesRandomVariableNodeDecorator AddNetworkParentIncludingThreeOutcomes()
        {
            var networkParentNode = NewNaiveBayesRandomVariableNode();
            var nodeOutcomes = new List<Outcome> {new Outcome(Guid.NewGuid(), networkParentNode, (float) 0.2)
            {
                Name = "is"
            }, new Outcome(Guid.NewGuid(), networkParentNode, (float) 0.08)
            {
                Name = "h"
            }, new Outcome(Guid.NewGuid(), networkParentNode, (float) 0.72)
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
            var nodeOutcomes = new List<Outcome> { new Outcome(Guid.NewGuid(), networkChildNode, (float) 0.55) { Name = "in"},new Outcome(Guid.NewGuid(), networkChildNode, (float) 0.45) { Name = "n"}};

            networkChildNode.AddOutcomes(nodeOutcomes);

            if (_network == null)
                _network = new NaiveBayesNetworkBuilder().Build();

            _network.NetworkParentNode.ConnectChild(networkChildNode);

            return networkChildNode;
        }

        public NaiveBayesRandomVariableNodeDecorator AddNetworkChildIncludingThreeOutcomes()
        {
            var networkChildNode = NewNaiveBayesRandomVariableNode();
            var nodeOutcomes = new List<Outcome> { new Outcome(Guid.NewGuid(), networkChildNode, (float) 0.33) { Name = "s" }, new Outcome(Guid.NewGuid(), networkChildNode, (float) 0.35) { Name = "m" }, new Outcome(Guid.NewGuid(), networkChildNode, (float) 0.32) { Name = "n" } };

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
