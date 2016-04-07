using System;
using System.Collections.Generic;
using Light.GuardClauses;

namespace Light.BayesianNetwork.Calculators
{
    public class DotNetProbabilityCalculator : IProbabilityCalculator
    {
        private readonly BayesianNetwork _network;

        public DotNetProbabilityCalculator(BayesianNetwork network)
        {
            network.MustNotBeNull(nameof(network));

            _network = network;
        }

        public void CalculateParentProbabilityFromEvidence(Outcome outcomeToSetEvidenceOn)
        {
            outcomeToSetEvidenceOn.MustNotBeNull(nameof(outcomeToSetEvidenceOn));

            var childNode = outcomeToSetEvidenceOn.Node;
            var parentNode = _network.NetworkParentNode;

            foreach (var parentNodeOutcome in parentNode.Outcomes)
            {
                var childsStandardOutcomeProbabilityValue = childNode.ProbabilityTable[new OutcomeCombination(parentNodeOutcome, outcomeToSetEvidenceOn)];
                var newParentNodeOutcomeProbabilityValue = parentNodeOutcome.CurrentProbability.Value / outcomeToSetEvidenceOn.StandardProbability.Value * childsStandardOutcomeProbabilityValue;

                parentNodeOutcome.CurrentProbability = OutcomeProbability.FromValue(newParentNodeOutcomeProbabilityValue);
            }
        }

        //calculator has to know the change (or just the new value) of the networks parent probability
        public void CalculateObservedProbabilitiesFromParentProbability(IReadOnlyList<IRandomVariableNode> childNodes)
        {
            foreach (var node in childNodes)
            {
                //no update for nodes with set evidence in outcomes
                if(node.ProbabilityKind() == OutcomeProbabilityKind.Evidence)
                    continue;

                foreach (var outcome in node.Outcomes)
                {
                    CalculateOutcomeProbabilityForSpecificChildNodesOutcome(outcome);
                }
            }
        }

        //calculator has to know the change (or just the new value) of the networks parent probability
        public void CalculateOutcomeProbabilityForSpecificChildNodesOutcome(Outcome childNodeOutcome)
        {
            childNodeOutcome.MustNotBeNull(nameof(childNodeOutcome));

            if (childNodeOutcome.Node.ProbabilityKind() == OutcomeProbabilityKind.Evidence)
                return;

            double result = 0;
            var childNode = childNodeOutcome.Node;

            foreach (var parentNodeOutcome in _network.NetworkParentNode.Outcomes)
            {
                double childParentOutcomeProbabilityFromTable;
                var outcomeCombinationChildParentOutcome = new OutcomeCombination(parentNodeOutcome, childNodeOutcome);
                if (childNode.ProbabilityTable.TryGetValue(outcomeCombinationChildParentOutcome, out childParentOutcomeProbabilityFromTable) == false)
                    throw new Exception($"The child nodes {childNode} probability table does not contain a probability combination for outcomes {childNodeOutcome} (childNodeOutcome) and {parentNodeOutcome} (parentNodeOutcome).");
                result += parentNodeOutcome.CurrentProbability.Value * childParentOutcomeProbabilityFromTable;
            }

            childNodeOutcome.CurrentProbability = OutcomeProbability.FromValue(result);
        }


    }
}