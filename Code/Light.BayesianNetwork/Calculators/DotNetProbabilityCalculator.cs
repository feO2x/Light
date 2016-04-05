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
                var newParentNodeOutcomeProbabilityValue = parentNodeOutcome.CurrentProbability.Value / outcomeToSetEvidenceOn.CurrentProbability.Value * childsStandardOutcomeProbabilityValue;

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
                    CalculateOutcomeProbabilityForSpecificOutcome(outcome);
                }
            }
        }

        //calculator has to know the change (or just the new value) of the networks parent probability
        public void CalculateOutcomeProbabilityForSpecificOutcome(Outcome outcome)
        {
            outcome.MustNotBeNull(nameof(outcome));

            if (outcome.Node.ProbabilityKind() == OutcomeProbabilityKind.Evidence)
                return;

            double result = 0;

            foreach (var parentNodeOutcome in _network.NetworkParentNode.Outcomes)
            {
                result += parentNodeOutcome.CurrentProbability.Value * outcome.CurrentProbability.Value;
            }

            outcome.CurrentProbability = OutcomeProbability.FromValue(result);
        }


    }
}