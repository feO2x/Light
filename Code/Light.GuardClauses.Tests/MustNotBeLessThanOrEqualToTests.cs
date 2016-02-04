using FluentAssertions;
using System;
using Xunit;

namespace Light.GuardClauses.Tests
{
    public sealed class MustNotBeLessThanOrEqualToTests
    {
        [Theory(DisplayName = "MustNotBeLessThanOrEqualTo must throw an exception when the specified value is below or equal to the given boundary.")]
        [InlineData(-1, 0)]
        [InlineData(0, 0)]
        [InlineData(-80, -40)]
        [InlineData("a", "b")]
        [InlineData("b", "b")]
        [InlineData("a", "Z")]
        public void ParameterAtOrBelowBoundary<T>(T value, T boundary) where T : IComparable<T>
        {
            Action act = () => value.MustNotBeLessThanOrEqualTo(boundary, nameof(value));

            act.ShouldThrow<ArgumentOutOfRangeException>()
               .And.Message.Should().Contain($"{nameof(value)} must not be less than or equal to {boundary}, but you specified {value}.");
        }

        [Theory(DisplayName = "MustNotBeLessThanOrEqualTo must not throw an exception when the specified value is above the given boundary.")]
        [InlineData(5, 4)]
        [InlineData(100, 1)]
        [InlineData(-87, -90)]
        [InlineData("A", "a")]
        [InlineData("b", "a")]
        public void ParamterAboveBoundary<T>(T value, T boundary) where T : IComparable<T>
        {
            Action act = () => value.MustNotBeLessThanOrEqualTo(boundary, nameof(value));

            act.ShouldNotThrow();
        }
    }
}
