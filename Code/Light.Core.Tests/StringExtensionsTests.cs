using FluentAssertions;
using Xunit;

namespace Light.Core.Tests
{
    public sealed class StringExtensionsTests
    {
        [Fact]
        public void SurroundWithQuotationMarksWorksCorrectly()
        {
            var actual = "Foo".SurroundWithQuotationMarks();

            const string expected = "\"Foo\"";
            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData("!", "!Foo!")]
        [InlineData("", "Foo")]
        [InlineData(".", ".Foo.")]
        [InlineData("ab", "abFooab")]
        public void SurroundsWithWorksCorrectly(string surroundCharacters, string expected)
        {
            var actual = "Foo".SurroundWith(surroundCharacters);

            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData("foo", false)]
        [InlineData("\"foo\"", true)]
        [InlineData("\"Foo", false)]
        [InlineData("Foo\"", false)]
        [InlineData("a", false)]
        [InlineData("ab", false)]
        [InlineData("\"a", false)]
        [InlineData("b\"", false)]
        public void IsSurroundedByQuotationMarksWorksCorrectly(string @string, bool expected)
        {
            var actual = @string.IsSurroundedByQuotationMarks();

            actual.Should().Be(expected);
        }
    }
}
