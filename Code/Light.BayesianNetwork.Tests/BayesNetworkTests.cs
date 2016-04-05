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
    public class BayesNetworkTests : BayesNetworkBaseTests
    {
        [Fact(DisplayName = "A new naive bayes network should have zero nodes.")]
        // ReSharper disable once InconsistentNaming
        public void NaiveBNWithZeroNodes()
        {
            var network = new NaiveBayesNetworkBuilder().Build();
            network.NetworkParentNode.Should().Be(null);
        }

        [Fact(DisplayName = "One parent node can be added to a naive bayes network.")]
        // ReSharper disable once InconsistentNaming
        public void NaiveBNWithParentNode()
        {
            var network = GetNaiveBayesianNetwork();
            var parentNode = NewNaiveBayesRandomVariableNode();

            network.AddNetworkParentNode(parentNode);

            network.NetworkParentNode.ShouldBeEquivalentTo(parentNode);
        }

        [Fact(DisplayName = "Two parent node cannot be added to a naive bayes network.")]
        // ReSharper disable once InconsistentNaming
        public void NaiveBNWithTwoParentNodesFails()
        {
            var network = GetNaiveBayesianNetwork();
            var parentNode = NewNaiveBayesRandomVariableNode();
            var secondParentNode = NewNaiveBayesRandomVariableNode();

            network.AddNetworkParentNode(parentNode);
            Action act = () => network.AddNetworkParentNode(secondParentNode);

            act.ShouldThrow<Exception>()
                .And.Message.Should()
                .Be($"The bayes network already has one parent node. Adding {secondParentNode} as second network parent node is not possbile.");
        }

        [Fact(DisplayName = "Adding a parent node to the naive bayes network parent node does not change the network parent node.")]
        // ReSharper disable once InconsistentNaming
        public void AddParentToNaiveBNParentNodeFails()
        {
            var network = GetNaiveBayesianNetwork();
            var networkParentNode = NewNaiveBayesRandomVariableNode();
            var parentParentNode = NewNaiveBayesRandomVariableNode();

            network.AddNetworkParentNode(networkParentNode);
            network.NetworkParentNode.ConnectParent(parentParentNode);

            network.NetworkParentNode.Should().Be(networkParentNode);
        }

        [Fact(DisplayName = "Adding a second parent node to the naive bayes network.")]
        // ReSharper disable once InconsistentNaming
        public void AddParentToNaiveBNThatHasAParent()
        {
            var network = GetNaiveBayesianNetwork();
            var networkParentNode = NewNaiveBayesRandomVariableNode();
            var secondParentNode = NewNaiveBayesRandomVariableNode();

            network.AddNetworkParentNode(networkParentNode);
            Action act = () => network.AddNetworkParentNode(secondParentNode);

            act.ShouldThrow<Exception>(
                $"The bayes network already has one parent node. Adding {secondParentNode} as second network parent node is not possbile.");
        }

        [Fact(DisplayName = "Adding a parent node that is not the networks parent node.")]
        // ReSharper disable once InconsistentNaming
        public void AddNonNetworkParentAsParent()
        {
            GetNaiveBayesianNetwork();
            var networkParentNode = NewNaiveBayesRandomVariableNode();
            var parentParentNode = NewNaiveBayesRandomVariableNode();

            networkParentNode.ConnectParent(parentParentNode);

            networkParentNode.ParentNodes.FirstOrDefault().Should().Be(parentParentNode);
        }
    }
}
