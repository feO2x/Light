using System;
using FluentAssertions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

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

        [Theory(DisplayName = "IsNotLessThan must throw an exception when the specified int value is below the given boundary.")]
        [InlineData(2, 5)]
        [InlineData(int.MinValue, int.MaxValue)]
        [InlineData(-80, 0)]
        public void IntParameterBelowBoundary(int value, int boundary)
        {
            Action act = () => value.IsNotLessThan(boundary, nameof(value));

            act.ShouldThrow<ArgumentOutOfRangeException>()
               .And.Message.Should().Contain($"{nameof(value)} must not be less than {boundary}, but you specified {value}.");
        }

        [Theory(DisplayName = "IsNotLessThan must not throw an exception when the specified int value is equal to or greater than the given boundary.")]
        [InlineData(4, 3)]
        [InlineData(int.MaxValue, int.MaxValue)]
        [InlineData(0, 0)]
        [InlineData(int.MinValue, int.MinValue)]
        [InlineData(82, 40)]
        public void IntParameterAtOrAboveBoundary(int value, int boundary)
        {
            Action act = () => value.IsNotLessThan(boundary, nameof(value));

            act.ShouldNotThrow();
        }

        [Theory(DisplayName = "IsNotLessThan must throw an exception when the specified double value is below the given boundary.")]
        [InlineData(2.9, 3.0)]
        [InlineData(double.MinValue, double.MaxValue)]
        [InlineData(-80.9976, -80.9975)]
        public void DoubleParameterBelowBoundary(double value, double boundary)
        {
            Action act = () => value.IsNotLessThan(boundary, nameof(value));

            act.ShouldThrow<ArgumentOutOfRangeException>()
               .And.Message.Should().Contain($"{nameof(value)} must not be less than {boundary}, but you specified {value}.");
        }

        [Theory(DisplayName = "IsNotLessThan must not throw an exception when the specified double value is equal to or greater than the given boundary.")]
        [InlineData(4.3, 4.2)]
        [InlineData(double.MaxValue, double.MaxValue)]
        [InlineData(0.0, 0.0)]
        [InlineData(double.MinValue, double.MinValue)]
        [InlineData(82.7577, 82.7576)]
        public void DoubleParameterAtOrAboveBoundary(double value, double boundary)
        {
            Action act = () => value.IsNotLessThan(boundary, nameof(value));

            act.ShouldNotThrow();
        }

        [Theory(DisplayName = "IsNotLessThan must throw an exception when the specified decimal value is below the given boundary.")]
        [MemberData(nameof(DecimalParameterBelowBoundaryData))]
        public void DecimalParameterBelowBoundary(decimal value, decimal boundary)
        {
            Action act = () => value.IsNotLessThan(boundary, nameof(value));

            act.ShouldThrow<ArgumentOutOfRangeException>()
               .And.Message.Should().Contain($"{nameof(value)} must not be less than {boundary}, but you specified {value}.");
        }

        public static readonly TestData DecimalParameterBelowBoundaryData =
            new []
            {
                new object[] {decimal.Zero, decimal.One},
                new object[] {decimal.Parse("42.0000001"), decimal.Parse("42.0000002")},
                new object[] {decimal.Parse("-242.75"), decimal.Parse("0.556")}
            };

        [Theory(DisplayName = "IsNotLessThan must not throw an exception when the specified decimal value is equal to or greater than the given boundary.")]
        [MemberData(nameof(DecimalParameterAtOrAboveBoundaryData))]
        public void DecimalParameterAtOrAboveBoundary(decimal value, decimal boundary)
        {
            Action act = () => value.IsNotLessThan(boundary, nameof(value));

            act.ShouldNotThrow();
        }

        public static readonly TestData DecimalParameterAtOrAboveBoundaryData =
            new[]
            {
                new object[] {decimal.One, decimal.Zero},
                new object[] {decimal.Parse("42.5556"), decimal.Parse("42.5556")},
                new object[] {-874555.444, -874555.4442 },
                new object[] {decimal.MaxValue, decimal.MaxValue}
            };
    }
}
