using System;
using Light.BayesianNetwork.Calculators;
using Light.GuardClauses;

namespace Light.BayesianNetwork.NaiveBayes
{
    public class NaiveBayesNetworkBuilder
    {
        private Guid _guid = Guid.NewGuid();
        private ICollectionFactory _collectionFactory = new DefaultCollectionFactory();
        private IReasonerFactory _reasonerFactory = new NaiveBayesReasonerFactory();
        private IProbabilityCalculatorFactory _probabilityCalculatorFactory = new DotNetProbabilityCalculatorFactory();

        public NaiveBayesNetworkBuilder WithGuid(Guid guid)
        {
            _guid = guid;
            return this;
        }

        public NaiveBayesNetworkBuilder WithCollectionFactory(ICollectionFactory collectionFactory)
        {
            collectionFactory.MustNotBeNull(nameof(collectionFactory));

            _collectionFactory = collectionFactory;
            return this;
        }

        public NaiveBayesNetworkBuilder WithReasonerFactory(IReasonerFactory reasonerFactory)
        {
            reasonerFactory.MustNotBeNull(nameof(reasonerFactory));

            _reasonerFactory = reasonerFactory;
            return this;
        }

        public NaiveBayesNetworkBuilder WithProbabilityCalculatorFactory(
            IProbabilityCalculatorFactory probabilityCalculatorFactory)
        {
            probabilityCalculatorFactory.MustNotBeNull(nameof(probabilityCalculatorFactory));

            _probabilityCalculatorFactory = probabilityCalculatorFactory;
            return this;
        }

        public BayesianNetwork Build()
        {
            return new BayesianNetwork(_guid, _collectionFactory, _reasonerFactory, _probabilityCalculatorFactory);
        }
    }
}
