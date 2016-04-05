using System;
using System.Collections.Generic;
using Light.BayesianNetwork.FrameworkExtensions;
using Light.GuardClauses;

namespace Light.BayesianNetwork
{
    public class BayesianNetwork : EntityWithName
    {
        public readonly ICollectionFactory CollectionFactory;
        public IRandomVariableNode NetworkParentNode;
        public readonly IProbabilityCalculator ProbabilityCalculator;
        private readonly IReasoner _reasoner;

        public BayesianNetwork(Guid id, IReasonerFactory reasonerFactory, IProbabilityCalculatorFactory probabilityCalculatorFactory) : this(id, new DefaultCollectionFactory(), reasonerFactory, probabilityCalculatorFactory)
        {
        }

        public BayesianNetwork(Guid id, ICollectionFactory collectionFactory, IReasonerFactory reasonerFactory, IProbabilityCalculatorFactory probabilityCalculator)
            : base(id)
        {
            //IList<RandomVariableNode> nodes;
            collectionFactory.MustNotBeNull(nameof(collectionFactory));
            reasonerFactory.MustNotBeNull(nameof(reasonerFactory));
            probabilityCalculator.MustNotBeNull(nameof(probabilityCalculator));

            CollectionFactory = collectionFactory;
            ProbabilityCalculator = probabilityCalculator.Create(this);
            //CollectionFactory.InitializeListFields(out nodes, out _nodesAsReadOnlyList);
            _reasoner = reasonerFactory.Create(this, ProbabilityCalculator);
        }

        public IReasoner Reasoner => _reasoner;

        public void AddNetworkParentNode(IRandomVariableNode node)
        {
            node.MustNotBeNull(nameof(node));
            if(NetworkParentNode != null) throw new Exception("The bayes network already has one parent node. Adding more network parent nodes not possbile");
            if(node.ParentNodes.Count != 0) throw new ArgumentException($"The new network parent node {node} has parents but the network parent node is not allowed to have parent nodes.");

            NetworkParentNode = node;
        }

        public void ReplaceNetworkParentNode(IRandomVariableNode node)
        {
            node.MustNotBeNull(nameof(node));

            if(NetworkParentNode != null)
                RemoveParentNode();

            AddNetworkParentNode(node);
        }

        public void RemoveParentNode()
        {
            NetworkParentNode = null;
        }
    }
}