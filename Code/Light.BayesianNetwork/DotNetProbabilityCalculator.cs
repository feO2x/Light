using System;
using System.Collections.Generic;
using System.Linq;
using Light.GuardClauses;

namespace Light.BayesianNetwork
{
    public class DotNetProbabilityCalculator : IProbabilityCalculator
    {
        public void CalculateParentProbabilityFromEvidence(RandomVariableNode node)
        {
            throw new System.NotImplementedException();
        }

        //calculator has to know the change (or just the new value) of the networks parent probability
        public void CalculateObservedProbabilitiesFromParentProbability(IReadOnlyList<RandomVariableNode> childNodes)
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
        public double CalculateOutcomeProbabilityForSpecificOutcome(Outcome outcome)
        {
            outcome.MustNotBeNull(nameof(outcome));

            throw new NotImplementedException();
        }


    }
}