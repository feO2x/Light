using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Light.GuardClauses
{
    public static class CollectionGuardClauses
    {
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustBeOneOf<T>(this T parameter, IReadOnlyList<T> items, string parameterName)
        {
            if (items.Contains(parameter))
                return;

            var stringBuilder = new StringBuilder().AppendItems(items);
            throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must be one of the items ({stringBuilder}), but you specified {parameter}.");
        }

        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotBeOneOf<T>(this T parameter, IReadOnlyList<T> items, string parameterName)
        {
            if (items.Contains(parameter) == false)
                return;

            var stringBuilder = new StringBuilder().AppendItems(items);
            throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must be none of the items ({stringBuilder}), but you specified {parameter}.");
        }

        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotBeNullOrEmpty<T>(this IReadOnlyCollection<T> collection, string parameterName)
        {
            if (collection == null)
                throw new ArgumentNullException(parameterName);

            if (collection.Count == 0)
                throw new EmptyCollectionException(parameterName);
        }

        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustHaveUniqueItems<T>(this IReadOnlyList<T> parameter, string parameterName)
        {
            for (var i = 0; i < parameter.Count; i++)
            {
                var itemToCompare = parameter[i];
                for (var j = i + 1; j < parameter.Count; j++)
                {
                    if (!itemToCompare.EqualsWithHashCode(parameter[j]))
                        continue;

                    var stringBuilder = new StringBuilder().AppendItems(parameter);
                    throw new CollectionException($"{parameterName} must be a collection with unique items, but you specified {stringBuilder}.", parameterName);
                }
            }
        }

        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustBeKeyOf<TKey, TValue>(this TKey parameter, IDictionary<TKey, TValue> dictionary, string parameterName)
        {
            if (dictionary.ContainsKey(parameter))
                return;

            var stringBuilder = new StringBuilder().AppendItems(dictionary.Keys.ToList());
            throw new ArgumentOutOfRangeException(parameterName, parameter, $"{parameterName} must be one of the dictionary keys ({stringBuilder}), but you specified {parameter}.");
        }
    }
}