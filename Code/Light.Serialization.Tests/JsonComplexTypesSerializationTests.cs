using Light.Core;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.Serialization.Tests
{
    public sealed class JsonComplexTypesSerializationTests : BaseDefaultJsonSerializerTest
    {
        [Theory]
        [MemberData("ComplexObjectData")]
        public void ComplexObjectIsSerializedCorrectly<T>(T complexObject, string expected)
        {
            CompareJsonToExpected(complexObject, expected);
        }

        public static readonly TestData ComplexObjectData = new[]
                                                            {
                                                                new object[]
                                                                {
                                                                    new ClassWithPublicPropertiesAndPrivateFields(42, "Foo"),
                                                                    $"{{{"Int".SurroundWithQuotationMarks()}:42,{"String".SurroundWithQuotationMarks()}:\"Foo\"}}"
                                                                },
                                                                new object[]
                                                                {
                                                                    new ClassWithPublicFieldAndPublicAndPrivateProperties("Foo", 42.7),
                                                                    $"{{{"Value".SurroundWithQuotationMarks()}:42.7,{"StringField".SurroundWithQuotationMarks()}:\"Foo\"}}"
                                                                }
                                                            };
    }
}