using System;
using Xunit;

namespace Light.Serialization.Tests
{
    public sealed class JsonCustomRuleSerializationTest : BaseJsonSerializerTest
    {
        [Fact(DisplayName = "Specific properties can be added to a blacklist so that the serializer must ignore them.")]
        public void CustomObjectIngoreProperty()
        {
            var dummyObject = CreateDummyObject();
            AddRule<DummyClass>(r => r.IgnoreProperty(o => o.PublicProperty));

            CompareJsonToExpected(dummyObject, "{\"$id\":0,\"publicField\":42}");
        }

        [Fact(DisplayName = "Specific fields can be added to a blacklist so that the serializer must ignore them.")]
        public void CustomObjectIgnoreField()
        {
            var dummyObject = CreateDummyObject();
            AddRule<DummyClass>(r => r.IgnoreField(o => o.PublicField));

            CompareJsonToExpected(dummyObject, "{\"$id\":0,\"publicProperty\":\"2016-02-09\"}");
        }

        [Fact(DisplayName = "All public properties and fields can be ignored with an empty white list.")]
        public void CustomObjectIgnoreAll()
        {
            var dummyObject = CreateDummyObject();
            AddRule<DummyClass>(r => r.IgnoreAll());

            CompareJsonToExpected(dummyObject, "{\"$id\":0}");
        }

        [Fact(DisplayName = "Specific properties can be added to a white list that gets serialized only.")]
        public void CustomObjectIgnoreAllButProperty()
        {
            var dummyObject = CreateDummyObject();
            AddRule<DummyClass>(r => r.IgnoreAll().ButProperty(o => o.PublicProperty));

            CompareJsonToExpected(dummyObject, "{\"$id\":0,\"publicProperty\":\"2016-02-09\"}");
        }

        [Fact(DisplayName = "Specific fields can be added to a white list that gets serialized only.")]
        public void CustomObjectIgnoreAllButField()
        {
            var dummyObject = CreateDummyObject();
            AddRule<DummyClass>(r => r.IgnoreAll().ButField(o => o.PublicField));

            CompareJsonToExpected(dummyObject, "{\"$id\":0,\"publicField\":42}");
        }

        [Fact(DisplayName = "Specific properties can be added to a blacklist so that the serializer must ignore them.")]
        public void CustomObjectIngoreProperties()
        {
            var moreComplexDummyObject = MoreComplexDummyClass.CreateDefault();
            AddRule<MoreComplexDummyClass>(r => r.IgnoreProperty(o => o.PublicProperty)
                                                 .IgnoreProperty(o => o.PublicDoubleProperty));

            CompareJsonToExpected(moreComplexDummyObject, "{\"$id\":0,\"publicStringProperty\":\"works\",\"publicField\":42}");
        }

        [Fact(DisplayName = "Specific properties can be added to a white so that the serializer must serialize them.")]
        public void CustomObjectIngoreAllButProperties()
        {
            var moreComplexDummyObject = MoreComplexDummyClass.CreateDefault();
            AddRule<MoreComplexDummyClass>(r => r.IgnoreAll()
                                                    .ButProperty(o => o.PublicStringProperty)
                                                    .AndField(o => o.PublicField));

            CompareJsonToExpected(moreComplexDummyObject, "{\"$id\":0,\"publicStringProperty\":\"works\",\"publicField\":42}");
        }

        private static DummyClass CreateDummyObject()
        {
            return new DummyClass("foo", 42, new DateTime(2016, 2, 9));
        }

        public class DummyClass
        {
            // ReSharper disable once NotAccessedField.Local
            private string _privateField;

            public int PublicField;

            public DummyClass(string privateFieldValue, int publicFieldValue, DateTime publicPropertyValue)
            {
                _privateField = privateFieldValue;
                PublicField = publicFieldValue;
                PublicProperty = publicPropertyValue;
            }

            public DateTime PublicProperty { get; }
        }
    }
}