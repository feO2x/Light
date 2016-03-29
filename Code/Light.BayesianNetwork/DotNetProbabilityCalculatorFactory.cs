namespace Light.BayesianNetwork
{
    public class DotNetProbabilityCalculatorFactory : IProbabilityCalculatorFactory
    {
        public IProbabilityCalculator Create()
        {
            return new DotNetProbabilityCalculator();
        }
    }
}