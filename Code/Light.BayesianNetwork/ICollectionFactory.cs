using System.Collections.Generic;

namespace Light.BayesianNetwork
{
    public interface ICollectionFactory
    {
        IList<T> CreateList<T>();

        IDictionary<TKey, TValue> CreateDictionary<TKey, TValue>();
    }
}