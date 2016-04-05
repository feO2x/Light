namespace Light.BayesianNetwork
{
    public class BayesReasonerNullObjectFactory : IReasonerFactory
    {
        public IReasoner Create(BayesianNetwork network, IProbabilityCalculator probabilityCalculator)
        {
            return new BayesReasonerNullObject(null, null);
        }
    }
}