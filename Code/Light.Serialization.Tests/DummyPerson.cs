using Light.GuardClauses.FrameworkExtensions;

namespace Light.Serialization.Tests
{
    public class DummyPerson : IDummyPerson
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public bool Equals(DummyPerson other)
        {
            if (other == null)
                return false;

            return Name == other.Name && Age == other.Age;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as DummyPerson);
        }

        public override int GetHashCode()
        {
            return Equality.CreateHashCode(Name, Age);
        }
    }
}