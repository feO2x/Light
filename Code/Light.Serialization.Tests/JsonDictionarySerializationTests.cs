using System.Collections.Generic;
using Light.Serialization.FrameworkExtensions;
using Xunit;

namespace Light.Serialization.Tests
{
    public sealed class JsonDictionarySerializationTests : BaseDefaultJsonSerializerTest
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
                    "{\"1\":87,\"2\":88,\"3\":89}"
                },
                new object[]
                {
                    new Dictionary<string, DummyPerson>
                    {
                        { "Walter", new DummyPerson { Name = "Walter White", Age = 52 } },
                        { "Jesse", new DummyPerson { Name = "Jesse Pinkman", Age = 27 } }
                    },
                    "{\"Walter\":{\"name\":\"Walter White\",\"age\":52},\"Jesse\":{\"name\":\"Jesse Pinkman\",\"age\":27}}"
                },
                new object[]
                {
                    new Dictionary<DummyPerson, string>
                    {
                        { new DummyPerson { Name = "Foo", Age = 34 }, "Bla" },
                        { new DummyPerson { Name = "Blubb", Age = 55 }, "Xoi" }
                    },
                    string.Format("{{\"{0}\":\"Bla\",\"{0}\":\"Xoi\"}}",
                                  typeof (DummyPerson).FullName.MakeFirstCharacterLowercase())
                }
            };
    }
}