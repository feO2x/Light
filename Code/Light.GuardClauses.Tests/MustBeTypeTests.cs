using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    public sealed class MustBeTypeTests
    {
        [Fact(DisplayName = "MustBeType must throw an exception when object cannot be downcasted.")]
        public void TypeMismatch()
        {
            object @object = "Hey";

            Action act = () => @object.MustBeType<Array>(nameof(@object));

            act.ShouldThrow<TypeMismatchException>()
               .And.Message.Should().Contain($"{nameof(@object)} is of type {typeof (string).FullName} and cannot be downcasted to {typeof (Array).FullName}.");
        }

        [Fact(DisplayName = "MustBeType must return the downcasted object if cast succeeds.")]
        public void TypeDowncasted()
        {
            var @string = "Hey";
            object @object = @string;

            var downcastedValue = @object.MustBeType<string>(nameof(@object));
            downcastedValue.Should().BeSameAs(@string);
        }
    }
}