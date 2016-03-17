using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    public sealed class MustBeNullTests
    {
        [Fact(DisplayName = "MustBeNull throws an exception when the specified value is not null.")]
        public void ArgumentNotNull()
        {
            var @string = "Hey";

            Action act = () => @string.MustBeNull(nameof(@string));

            act.ShouldThrow<ArgumentNotNullException>()
               .And.ParamName.Should().Be(nameof(@string));
        }

        [Fact(DisplayName = "MustBeNull must not throw an exception when the specified value is null.")]
        public void ArgumentNull()
        {
            object @object = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => @object.MustBeNull(nameof(@object));

            act.ShouldNotThrow();
        }

        [Fact(DisplayName = "The exception thrown by MustBeNull must contain the message if it is specified by the caller.")]
        public void SpecifyMessage()
        {
            var @object = new object();
            const string message = "Thou shall be null!";

            Action act = () => @object.MustBeNull(message: message);

            act.ShouldThrow<ArgumentNotNullException>()
               .And.Message.Should().Be(message);
        }

        [Fact(DisplayName = "MustBeNull must throw the specified exception when the corresponding overload is called.")]
        public void SpecifyException()
        {
            var @object = new object();
            var exception = new Exception();

            Action act = () => @object.MustBeNull(() => exception);

            act.ShouldThrow<Exception>().Which.Should().Be(exception);
        }
    }
}