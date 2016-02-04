﻿using FluentAssertions;
using System;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests
{
    public sealed class MustNotBeOneOfTests
    {
        [Theory(DisplayName = "MustNotBeOneOf must throw an exception when the specified value is within the given items.")]
        [MemberData(nameof(ParameterWithinItemsTestData))]
        public void ParameterWithinItems<T>(T value, T[] items, string itemsString)
        {
            Action act = () => value.MustNotBeOneOf(items, nameof(value));

            act.ShouldThrow<ArgumentOutOfRangeException>()
               .And.Message.Should().Contain($"{nameof(value)} must be none of the items ({itemsString}), but you specified {value}.");
        }

        public static readonly TestData ParameterWithinItemsTestData =
            new[]
            {
                new object[] {'a', new [] {'a', 'b', 'c'}, "a, b, c"},
                new object[] {1, new [] {81, 1, -55}, "81, 1, -55"}
            };

        [Theory(DisplayName = "MustNotBeOneOf must not throw an exception when the specified value is not one of the given items.")]
        [MemberData(nameof(InItemsTestData))]
        public void ParameterOutOfItems<T>(T value, T[] items)
        {
            Action act = () => value.MustNotBeOneOf(items, nameof(value));

            act.ShouldNotThrow();
        }

        public static readonly TestData InItemsTestData =
            new[]
            {
                new object[] { 'X', new[] { 'S', 'T', 'U', 'F', 'F' } },
                new object[] { -3, new[] { 41, 42, 43 } }
            };
    }
}
