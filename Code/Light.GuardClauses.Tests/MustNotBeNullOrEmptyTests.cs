using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests
{
    public sealed class MustNotBeNullOrEmptyTests
    {
        [Fact(DisplayName = "MustNotBeNullOrEmpty must throw an ArgumentNullException when the collection is null.")]
        public void ListNull()
        {
            List<string> list = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => list.MustNotBeNullOrEmpty(nameof(list));

            act.ShouldThrow<ArgumentNullException>()
               .And.ParamName.Should().Be(nameof(list));
        }

        [Fact(DisplayName = "MustNotBeNullOrEmpty must throw an EmptyCollectionException when the parameter is a collection with no items.")]
        public void ListEmpty()
        {
            var list = new List<int>();

            Action act = () => list.MustNotBeNullOrEmpty(nameof(list));

            act.ShouldThrow<EmptyCollectionException>()
               .And.ParamName.Should().Be(nameof(list));
        }

        [Theory(DisplayName = "MustNotBeNullOrEmpty must not throw an exception when the parameter is a collection with at least one item.")]
        [MemberData(nameof(ListNotEmptyTestData))]
        public void ListNotEmpty(List<int> list)
        {
            Action act = () => list.MustNotBeNullOrEmpty(nameof(list));

            act.ShouldNotThrow();
        }

        public static readonly TestData ListNotEmptyTestData =
            new[]
            {
                new object[] { new List<int> {1} },
                new object[] { new List<int> {1, 2, 3} },
                new object[] { new List<int> {10, -11, 187, 22557} }
            };

        [Fact(DisplayName = "MustNotBeNullOrEmpty must throw an ArgumentNullException when a string is null.")]
        public void StringNull()
        {
            string @string = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => @string.MustNotBeNullOrEmpty(nameof(@string));

            act.ShouldThrow<ArgumentNullException>()
               .And.ParamName.Should().Be(nameof(@string));
        }

        [Fact(DisplayName = "MustNotBeNullOrEmpty must throw an EmptyStringException when the parameter is empty.")]
        public void EmptyString()
        {
            var @string = string.Empty;

            Action act = () => @string.MustNotBeNullOrEmpty(nameof(@string));

            act.ShouldThrow<EmptyStringException>()
               .And.ParamName.Should().Be(nameof(@string));
        }

        [Theory(DisplayName = "MustNotBeNullOrEmpty must not throw an excpetion when a proper string reference is handled.")]
        [InlineData("abc")]
        [InlineData("Hello World")]
        public void ProperString(string @string)
        {
            Action act = () => @string.MustNotBeNullOrEmpty(nameof(@string));

            act.ShouldNotThrow();
        }
    }
}
