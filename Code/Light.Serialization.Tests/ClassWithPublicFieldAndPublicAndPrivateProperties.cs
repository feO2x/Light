namespace Light.Serialization.Tests
{
    public class ClassWithPublicFieldAndPublicAndPrivateProperties
    {
        public readonly string StringField;

        public ClassWithPublicFieldAndPublicAndPrivateProperties(string stringField, double value)
        {
            StringField = stringField;
            Value = value;
            ProtectedStringProperty = "Value";
        }

        protected string ProtectedStringProperty { get; set; }

        public double Value { get; private set; }
    }
}