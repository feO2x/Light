using System.Collections.Generic;
using Light.GuardClauses;

namespace Light.BayesianNetwork
{
    public static class CollectionFactoryExtensions
    {
        public static void InitializeListFields<T>(this ICollectionFactory collectionFactory, out IList<T> listField, out IReadOnlyList<T> readOnlyListField)
        {
            collectionFactory.MustNotBeNull(nameof(collectionFactory));

            listField = collectionFactory.CreateList<T>();
            readOnlyListField = (IReadOnlyList<T>) listField;
        }
    }
}