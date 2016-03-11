using System;
using Xunit;

#pragma warning disable 169

namespace Light.Serialization.Tests
{
    public class JsonNormalizedKeysTests : BaseJsonSerializerTest
    {
        [Fact(DisplayName = "Json Keys will be normalized correctly with the JsonNormalizedKeyWriter.")]
        public void CustomObjectNormalizeJsonKeys()
        {
            AddRule<MoreComplexDummyClass>(r => r.IgnoreAll()
                                                 .ButProperty(o => o.PublicStringProperty)
                                                 .AndField(o => o.PublicField));
            var moreComplexDummyObject = new MoreComplexDummyClass("PrivateFieldValue", 11, new DateTime(2016, 2, 10), 42.0, "PublicStringProperty", "publicStringPropertyTwo");

            CompareJsonToExpected(moreComplexDummyObject, "{\"publicStringProperty\":\"PublicStringProperty\",\"publicField\":11}");
        }

        public class MoreComplexDummyClass
        {
            public static int PublicStaticField = 0;
            // ReSharper disable once NotAccessedField.Local
            private string _privateField;
            public int PublicField;

            public MoreComplexDummyClass(string privateFieldValue, int publicFieldValue, DateTime publicPropertyValue, double publicDoubleProperty, string publicStringProperty, string publicStringPropertyTwo)
            {
                _privateField = privateFieldValue;
                PublicField = publicFieldValue;
                PublicProperty = publicPropertyValue;
                PublicDoubleProperty = publicDoubleProperty;
                PublicStringProperty = publicStringProperty;
                PublicStringPropertyTwo = publicStringPropertyTwo;
            }

            public DateTime PublicProperty { get; }
            public double PublicDoubleProperty { get; set; }
            public string PublicStringProperty { get; set; }
            public string PublicStringPropertyTwo { get; set; }
        }
    }
}