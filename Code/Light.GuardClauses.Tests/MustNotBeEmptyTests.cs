using FluentAssertions;
using Light.GuardClauses.Exceptions;
using System;
using Xunit;

namespace Light.GuardClauses.Tests
{
    public sealed class MustNotBeEmptyTests
    {
        [Fact(DisplayName = "MustNotBeEmpty must throw an exception when the specified GUID is empty.")]
        public void GuidEmpty()
        {
            var emptyGuid = Guid.Empty;

            Action act = () => emptyGuid.MustNotBeEmpty(nameof(emptyGuid));

            act.ShouldThrow<EmptyGuidException>()
               .And.ParamName.Should().Be(nameof(emptyGuid));
        }

        [Fact(DisplayName = "MustNotBeEmpty must not throw an exception when the specified GUID is not empty.")]
        public void GuidNotEmpty()
        {
            var validGuid = Guid.NewGuid();

            Action act = () => validGuid.MustNotBeEmpty(nameof(validGuid));

            act.ShouldNotThrow();
        }
    }
}
