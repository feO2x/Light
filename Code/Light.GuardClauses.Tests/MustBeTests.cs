using FluentAssertions;
using System;
using Xunit;

namespace Light.GuardClauses.Tests
{
    public sealed class MustBeTests
    {
        [Theory(DisplayName = "MustBe must throw an exception when the specified value is not the expected one.")]
        [InlineData(42, 0)]
        [InlineData(true, false)]
        [InlineData("Hello", "World!")]
        public void ValuesNotEqual<T>(T value, T other)
        {
            Action act = () => value.MustBe(other, nameof(value));

            act.ShouldThrow<ArgumentException>()
               .And.Message.Should().Contain($"{nameof(value)} must be {other}, but you specified {value}");
        }

        [Theory(DisplayName = "MustBe must not throw an exception when the specified value is the same as the expected one.")]
        [InlineData(42)]
        [InlineData(55.89)]
        [InlineData("Hey")]
        public void ValuesEqual<T>(T value)
        {
            Action act = () => value.MustBe(value, nameof(value));

            act.ShouldNotThrow();
        }

        [Fact(DisplayName = "User can throw a custom exception.")]
        public void CustomException()
        {
            var exception = new InvalidOperationException("Foo");

            Action act = () => 42.MustBe(45, exception);

            act.ShouldThrowExactly<InvalidOperationException>().Which.Should().BeSameAs(exception);
        }
    }
}
