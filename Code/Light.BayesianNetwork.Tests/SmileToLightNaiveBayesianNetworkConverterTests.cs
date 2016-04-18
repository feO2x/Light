using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using FluentAssertions;
using Light.BayesianNetwork.Calculators;
using Light.BayesianNetwork.NaiveBayes;
using Light.BayesianNetwork.NetworkConverter.SmileConverter;
using Light.GuardClauses;
using Xunit;

namespace Light.BayesianNetwork.Tests
{
    public class SmileToLightNaiveBayesianNetworkConverterTests
    {
        [Fact(DisplayName = "Parsing and converting a smile network with 22 node should instantiate 22 smile nodes.")]
        // ReSharper disable once InconsistentNaming
        public void ParseSmileNetworkAll()
        {
            XDocument networkXml = XDocument.Load("naivenetwork.xdsl");

            var network = new BayesNetworkBuilder().Build();
            
            var converter = new SmileToLightNetworkConverter(network, new NaiveBayesRandomVariableNodeBuilder(network), new ParentSmileNodesParser(), new ChildSmileNodesParser());
            var nodes = converter.GetSmileNetworkNodes(networkXml);

            nodes.Count.Should().Be(22);
        }

        [Fact(DisplayName = "Parsing and converting a smile network with 22 node should instantiate 22 random variable nodes.")]
        // ReSharper disable once InconsistentNaming
        public void ParseAndAddSmileNetworkNodesToNewBN()
        {
            XDocument networkXml = XDocument.Load("naivenetwork.xdsl");

            var network = new NaiveBayesNetworkBuilder().Build();
            var nodeBuilder = new NaiveBayesRandomVariableNodeBuilderFactory().Create(network);
            var converter = new SmileToLightNetworkConverter(network, nodeBuilder,
                new ParentSmileNodesParser(), new ChildSmileNodesParser());
            var nodes = converter.ConvertNodesAndAddToNetwork(networkXml);

            nodes.First().Network.CalculateAllNetworkChildProbabilitiesAccordingToParentOutcomeValues();
            nodes[1].Outcomes[0].SetEvidence();
            nodes[16].Outcomes[0].SetEvidence();

            nodes.Count.Should().Be(22);
        }
    }
}
