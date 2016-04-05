﻿using System;
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

        public BayesianNetwork GetNaiveBayesianNetwork()
        {
            if(_network == null)
                _network = new NaiveBayesNetworkBuilder().Build();

            _nodeBuilder = new NaiveBayesRandomVariableNodeBuilderFactory().Create(_network);

            return _network;
        }

        public NaiveBayesRandomVariableNodeDecorator AddNetworkParentIncludingThreeOutcomes()
        {
            var networkParentNode = NewNaiveBayesRandomVariableNode();
            var nodeOutcomes = new List<Outcome> {new Outcome(Guid.NewGuid(), networkParentNode)
            {
                CurrentProbability = OutcomeProbability.FromValue(0.2)
            }, new Outcome(Guid.NewGuid(), networkParentNode)
            {
                CurrentProbability = OutcomeProbability.FromValue(0.08)
            }, new Outcome(Guid.NewGuid(), networkParentNode)
            {
                CurrentProbability = OutcomeProbability.FromValue(0.72)
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
            var nodeOutcomes = new List<Outcome> { new Outcome(Guid.NewGuid(), networkChildNode) { CurrentProbability = OutcomeProbability.FromValue(0.55) },new Outcome(Guid.NewGuid(), networkChildNode) { CurrentProbability = OutcomeProbability.FromValue(0.45) }};

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
