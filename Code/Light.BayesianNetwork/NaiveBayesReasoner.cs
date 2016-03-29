using System;
using System.Collections.Generic;
using System.Linq;
using Light.GuardClauses;

namespace Light.BayesianNetwork
{
    public class NaiveBayesReasoner : IReasoner
    {
        private readonly IReadOnlyList<RandomVariableNode> _networkNodes;
        private readonly IProbabilityCalculator _probabilityCalculator;

        public NaiveBayesReasoner(IReadOnlyList<RandomVariableNode> networkNodes, IProbabilityCalculator probabilityCalculator)
        {
            networkNodes.MustNotBeNull(nameof(networkNodes));
            probabilityCalculator.MustNotBeNull(nameof(probabilityCalculator));

            _networkNodes = networkNodes;
            _probabilityCalculator = probabilityCalculator;
        }

        private void PropagateNewEvidenceInChild(RandomVariableNode node)
        {
            //should work for networks with two layers (one parent per network and one or more child nodes)
            _probabilityCalculator.CalculateParentProbabilityFromEvidence(node);
            PropagateNewParentPropability();
        }

        private void PropagateNewParentPropability()
        {
            List<RandomVariableNode> networkParentNodes = _networkNodes.Where(node => node.ParentNodes.Count == 0).ToList();

            if(networkParentNodes.Count == 0) throw new Exception("Naive bayes networks must have one parent node. The current network do not have a parent node.");
            if(networkParentNodes.Count > 1) throw new Exception($"Every naive bayes network must have one parent node. The current network consists of {networkParentNodes.Count} parent nodes.");

            var networkParentNode = networkParentNodes.First();

            _probabilityCalculator.CalculateObservedProbabilitiesFromParentProbability(networkParentNode.ChildNodes);
        }

        public void PropagateNewEvidence(RandomVariableNode node)
        {
            node.MustNotBeNull(nameof(node));

            if(node.ParentNodes.Count > 0)
                PropagateNewEvidenceInChild(node);

            //if evidence on naive bayes networks parent node is set, no more updates and calculations are needed
        }
    }
}