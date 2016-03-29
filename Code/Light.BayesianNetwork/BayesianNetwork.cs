using System;
using System.Collections.Generic;
using Light.BayesianNetwork.FrameworkExtensions;
using Light.GuardClauses;

namespace Light.BayesianNetwork
{
    public class BayesianNetwork : EntityWithName
    {
        private readonly IList<RandomVariableNode> _nodes;
        private readonly IReadOnlyList<RandomVariableNode> _nodesAsReadOnlyList;
        public readonly ICollectionFactory CollectionFactory;
        private RandomVariableNode _networkParentNode;

        public BayesianNetwork(Guid id) : this(id, new DefaultCollectionFactory())
        {
        }

        public BayesianNetwork(Guid id, ICollectionFactory collectionFactory)
            : base(id)
        {
            collectionFactory.MustNotBeNull(nameof(collectionFactory));

            CollectionFactory = collectionFactory;
            CollectionFactory.InitializeListFields(out _nodes, out _nodesAsReadOnlyList);
        }

        public IReadOnlyList<RandomVariableNode> Nodes => _nodesAsReadOnlyList;

        public void AddNode(RandomVariableNode node)
        {
            node.MustNotBeNull(nameof(node));

            _nodes.Add(node);

            // TODO: Check the network for consistency (no circular references etc.)
        }

        public void AddNetworkParentNode(RandomVariableNode node)
        {
            node.MustNotBeNull(nameof(node));
            if(_networkParentNode != null) throw new Exception("The bayes network already has one parent node. Adding more network parent nodes not possbile");
            if(node.ParentNodes.Count != 0) throw new ArgumentException($"The new network parent node {node} has parents but the network parent node is not allowed to have parent nodes.");

            _networkParentNode = node;
        }

        public void ReplaceNetworkParentNode(RandomVariableNode node)
        {
            node.MustNotBeNull(nameof(node));

            if(_networkParentNode != null)
                RemoveNode(_networkParentNode);

            AddNetworkParentNode(node);
        }

        public void RemoveNode(RandomVariableNode node)
        {
            node.MustNotBeNull(nameof(node));

            if (_nodes.Remove(node) == false)
                throw new ArgumentException($"The specified node {0} is no random variable in the network {this}.");

            foreach (var childNode in node.ChildNodes)
            {
                childNode.DisconnectParent(node);
            }
            foreach (var parentNode in node.ParentNodes)
            {
                parentNode.DisconnectChild(node);
            }
        }
    }
}