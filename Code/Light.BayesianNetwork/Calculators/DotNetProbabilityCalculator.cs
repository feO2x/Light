using System;
using System.Collections.Generic;
using System.Linq;
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
            var parentNode = childNode.ParentNodes.First();

            foreach (var parentNodeOutcome in parentNode.Outcomes)
            {
                var childsStandardOutcomeProbabilityValue = childNode.ProbabilityTable[new OutcomeCombination(parentNodeOutcome, outcomeToSetEvidenceOn)];
                var newParentNodeOutcomeProbabilityValue = parentNodeOutcome.CurrentProbabilityValue.Value / outcomeToSetEvidenceOn.PreviousProbabilityValue.Value * childsStandardOutcomeProbabilityValue;

                parentNodeOutcome.CurrentProbabilityValue = OutcomeProbability.FromValue(newParentNodeOutcomeProbabilityValue);
            }
        }

        //calculator has to know the change (or just the new value) of the networks parent probability
        public void CalculateObservedProbabilitiesFromParentProbability(IList<IRandomVariableNode> childNodes)
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

                if (node.ChildNodes.Count > 0)
                    CalculateObservedProbabilitiesFromParentProbability(node.ChildNodes);
            }
        }

        //calculator has to know the change (or just the new value) of the networks parent probability
        public void CalculateOutcomeProbabilityForSpecificChildNodesOutcome(Outcome childNodeOutcome)
        {
            childNodeOutcome.MustNotBeNull(nameof(childNodeOutcome));

            if (childNodeOutcome.Node.ProbabilityKind() == OutcomeProbabilityKind.Evidence)
                return;

            float result = 0;
            var childNode = childNodeOutcome.Node;
            var childsParentNode = childNode.ParentNodes.First();

            foreach (var parentNodeOutcome in childsParentNode.Outcomes)
            {
                float childParentOutcomeProbabilityFromTable;
                var outcomeCombinationChildParentOutcome = new OutcomeCombination(parentNodeOutcome, childNodeOutcome);
                if (childNode.ProbabilityTable.TryGetValue(outcomeCombinationChildParentOutcome, out childParentOutcomeProbabilityFromTable) == false)
                    throw new Exception($"The child nodes {childNode} probability table does not contain a probability combination for outcomes {childNodeOutcome} (childNodeOutcome) and {parentNodeOutcome} (parentNodeOutcome).");
                result += parentNodeOutcome.CurrentProbabilityValue.Value * childParentOutcomeProbabilityFromTable;
            }

            childNodeOutcome.CurrentProbabilityValue = OutcomeProbability.FromValue(result);
        }


    }
}