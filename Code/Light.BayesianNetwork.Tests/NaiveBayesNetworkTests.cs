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
                .Be("The bayes network already has one parent node. Adding more network parent nodes not possbile");
        }

        [Fact(DisplayName = "Adding a parent node to the naive bayes network parent node fails.")]
        // ReSharper disable once InconsistentNaming
        public void AddParentToNaiveBNParentNodeFails()
        {
            var network = GetNaiveBayesianNetwork();
            var networkParentNode = NewNaiveBayesRandomVariableNode();
            var parentParentNode = NewNaiveBayesRandomVariableNode();

            network.AddNetworkParentNode(networkParentNode);
            Action act = () => network.NetworkParentNode.ConnectParent(parentParentNode);

            //todo: need an RandomVariableNode decorator for naive bayes networks where multiple parents are not allowed and may other stuff 
            act.ShouldThrow<ArgumentException>()
                .And.Message.Should()
                .Be($"Cannot add node {parentParentNode} as parent to {networkParentNode} because {parentParentNode} is not the networks parent node.");
        }

        [Fact(DisplayName = "Adding a parent node that already is the naive bayes network parent node fails.")]
        // ReSharper disable once InconsistentNaming
        public void AddParentToNaiveBNThatHasAParent()
        {
            var network = GetNaiveBayesianNetwork();
            var networkParentNode = NewNaiveBayesRandomVariableNode();
            var parentParentNode = NewNaiveBayesRandomVariableNode();

            network.AddNetworkParentNode(networkParentNode);

            Action act = () => network.NetworkParentNode.ConnectParent(parentParentNode);

            act.ShouldThrow<ArgumentException>()
                .And.Message.Should()
                .Be($"Cannot add node {parentParentNode} as parent to {networkParentNode} because {parentParentNode} is not the networks parent node.");
        }

        [Fact(DisplayName = "Try adding a parent node that is not the networks parent node fails.")]
        // ReSharper disable once InconsistentNaming
        public void AddNonNetworkParentAsParent()
        {
            GetNaiveBayesianNetwork();
            var networkParentNode = NewNaiveBayesRandomVariableNode();
            var parentParentNode = NewNaiveBayesRandomVariableNode();

            Action act = () => networkParentNode.ConnectParent(parentParentNode);

            act.ShouldThrow<ArgumentException>()
                .And.Message.Should()
                .Be($"Cannot add node {parentParentNode} as parent to {networkParentNode} because {parentParentNode} is not the networks parent node.");
        }
    }
}
