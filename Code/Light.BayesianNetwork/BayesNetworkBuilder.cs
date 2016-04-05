using System;
using Light.BayesianNetwork.Calculators;
using Light.BayesianNetwork.NaiveBayes;
using Light.GuardClauses;

namespace Light.BayesianNetwork
{
    public class BayesNetworkBuilder : IBayesNetworkBuilder
    {
        private Guid _guid = Guid.NewGuid();
        private ICollectionFactory _collectionFactory = new DefaultCollectionFactory();
        private IReasonerFactory _reasonerFactory = new BayesReasonerNullObjectFactory();
        private IProbabilityCalculatorFactory _probabilityCalculatorFactory = new DotNetProbabilityCalculatorFactory();


        public IBayesNetworkBuilder WithCollectionFactory(ICollectionFactory collectionFactory)
        {
            collectionFactory.MustNotBeNull(nameof(collectionFactory));

            _collectionFactory = collectionFactory;
            return this;
        }

        public IBayesNetworkBuilder WithGuid(Guid guid)
        {
            _guid = guid;
            return this;
        }

        public IBayesNetworkBuilder WithProbabilityCalculatorFactory(IProbabilityCalculatorFactory probabilityCalculatorFactory)
        {
            probabilityCalculatorFactory.MustNotBeNull(nameof(probabilityCalculatorFactory));

            _probabilityCalculatorFactory = probabilityCalculatorFactory;
            return this;
        }

        public IBayesNetworkBuilder WithReasonerFactory(IReasonerFactory reasonerFactory)
        {
            reasonerFactory.MustNotBeNull(nameof(reasonerFactory));

            _reasonerFactory = reasonerFactory;
            return this;
        }

        public BayesianNetwork Build()
        {
            return new BayesianNetwork(_guid, _collectionFactory, _reasonerFactory, _probabilityCalculatorFactory);
        }
    }
}