using FluentAssertions;
using System;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    public sealed class MustHaveValueTests
    {
        [Fact(DisplayName = "MustHaveValue must throw an exception when the specified Nullable<T> has no value.")]
        public void HasNoValue()
        {
            DateTime? value = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => value.MustHaveValue(nameof(value));

            act.ShouldThrow<NullableHasNoValueException>()
               .And.ParamName.Should().Be(nameof(value));
        }

        [Fact(DisplayName = "MustHaveValue must not throw an exception when the specified Nullable<T> has a value.")]
        public void HasValue()
        {
            int? value = 42;

            Action act = () => value.MustHaveValue(nameof(value));

            act.ShouldNotThrow();
        }
    }
}
