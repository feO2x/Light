using FluentAssertions;
using Light.GuardClauses.Exceptions;
using System;
using Xunit;

namespace Light.GuardClauses.Tests
{
    public sealed class MustBeValidEnumValueTests
    {
        [Fact(DisplayName = "MustBeValidEnumValue must throw an exception when the specified value is not within the defined values of the enumeration.")]
        public void InvalidEnumValue()
        {
            const ConsoleSpecialKey invalidValue = (ConsoleSpecialKey) 15;

            Action act = () => invalidValue.MustBeValidEnumValue(nameof(invalidValue));

            act.ShouldThrow<EnumValueNotDefinedException>()
               .And.ParamName.Should().Be(nameof(invalidValue));
        }

        [Fact(DisplayName = "MustBeValidEnumValue must not throw an exception when the specified value is within the defined values of the enumeration.")]
        public void ValidEnumValue()
        {
            const ConsoleColor validValue = ConsoleColor.DarkRed;

            Action act = () => validValue.MustBeValidEnumValue(nameof(validValue));

            act.ShouldNotThrow();
        }
    }
}
