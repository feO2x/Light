using System;
using FluentAssertions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests
{
    public sealed class MustNotBeNullTests
    {
        [Fact(DisplayName = "MustNotBeNull must throw an exception when null is provided.")]
        public void NullIsGiven()
        {
            Action act = () => DummyMethod<string>(null);

            act.ShouldThrow<ArgumentNullException>()
               .And.ParamName.Should().Be("someObject");
        }

        [Theory(DisplayName = "MustNotBeNull must not throw an exception when the specified reference is not null.")]
        [MemberData(nameof(ObjectReferenceTestData))]
        public void ValidObjectReferenceIsGiven<T>(T value) where T : class
        {
            Action act = () => DummyMethod(value);

            act.ShouldNotThrow();
        }

        public static readonly TestData ObjectReferenceTestData =
            new[]
            {
                new object[] { string.Empty },
                new[] { new object() }
            };

        [Fact(DisplayName = "The exception thrown by MustNotBeNull must contain the message if it is specified by the caller.")]
        public void SpecifyMessage()
        {
            object someObject = null;
            const string message = "Thou shall not be null!";

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => someObject.MustNotBeNull(message: message);

            act.ShouldThrow<ArgumentNullException>()
               .And.Message.Should().Be(message);
        }

        [Fact(DisplayName = "MustNotBeNull must raise the specified exception when the corresponding overload is called.")]
        public void SpecifyException()
        {
            object someObject = null;
            var exception = new Exception();

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => someObject.MustNotBeNull(() => exception);

            act.ShouldThrow<Exception>().Which.Should().Be(exception);
        }

        private static void DummyMethod<T>(T someObject) where T : class
        {
            someObject.MustNotBeNull(nameof(someObject));
        }
    }
}