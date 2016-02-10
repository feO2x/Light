﻿using FluentAssertions;
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

            act.ShouldThrow<SerializationException>()
               .And.Message.Should().Contain($"Type {action.GetType()} cannot be serialized because there is no IJsonWriterInstructor registered that can cover this type.");
        }
    }
}