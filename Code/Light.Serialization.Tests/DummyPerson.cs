namespace Light.Serialization.Tests
{
    public class DummyPerson : IDummyPerson
    {
        public DummyPerson()
        {

        }

        public DummyPerson(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public string Name { get; set; }
        public int Age { get; set; }
    }
}