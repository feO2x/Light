using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Light.Serialization.Json;
using Light.Serialization.Json.LowLevelWriting;

namespace Light.Serialization.Tests
{
    public abstract class BaseNormalizedJsonKeysSerializerTest
    {
        protected readonly ISerializer JsonSerializer;

        protected BaseNormalizedJsonKeysSerializerTest()
        {
            JsonSerializer = new JsonSerializerBuilder().WithWriterFactory(new JsonWriterNormalizedKeysDecoratorFactory())
                                                        .Build();
        }

        protected void CompareJsonToExpected<T>(T value, string expected)
        {
            var json = JsonSerializer.Serialize(value);

            json.Should().Be(expected);
        }
    }
}
