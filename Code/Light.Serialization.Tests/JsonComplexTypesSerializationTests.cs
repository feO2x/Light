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
                                                                    string.Format("{{{0}:42,{1}:\"Foo\"}}",
                                                                                  "Int".SurroundWithQuotationMarks(),
                                                                                  "String".SurroundWithQuotationMarks())
                                                                },
                                                                new object[]
                                                                {
                                                                    new ClassWithPublicFieldAndPublicAndPrivateProperties("Foo", 42.7),
                                                                    string.Format("{{{0}:42.7,{1}:\"Foo\"}}",
                                                                                  "Value".SurroundWithQuotationMarks(),
                                                                                  "StringField".SurroundWithQuotationMarks())
                                                                }
                                                            };
    }
}