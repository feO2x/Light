using System;
using FluentAssertions;
using Light.Serialization.Json;
using Xunit;

namespace Light.Serialization.Tests
{
    public class JsonNormalizedKeysTests : BaseDefaultJsonSerializerTest
    {
        [Fact(DisplayName = "Json Keys will be normalized correctly with the JsonNormalizedKeyWriter.")]
        public void CustomObjectNormalizeJsonKeys()
        {
            var moreComplexDummyObject = new MoreComplexDummyClass("PrivateFieldValue", 11, new DateTime(2016, 2, 10), 42.0, "PublicStringProperty", "publicStringPropertyTwo");
            var serializer = new JsonSerializerBuilder().WithRuleFor<JsonCustomRuleSerializationTest.MoreComplexDummyClass>(r => r.IgnoreAll()
                                                                                                                                  .ButProperty(o => o.PublicStringProperty)
                                                                                                                                  .AndField(o => o.PublicField))
                                                        .Build();

            var json = serializer.Serialize(moreComplexDummyObject);

            json.Should().Be("{\"publicProperty\":\"2016-02-10\",\"publicDoubleProperty\":42.0,\"publicStringProperty\":\"PublicStringProperty\",\"publicStringPropertyTwo\":\"publicStringPropertyTwo\",\"publicField\":11}");
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