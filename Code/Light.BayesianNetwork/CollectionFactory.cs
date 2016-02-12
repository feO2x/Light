using System.Collections.Generic;

namespace Light.BayesianNetwork
{
    public class CollectionFactory
    {
        public virtual IList<T> CreateList<T>()
        {
            return new List<T>();
        }

        public virtual IDictionary<TKey, TValue> CreateDictionary<TKey, TValue>()
        {
            return new Dictionary<TKey, TValue>();
        }
    }
}