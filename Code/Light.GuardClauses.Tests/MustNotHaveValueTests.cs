using FluentAssertions;
using System;
using Xunit;

namespace Light.GuardClauses.Tests
{
    public sealed class MustNotHaveValueTests
    {
        [Fact(DisplayName = "MustNotHaveValue must throw an exception when the specified Nullable<T> has a value.")]
        public void HasValue()
        {
            DateTime? value = DateTime.Today;

            Action act = () => value.MustNotHaveValue(nameof(value));

            act.ShouldThrow<NullableHasValueException>()
               .And.ParamName.Should().Be(nameof(value));
        }

        [Fact(DisplayName = "MustHaveNoValue must not throw an exception when the specified Nullable<T> is null.")]
        public void HasNoValue()
        {
            double? value = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => value.MustNotHaveValue(nameof(value));

            act.ShouldNotThrow();
        }
    }
}
