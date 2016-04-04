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
        private RandomVariableNodeBuilder _nodeBuilder;

        public BayesianNetwork GetNaiveBayesianNetwork()
        {
            if(_network == null)
                _network = new NaiveBayesNetworkBuilder().Build();

            _nodeBuilder = new RandomVariableNodeBuilderFactory().Create(_network);

            return _network;
        }

        public RandomVariableNode NewRandomVariableNode()
        {
            return _nodeBuilder.Build();
        }
    }
}
