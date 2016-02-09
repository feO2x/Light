using System;
using FluentAssertions;
using Light.Serialization.Json;
using Xunit;

namespace Light.Serialization.Tests
{
    public sealed class JsonCustomRuleSerializationTest
    {
        [Fact(DisplayName = "Specific properties can be added to a blacklist so that the serializer must ignore them.")]
        public void CustomObjectIngoreProperty()
        {
            var dummyObject = CreateDummyObject();
            var serializer = new JsonSerializerBuilder().WithRuleFor<DummyClass>(r => r.IgnoreProperty(o => o.PublicProperty))
                                                        .Build();

            var json = serializer.Serialize(dummyObject);

            json.Should().Be("{\"PublicField\":42}");
        }

        [Fact(DisplayName = "Specific fields can be added to a blacklist so that the serializer must ignore them.")]
        public void CustomObjectIgnoreField()
        {
            var dummyObject = CreateDummyObject();
            var serializer = new JsonSerializerBuilder().WithRuleFor<DummyClass>(r => r.IgnoreField(o => o.PublicField))
                                                        .Build();

            var json = serializer.Serialize(dummyObject);

            json.Should().Be("{\"PublicProperty\":\"2016-02-09\"}");
        }

        [Fact(DisplayName = "All public properties and fields can be ignored with an empty white list.")]
        public void CustomObjectIgnoreAll()
        {
            var dummyObject = CreateDummyObject();
            var serializer = new JsonSerializerBuilder().WithRuleFor<DummyClass>(r => r.IgnoreAll())
                                                        .Build();

            var json = serializer.Serialize(dummyObject);

            json.Should().Be("{}");
        }

        [Fact(DisplayName = "Specific properties can be added to a white list that gets serialized only.")]
        public void CustomObjectIgnoreAllButProperty()
        {
            var dummyObject = CreateDummyObject();
            var serializer = new JsonSerializerBuilder().WithRuleFor<DummyClass>(r => r.IgnoreAll().ButProperty(o => o.PublicProperty))
                                                        .Build();

            var json = serializer.Serialize(dummyObject);

            json.Should().Be("{\"PublicProperty\":\"2016-02-09\"}");
        }

        [Fact(DisplayName = "Specific fields can be added to a white list that gets serialized only.")]
        public void CustomObjectIgnoreAllButField()
        {
            var dummyObject = CreateDummyObject();
            var serializer = new JsonSerializerBuilder().WithRuleFor<DummyClass>(r => r.IgnoreAll().ButField(o => o.PublicField))
                                                        .Build();

            var json = serializer.Serialize(dummyObject);

            json.Should().Be("{\"PublicField\":42}");
        }

        [Fact(DisplayName = "Specific properties can be added to a blacklist so that the serializer must ignore them.")]
        public void CustomObjectIngoreProperties()
        {
            var moreComplexDummyObject = CreateMoreComplexDummyClass();
            var serializer = new JsonSerializerBuilder().WithRuleFor<MoreComplexDummyClass>(r => r.IgnoreProperty(o => o.PublicProperty)
                                                                                                  .IgnoreProperty(o => o.PublicDoubleProperty))
                                                        .Build();

            var json = serializer.Serialize(moreComplexDummyObject);

            json.Should().Be("{\"PublicStringProperty\":\"works\",\"PublicField\":42}");
        }

        [Fact(DisplayName = "Specific properties can be added to a white so that the serializer must serialize them.")]
        public void CustomObjectIngoreAllButProperties()
        {
            var moreComplexDummyObject = CreateMoreComplexDummyClass();
            var serializer = new JsonSerializerBuilder().WithRuleFor<MoreComplexDummyClass>(r => r.IgnoreAll()
                                                                                                  .ButProperty(o => o.PublicStringProperty)
                                                                                                  .AndField(o => o.PublicField))
                                                        .Build();

            var json = serializer.Serialize(moreComplexDummyObject);

            json.Should().Be("{\"PublicStringProperty\":\"works\",\"PublicField\":42}");
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

        private static MoreComplexDummyClass CreateMoreComplexDummyClass()
        {
            return new MoreComplexDummyClass("foo", 42, new DateTime(2016, 2, 9), 42.0, "works");
        }

        public class MoreComplexDummyClass
        {
            public static int PublicStaticField = 0;
            // ReSharper disable once NotAccessedField.Local
            private string _privateField;
            public int PublicField;

            public MoreComplexDummyClass(string privateFieldValue, int publicFieldValue, DateTime publicPropertyValue, double publicDoubleProperty, string publicStringProperty)
            {
                _privateField = privateFieldValue;
                PublicField = publicFieldValue;
                PublicProperty = publicPropertyValue;
                PublicDoubleProperty = publicDoubleProperty;
                PublicStringProperty = publicStringProperty;
            }

            public DateTime PublicProperty { get; }
            public double PublicDoubleProperty { get; set; }
            public string PublicStringProperty { get; set; }
        }
    }
}