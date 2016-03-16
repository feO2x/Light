using FluentAssertions;
using Microsoft.Practices.Unity;
using Xunit;

namespace Light.Serialization.UnityContainerIntegration.Tests
{
    public sealed class SerializerResolveTests
    {
        [Fact(DisplayName = "The unity container must be able to resolve a serializer that can handle a simple object graph when RegisterDefaultSerializationTypes was called.")]
        public void ResolveSerializer()
        {
            var objectGraph = new Person
                              {
                                  Name = "Walter White",
                                  Age = 52,
                                  Address = new Address
                                            {
                                                Street = "One Way",
                                                ZipCode = "12345",
                                                Location = "Abq"
                                            }
                              };
            const string expectedJson = @"{""$id"":0,""name"":""Walter White"",""age"":52,""address"":{""$id"":1,""street"":""One Way"",""zipCode"":""12345"",""location"":""Abq""}}";
            var unityContainer = new UnityContainer().RegisterDefaultSerializationTypes();
            var serializer = unityContainer.Resolve<ISerializer>();

            var json = serializer.Serialize(objectGraph);

            json.Should().Be(expectedJson);
        }

        public class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public Address Address { get; set; }
        }

        public class Address
        {
            public string Street { get; set; }
            public string ZipCode { get; set; }
            public string Location { get; set; }
        }
    }
}