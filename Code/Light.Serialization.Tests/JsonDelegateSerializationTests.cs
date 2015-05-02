using FluentAssertions;
using Light.Serialization.Json;
using System;
using Xunit;

namespace Light.Serialization.Tests
{
    public sealed class JsonDelegateSerializationTests
    {
        [Fact]
        public void ExceptionMustBeThrownWhenDelegateIsSerialized()
        {
            var testTarget = new JsonSerializerBuilder().Build();
            var numberOfCalls = 0;
            var action = new Action(() => numberOfCalls++);

            Action act = () => testTarget.Serialize(action);

            act.ShouldThrow<SerializationException>().And.Message.Should().Be(string.Format("Type {0} cannot be serialized.", action.GetType()));
        }
    }
}
