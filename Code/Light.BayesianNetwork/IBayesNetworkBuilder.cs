using System;

namespace Light.BayesianNetwork
{
    public interface IBayesNetworkBuilder
    {
        BayesianNetwork Build();
        IBayesNetworkBuilder WithCollectionFactory(ICollectionFactory collectionFactory);
        IBayesNetworkBuilder WithGuid(Guid guid);
        IBayesNetworkBuilder WithProbabilityCalculatorFactory(IProbabilityCalculatorFactory probabilityCalculatorFactory);
        IBayesNetworkBuilder WithReasonerFactory(IReasonerFactory reasonerFactory);
    }
}