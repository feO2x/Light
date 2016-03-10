using System;
using FluentAssertions;
using Xunit;

namespace Light.Serialization.Tests
{
    public sealed class JsonDelegateSerializationTests : BaseJsonSerializerTest
    {
        [Fact(DisplayName = "An exception is thrown when a delegate is serialized as delegates are currently not supported.")]
        public void ExceptionMustBeThrownWhenDelegateIsSerialized()
        {
            var numberOfCalls = 0;
            var action = new Action(() => numberOfCalls++);

            Action act = () => GetSerializedJson(action);

            act.ShouldThrow<SerializationException>()
               .And.Message.Should().Contain($"Type {action.GetType()} cannot be serialized because there is no IJsonWriterInstructor registered that can cover this type.");
        }
    }
}