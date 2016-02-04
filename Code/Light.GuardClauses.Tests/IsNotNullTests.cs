using FluentAssertions;
using System;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests
{
    public sealed class IsNotNullTests
    {
        [Fact(DisplayName = "IsNotNull must throw an exception when null is provided.")]
        public void NullIsGiven()
        {
            Action act = () => DummyMethod<string>(null);

            act.ShouldThrow<ArgumentNullException>()
               .And.ParamName.Should().Be("someObject");
        }

        [Theory(DisplayName = "IsNotNull must throw no exception when the specified reference is not null.")]
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

        private static void DummyMethod<T>(T someObject) where T : class
        {
            someObject.IsNotNull(nameof(someObject));
        }
    }
}
