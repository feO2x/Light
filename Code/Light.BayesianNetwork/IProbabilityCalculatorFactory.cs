namespace Light.BayesianNetwork
{
    public interface IProbabilityCalculatorFactory
    {
        IProbabilityCalculator Create();
    }
}