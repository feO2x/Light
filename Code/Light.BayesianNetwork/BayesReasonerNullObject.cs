namespace Light.BayesianNetwork
{
    public class BayesReasonerNullObject : IReasoner
    {
        public BayesReasonerNullObject(BayesianNetwork network, IProbabilityCalculator probabilityCalculator)
        {
            return;
        }

        public void PropagateNewEvidence(Outcome childNode)
        {
            return;
        }
    }
}