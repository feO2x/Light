using FluentAssertions;
using Light.GuardClauses.Exceptions;
using System;
using Xunit;

namespace Light.GuardClauses.Tests
{
    public sealed class MustNotContainTests
    {
        [Theory(DisplayName = "MustNotContain must throw an exception when the specified string contains the given text.")]
        [InlineData("abc", "b")]
        [InlineData("Say herro to my littre friend", "herro")]
        public void TextContained(string value, string containedText)
        {
            Action act = () => value.MustNotContain(containedText, nameof(value));

            act.ShouldThrow<StringException>()
               .And.Message.Should().Contain($"{nameof(value)} must not contain the text \"{containedText}\", but you specified \"{value}\".");
        }

        [Theory(DisplayName = "MustNotContain must not throw an exception when the specified string does not contain the given text.")]
        [InlineData("1, 2, 3", ".")]
        [InlineData("Say herro to my littre friend", "hello")]
        public void TextNotContained(string value, string containedText)
        {
            Action act = () => value.MustNotContain(containedText, nameof(value));

            act.ShouldNotThrow();
        }
    }
}
