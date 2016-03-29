namespace Light.BayesianNetwork
{
    public interface IReasoner
    {
        void PropagateNewEvidence(RandomVariableNode node);
    }
}
