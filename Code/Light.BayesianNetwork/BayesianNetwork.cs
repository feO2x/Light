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