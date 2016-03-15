using FluentAssertions;
using Light.Serialization.Tests;
using Microsoft.Practices.Unity;
using Xunit;

namespace Light.Serialization.UnityContainerIntegration.Tests
{
    public sealed class RulesSupportTests
    {
        private readonly IUnityContainer _container;

        public RulesSupportTests()
        {
            _container = new UnityContainer().RegisterDefaultSerializationTypes();
        }

        [Fact(DisplayName = "Single property can be added to a blacklist using the Unity container so that the serializer must ignore them.")]
        public void IgnoreSingle()
        {
            _container.WithSerializationRuleFor<MoreComplexDummyClass>(r => r.IgnoreProperty(o => o.PublicStringProperty));

            SerializeAndCompareWithExpected("{\"$id\":0,\"publicProperty\":\"2016-02-09\",\"publicDoubleProperty\":42.75,\"publicField\":42}");
        }

        [Fact(DisplayName = "Several properties can be added to a blacklist using the Unity container so that the serializer must ignore them.")]
        public void IgnoreSeveral()
        {
            _container.WithSerializationRuleFor<MoreComplexDummyClass>(r => r.IgnoreProperty(o => o.PublicDoubleProperty)
                                                                             .IgnoreProperty(o => o.PublicProperty));

            SerializeAndCompareWithExpected("{\"$id\":0,\"publicStringProperty\":\"works\",\"publicField\":42}");
        }

        [Fact(DisplayName = "Fields can be added to a blacklist using the Unity container so that the serializer must ignore them.")]
        public void IgnoreField()
        {
            _container.WithSerializationRuleFor<MoreComplexDummyClass>(r => r.IgnoreField(o => o.PublicField));

            SerializeAndCompareWithExpected("{\"$id\":0,\"publicProperty\":\"2016-02-09\",\"publicDoubleProperty\":42.75,\"publicStringProperty\":\"works\"}");
        }

        [Fact(DisplayName = "All public members can be set on a blacklist using the Unity container so that the serializer must ignore them all.")]
        public void IgnoreAll()
        {
            _container.WithSerializationRuleFor<MoreComplexDummyClass>(r => r.IgnoreAll());

            SerializeAndCompareWithExpected("{\"$id\":0}");
        }

        [Fact(DisplayName = "Properties can be added to a whitelist using the Unity container so that the serializer must only serialize them.")]
        public void IncludeProperties()
        {
            _container.WithSerializationRuleFor<MoreComplexDummyClass>(r => r.IgnoreAll()
                                                                             .ButProperty(o => o.PublicProperty)
                                                                             .AndProperty(o => o.PublicDoubleProperty));

            SerializeAndCompareWithExpected("{\"$id\":0,\"publicProperty\":\"2016-02-09\",\"publicDoubleProperty\":42.75}");
        }

        [Fact(DisplayName = "Fields can be added to a whitelist using  the Unity container so that the serializer must only serialize them.")]
        public void IncludeFields()
        {
            _container.WithSerializationRuleFor<MoreComplexDummyClass>(r => r.IgnoreAll()
                                                                             .ButField(o => o.PublicField));

            SerializeAndCompareWithExpected("{\"$id\":0,\"publicField\":42}");
        }

        private void SerializeAndCompareWithExpected(string expectedJson)
        {
            var serializer = _container.Resolve<ISerializer>();
            var json = serializer.Serialize(MoreComplexDummyClass.CreateDefault());
            json.Should().Be(expectedJson);
        }
    }
}