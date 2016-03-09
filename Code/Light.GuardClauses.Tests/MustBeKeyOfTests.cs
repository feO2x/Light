﻿using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests
{
    public sealed class MustBeKeyOfTests
    {
        [Theory(DisplayName = "MustBeKeyOf must throw an exception when the specified value is not within the keys of the given dictionary.")]
        [MemberData(nameof(NotInItemsTestData))]
        public void NotInItems(string value, Dictionary<string, string> dictionary, string itemsString)
        {
            Action act = () => value.MustBeKeyOf(dictionary, nameof(value));

            act.ShouldThrow<ArgumentOutOfRangeException>()
               .And.Message.Should().Contain($"{nameof(value)} must be one of the dictionary keys ({itemsString}), but you specified {value}.");
        }

        public static readonly TestData NotInItemsTestData =
            new[]
            {
                new object[] { "A", new Dictionary<string, string> { ["B"] = "Hello", ["C"] = "World" }, "B, C" },
                new object[] { "42", new Dictionary<string, string> { ["1"] = "a", ["2"] = "y" }, "1, 2" }
            };

        [Theory(DisplayName = "MustBeKeyOf must not throw an exception when the specified value is one of the keys in the given dictionary.")]
        [MemberData(nameof(InItemsTestData))]
        public void InItems(int value, Dictionary<int, string> dictionary)
        {
            Action act = () => value.MustBeKeyOf(dictionary, nameof(value));

            act.ShouldNotThrow();
        }

        public static readonly TestData InItemsTestData =
            new[]
            {
                new object[] { 42, new Dictionary<int, string> { [42] = "Hey" } },
                new object[] { 2, new Dictionary<int, string> { [1] = "Hello", [2] = "World" } }
            };
    }
}