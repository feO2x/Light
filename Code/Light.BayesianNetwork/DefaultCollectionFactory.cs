using System.Collections.Generic;

namespace Light.BayesianNetwork
{
    public sealed class DefaultCollectionFactory : ICollectionFactory
    {
        public IList<T> CreateList<T>()
        {
            return new List<T>();
        }

        public IDictionary<TKey, TValue> CreateDictionary<TKey, TValue>()
        {
            return new Dictionary<TKey, TValue>();
        }
    }
}