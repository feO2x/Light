using Light.Serialization.FrameworkExtensions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.Serialization.Tests
{
    public sealed class JsonComplexTypesSerializationTests : BaseJsonSerializerTest
    {
        [Theory]
        [MemberData("ComplexObjectData")]
        public void ComplexObjectIsSerializedCorrectly<T>(T complexObject, string expected)
        {
            CompareJsonToExpected(complexObject, expected);
        }

        public static readonly TestData ComplexObjectData =
            new[]
            {
                new object[]
                {
                    new ClassWithPublicPropertiesAndPrivateFields(42, "Foo"),
                    $"{{{"int".SurroundWithQuotationMarks()}:42,{"string".SurroundWithQuotationMarks()}:\"Foo\"}}"
                },
                new object[]
                {
                    new ClassWithPublicFieldAndPublicAndPrivateProperties("Foo", 42.7),
                    $"{{{"value".SurroundWithQuotationMarks()}:42.7,{"stringField".SurroundWithQuotationMarks()}:\"Foo\"}}"
                }
            };

        [Theory(DisplayName = "Serialize must produce a JSON document with an empty object when the target has no public properties or fields.")]
        [MemberData(nameof(EmptyObjectData))]
        public void EmptyObject<T>(T emptyObject)
        {
            CompareJsonToExpected(emptyObject, "{}");
        }

        public static readonly TestData EmptyObjectData =
            new[]
            {
                new object[] { new EmptyClass() },
                new object[] { new ClassWithPrivateFieldsOnly(42, "Hello!") }
            };

        public class EmptyClass
        {
        }

        public class ClassWithPrivateFieldsOnly
        {
            public ClassWithPrivateFieldsOnly(int value1, string value2)
            {
                _privateField1 = value1;
                _privateField2 = value2;
            }

            // ReSharper disable NotAccessedField.Local
            private int _privateField1;
            private string _privateField2;
            // ReSharper restore NotAccessedField.Local
        }
    }
}