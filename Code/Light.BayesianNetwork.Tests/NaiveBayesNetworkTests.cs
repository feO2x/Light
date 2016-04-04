using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Light.BayesianNetwork.NaiveBayes;
using Xunit;

namespace Light.BayesianNetwork.Tests
{
    public sealed class NaiveBayesNetworkTests : NaiveBayesNetworkBaseTests
    {
        [Fact(DisplayName = "A new naive bayes network should have zero nodes.")]
        // ReSharper disable once InconsistentNaming
        public void NaiveBNWithZeroNodes()
        {
            var network = new NaiveBayesNetworkBuilder().Build();
            network.Nodes.Count.Should().Be(0);
        }

        [Fact(DisplayName = "One parent node can be added to a naive bayes network.")]
        // ReSharper disable once InconsistentNaming
        public void NaiveBNWithParentNode()
        {
            var network = GetNaiveBayesianNetwork();
            var parentNode = NewRandomVariableNode();

            network.AddNetworkParentNode(parentNode);

            network.NetworkParentNode.ShouldBeEquivalentTo(parentNode);
        }
    }
}
