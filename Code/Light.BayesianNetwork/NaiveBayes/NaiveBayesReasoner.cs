using System;
using System.Collections.Generic;
using System.Linq;
using Light.GuardClauses;

namespace Light.BayesianNetwork.NaiveBayes
{
    public class NaiveBayesReasoner : IReasoner
    {
        private readonly BayesianNetwork _network;
        private readonly IProbabilityCalculator _probabilityCalculator;

        public NaiveBayesReasoner(BayesianNetwork network, IProbabilityCalculator probabilityCalculator)
        {
            network.MustNotBeNull(nameof(network));
            probabilityCalculator.MustNotBeNull(nameof(probabilityCalculator));

            _network = network;
            _probabilityCalculator = probabilityCalculator;
        }

        private void PropagateNewChildsEvidenceToParent(Outcome outcome)
        {
            //should work for networks with two layers (one parent per network and one or more child nodes)
            _probabilityCalculator.CalculateParentProbabilityFromEvidence(outcome);
            PropagateNewParentPropability();
        }

        private void PropagateNewParentPropability()
        {
            var networkNodes = _network.Nodes;
            List<RandomVariableNode> networkParentNodes = networkNodes.Where(node => node.ParentNodes.Count == 0).ToList();

            if(networkParentNodes.Count == 0) throw new Exception("Naive bayes networks must have one parent node. The current network do not have a parent node.");
            if(networkParentNodes.Count > 1) throw new Exception($"Every naive bayes network must have one parent node. The current network consists of {networkParentNodes.Count} parent nodes.");

            var networkParentNode = networkParentNodes.First();

            _probabilityCalculator.CalculateObservedProbabilitiesFromParentProbability(networkParentNode.ChildNodes);
        }

        public void PropagateNewEvidence(Outcome outcomeWithNewEvidence)
        {
            outcomeWithNewEvidence.MustNotBeNull(nameof(outcomeWithNewEvidence));

            PropagateNewChildsEvidenceToParent(outcomeWithNewEvidence);
            //if evidence on naive bayes networks parent node is set, no more updates and calculations are needed
        }
    }
}