﻿using Light.GuardClauses;

namespace Light.Serialization.Json.ComplexTypeDecomposition
{
    public sealed class ToLowerStringNormalizer : IInjectableValueNameNormalizer
    {
        public string Normalize(string name)
        {
            name.MustNotBeNull(nameof(name));

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var character in name)
            {
                if (char.IsLower(character))
                    continue;

                return name.ToLower();
            }

            return name;
        }
    }
}