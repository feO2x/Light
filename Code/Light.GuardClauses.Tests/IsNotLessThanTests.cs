using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    public sealed class IsNotLessThanTests
    {
        [Theory(DisplayName = "IsNotLessThan must throw an exception when the specified value is below the given boundary.")]
        [InlineData("a", "b")]
        [InlineData("X", "Y")]
        [InlineData("a", "Z")]
        public void ParameterBelowBoundary(string value, string boundary)
        {
            Action act = () => value.IsNotLessThan(boundary, nameof(value));

            act.ShouldThrow<ArgumentOutOfRangeException>()
               .And.Message.Should().Contain($"{nameof(value)} must not be less than {boundary}, but you specified {value}.");
        }

        [Theory(DisplayName = "IsNotLessThan must not throw an exception when the specified value is equal to or greater than the given boundary.")]
        [InlineData("b", "a")]
        [InlineData("b", "b")]
        [InlineData("X", "f")]
        public void ParameterAtOrAboveBoundary(string value, string boundary)
        {
            Action act = () => value.IsNotLessThan(boundary, nameof(value));

            act.ShouldNotThrow();
        }
    }
}
