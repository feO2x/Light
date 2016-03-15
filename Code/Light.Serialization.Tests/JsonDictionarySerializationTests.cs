﻿using System.Collections.Generic;
using Xunit;

namespace Light.Serialization.Tests
{
    public sealed class JsonDictionarySerializationTests : BaseJsonSerializerTest
    {
        [Theory]
        [MemberData("DictionaryTestData")]
        public void DictionariesMustBeSerializedCorrectly(object dictionary, string expected)
        {
            CompareJsonToExpected(dictionary, expected);
        }

        public static readonly IEnumerable<object[]> DictionaryTestData =
            new[]
            {
                new object[]
                {
                    new Dictionary<int, int> { { 1, 87 }, { 2, 88 }, { 3, 89 } },
                    "{\"$id\":0,\"1\":87,\"2\":88,\"3\":89}"
                },
                new object[]
                {
                    new Dictionary<string, DummyPerson>
                    {
                        { "Walter", new DummyPerson { Name = "Walter White", Age = 52 } },
                        { "Jesse", new DummyPerson { Name = "Jesse Pinkman", Age = 27 } }
                    },
                    "{\"$id\":0,\"walter\":{\"$id\":1,\"name\":\"Walter White\",\"age\":52},\"$id\":3,\"jesse\":{\"$id\":4,\"name\":\"Jesse Pinkman\",\"age\":27}}"
                },
                new object[]
                {
                    new Dictionary<DummyPerson, string>
                    {
                        { new DummyPerson { Name = "Foo", Age = 34 }, "Bla" },
                        { new DummyPerson { Name = "Blubb", Age = 55 }, "Xoi" }
                    },
                    string.Format("{{\"$id\":0,\"{0}\":\"Bla\",\"{0}\":\"Xoi\"}}",
                                  typeof (DummyPerson).FullName)
                }
            };
    }
}