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
            var network = GetRawNaiveBayesianNetwork();
            network.NetworkParentNode.Should().Be(null);
        }

        [Fact(DisplayName = "Adding three outcomes to a naive bayes networks parent node.")]
        // ReSharper disable once InconsistentNaming
        public void NaiveBNWithProbabilityTableOnParent()
        {
            var network = GetRawNaiveBayesianNetwork();

            AddNetworkParentIncludingThreeOutcomes();

            network.NetworkParentNode.Outcomes.Count.Should().Be(3);
        }

        [Fact(DisplayName = "Adding a child node with outcomes to a naive bayes networks parent node.")]
        // ReSharper disable once InconsistentNaming
        public void NaiveBNWithProbabilityTableOnChild()
        {
            var network = GetRawNaiveBayesianNetwork();
            AddNetworkParentIncludingThreeOutcomes();

            AddNetworkChildIncludingTwoOutcomes();

            // ReSharper disable once PossibleNullReferenceException
            network.NetworkParentNode.ChildNodes.FirstOrDefault().Outcomes.Count.Should().Be(2);
        }

        [Fact(DisplayName = "Propagate evidence in child to parent, calculate new childs and parents probability values and set them to outcomes.")]
        // ReSharper disable once InconsistentNaming
        public void NaiveBNWithTwoNodesSetChildEvidence()
        {
            var network = GetPreconfiguredBayesianNetworkWithOneParentAndOneChild();

            // ReSharper disable once PossibleNullReferenceException
            network.NetworkParentNode.ChildNodes.FirstOrDefault().Outcomes[0].SetEvidence();
            // ReSharper disable once PossibleNullReferenceException
            var isOutcomeProbability = network.NetworkParentNode.Outcomes[0].CurrentProbability.Value;

            (Math.Abs(isOutcomeProbability - 0.054) < 0.001).Should().Be(true);
        }

        [Fact(DisplayName = "Propagate evidence in one child to parent, calculate new childs and parents probability values and set them to outcomes.")]
        // ReSharper disable once InconsistentNaming
        public void NaiveBNWithThreeNodesSetOneChildsEvidence()
        {
            var network = GetPreconfiguredBayesianNetworkWithOneParentAndTwoChild();
            var secondChildNode = network.NetworkParentNode.ChildNodes[1];

            // ReSharper disable once PossibleNullReferenceException
            network.NetworkParentNode.ChildNodes.FirstOrDefault().Outcomes[0].SetEvidence();
            // ReSharper disable once PossibleNullReferenceException
            var secondChildsFirstOutcome = secondChildNode.Outcomes[0].CurrentProbability.Value;

            (Math.Abs(secondChildsFirstOutcome - 0.38) < 0.001).Should().Be(true);
        }

        [Fact(DisplayName = "One parent node can be added to a naive bayes network.")]
        // ReSharper disable once InconsistentNaming
        public void NaiveBNWithParentNode()
        {
            var network = GetRawNaiveBayesianNetwork();
            var parentNode = NewNaiveBayesRandomVariableNode();

            network.AddNetworkParentNode(parentNode);

            network.NetworkParentNode.ShouldBeEquivalentTo(parentNode);
        }

        [Fact(DisplayName = "Two parent node cannot be added to a naive bayes network.")]
        // ReSharper disable once InconsistentNaming
        public void NaiveBNWithTwoParentNodesFails()
        {
            var network = GetRawNaiveBayesianNetwork();
            var parentNode = NewNaiveBayesRandomVariableNode();
            var secondParentNode = NewNaiveBayesRandomVariableNode();

            network.AddNetworkParentNode(parentNode);
            Action act = () => network.AddNetworkParentNode(secondParentNode);

            act.ShouldThrow<Exception>()
                .And.Message.Should()
                .Be($"The bayes network already has one parent node. Adding {secondParentNode} as second network parent node is not possbile.");
        }

        [Fact(DisplayName = "Adding a parent node to the naive bayes network parent node fails.")]
        // ReSharper disable once InconsistentNaming
        public void AddParentToNaiveBNParentNodeFails()
        {
            var network = GetRawNaiveBayesianNetwork();
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
            var network = GetRawNaiveBayesianNetwork();
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
            GetRawNaiveBayesianNetwork();
            var networkParentNode = NewNaiveBayesRandomVariableNode();
            var parentParentNode = NewNaiveBayesRandomVariableNode();

            Action act = () => networkParentNode.ConnectParent(parentParentNode);

            act.ShouldThrow<ArgumentException>()
                .And.Message.Should()
                .Be($"Cannot add node {parentParentNode} as parent to {networkParentNode} because {parentParentNode} is not the networks parent node.");
        }
    }
}
