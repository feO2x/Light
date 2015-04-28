namespace Light.Serialization.Tests
{
    public class ClassWithPublicPropertiesAndPrivateFields
    {
        private readonly int _intField;
        private readonly string _stringField;

        public ClassWithPublicPropertiesAndPrivateFields(int intField, string stringField)
        {
            _intField = intField;
            _stringField = stringField;
        }

        public int Int
        {
            get { return _intField; }
        }

        public string String
        {
            get { return _stringField; }
        }
    }
}