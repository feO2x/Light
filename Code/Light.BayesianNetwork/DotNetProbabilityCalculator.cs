using System;
using System.Collections.Generic;
using System.Linq;

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
                    CalculateOutcomeProbabilityUpdates(outcome);
                }
            }
        }

        //calculator has to know the change (or just the new value) of the networks parent probability
        private void CalculateOutcomeProbabilityUpdates(Outcome outcome)
        {
            //var outcomeProbability = new OutcomeProbability();
            throw new NotImplementedException();
        }
    }
}