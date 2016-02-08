﻿using FluentAssertions;
using Light.GuardClauses.Exceptions;
using System;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests
{
    public sealed class MustHaveUniqueItems
    {
        [Theory(DisplayName = "MustHaveUniqueItems must throw an exception when the collection has duplicates.")]
        [MemberData(nameof(DuplicateItemsTestData))]
        public void DuplicateItems<T>(T[] collection, string collectionItems)
        {
            Action act = () => collection.MustHaveUniqueItems(nameof(collection));

            act.ShouldThrow<CollectionException>()
               .And.Message.Should().Contain($"{nameof(collection)} must be a collection with unique items, but you specified {collectionItems}.");
        }

        public static readonly TestData DuplicateItemsTestData =
            new[]
            {
                new object[] { new[] { "1", "2", "3", "1" }, "1, 2, 3, 1" },
                new object[] { new object[] { 1, 42, 42, 87 }, "1, 42, 42, 87" },
                new object[] { new[] { "1", null, "1" }, "1, null, 1" }
            };

        [Theory(DisplayName = "MustHaveUniqueItems must not throw an exception when the items are unique.")]
        [MemberData(nameof(UniqueItemsTestData))]
        public void UniqueItems<T>(T[] collection)
        {
            Action act = () => collection.MustHaveUniqueItems(nameof(collection));

            act.ShouldNotThrow();
        }

        public static readonly TestData UniqueItemsTestData =
            new[]
            {
                new object[] { new [] { "41", "42", "43"} },
                new object[] { new object[] { 1, 2, 3, 4, 5 } }
            };
    }
}