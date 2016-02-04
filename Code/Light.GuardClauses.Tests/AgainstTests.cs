using FluentAssertions;
using System;
using Xunit;

namespace Light.GuardClauses.Tests
{
    public sealed class AgainstTests
    {
        [Fact(DisplayName = "Against must execute exception delegate when condition returns true.")]
        public void DelegateExecutesOnTrue()
        {
            Action act = () => Guard.Against(true, () => new Exception());

            act.ShouldThrow<Exception>();
        }

        [Fact(DisplayName = "Against must not execute exception delegate when condition returns false.")]
        public void DelegateDoesNotExecuteOnFalse()
        {
            Action act = () => Guard.Against(false, () => new Exception());

            act.ShouldNotThrow();
        }
    }
}
