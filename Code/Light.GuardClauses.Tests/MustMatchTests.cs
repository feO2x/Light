using FluentAssertions;
using Light.GuardClauses.Exceptions;
using System;
using System.Text.RegularExpressions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    public class MustMatchTests
    {
        [Fact(DisplayName = "MustMatch must throw an exception when the specified string does not match the regular expression.")]
        public void StringDoesNotMatch()
        {
            var pattern = new Regex(@"\d{5}");
            var @string = "12c45";

            Action act = () => @string.MustMatch(pattern, nameof(@string));

            act.ShouldThrow<StringDoesNotMatchException>()
               .And.ParamName.Should().Be(nameof(@string));
        }

        [Fact(DisplayName = "MustMatch must not throw an exception when the specified string matches the regular expression.")]
        public void StringMatches()
        {
            var pattern = new Regex(@"\w{5}");
            var @string = "abcde";

            Action act = () => @string.MustMatch(pattern, nameof(@string));

            act.ShouldNotThrow();
        }
    }
}
