using System;
using Light.BayesianNetwork.Calculators;
using Light.GuardClauses;

namespace Light.BayesianNetwork.NaiveBayes
{
    public class NaiveBayesBuilder
    {
        private Guid _guid = Guid.NewGuid();
        private ICollectionFactory _collectionFactory = new DefaultCollectionFactory();
        private IReasonerFactory _reasonerFactory = new NaiveBayesReasonerFactory();
        private IProbabilityCalculatorFactory _probabilityCalculatorFactory = new DotNetProbabilityCalculatorFactory();

        public NaiveBayesBuilder WithGuid(Guid guid)
        {
            _guid = guid;
            return this;
        }

        public NaiveBayesBuilder WithCollectionFactory(ICollectionFactory collectionFactory)
        {
            collectionFactory.MustNotBeNull(nameof(collectionFactory));

            _collectionFactory = collectionFactory;
            return this;
        }

        public NaiveBayesBuilder WithReasonerFactory(IReasonerFactory reasonerFactory)
        {
            reasonerFactory.MustNotBeNull(nameof(reasonerFactory));

            _reasonerFactory = reasonerFactory;
            return this;
        }

        public NaiveBayesBuilder WithProbabilityCalculatorFactory(
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
