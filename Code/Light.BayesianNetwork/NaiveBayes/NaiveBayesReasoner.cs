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

        private void PropagateNewChildsEvidenceToNetworkParent(Outcome outcome)
        {
            //should work for networks with two layers (one parent per network and one or more child nodes)
            var currentNode = outcome.Node;
            while (currentNode.ParentNodes.Count != 0)
            {
                _probabilityCalculator.CalculateParentProbabilityFromEvidence(currentNode.Outcomes.First());
                currentNode = currentNode.ParentNodes.First();
            }

            PropagateNewParentPropability(outcome);
        }

        private void PropagateNewParentPropability(Outcome outcome)
        {
            var networkParentNode = outcome.Node.ParentNodes.First();
            _probabilityCalculator.CalculateObservedProbabilitiesFromParentProbability(networkParentNode.ChildNodes);
        }

        public void PropagateNewEvidence(Outcome outcomeWithNewEvidence)
        {
            outcomeWithNewEvidence.MustNotBeNull(nameof(outcomeWithNewEvidence));

            PropagateNewChildsEvidenceToNetworkParent(outcomeWithNewEvidence);
            //if evidence on naive bayes networks parent node is set, no more updates and calculations are needed
        }
    }
}