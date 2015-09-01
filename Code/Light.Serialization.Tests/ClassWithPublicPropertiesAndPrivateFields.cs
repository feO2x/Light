namespace Light.Serialization.Tests
{
    public class ClassWithPublicPropertiesAndPrivateFields
    {
        public ClassWithPublicPropertiesAndPrivateFields(int intField, string stringField)
        {
            Int = intField;
            String = stringField;
        }

        public int Int { get; }

        public string String { get; }
    }
}