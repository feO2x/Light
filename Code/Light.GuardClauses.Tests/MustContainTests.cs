using FluentAssertions;
using Light.GuardClauses.Exceptions;
using System;
using Xunit;

namespace Light.GuardClauses.Tests
{
    public sealed class MustContainTests
    {
        [Theory(DisplayName = "MustContain must throw a StringException when the specified text is not part of the given string.")]
        [InlineData("abc", "d")]
        [InlineData("Hello, World!", "You")]
        [InlineData("1, 2, 3", ". ")]
        public void StringDoesNotContainText(string value, string containedText)
        {
            Action act = () => value.MustContain(containedText, nameof(value));

            act.ShouldThrow<StringException>()
               .And.Message.Should().Contain($"{nameof(value)} must contain the text \"{containedText}\", but you specified \"{value}\".");
        }

        [Theory(DisplayName = "MustContain must not throw an exception when the text is contained.")]
        [InlineData("abc", "a")]
        [InlineData("Hello, World!", "orl")]
        [InlineData("1, 2, 3", ", ")]
        public void StringContainsText(string value, string containedText)
        {
            Action act = () => value.MustContain(containedText, nameof(value));

            act.ShouldNotThrow();
        }
    }
}