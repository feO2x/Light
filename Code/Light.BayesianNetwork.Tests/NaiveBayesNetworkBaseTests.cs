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

        public BayesianNetwork GetNaiveBayesianNetwork()
        {
            if(_network == null)
                _network = new NaiveBayesNetworkBuilder().Build();

            _nodeBuilder = new NaiveBayesRandomVariableNodeBuilderFactory().Create(_network);

            return _network;
        }

        public NaiveBayesRandomVariableNodeDecorator NewNaiveBayesRandomVariableNode()
        {
            return _nodeBuilder.Build();
        }
    }
}
