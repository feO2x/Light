using Light.GuardClauses;

namespace Light.BayesianNetwork.NetworkConverter.SmileConverter
{
    public class ParentSmileNode : BaseSmileNode
    {
        private string[] _parameters;

        public ParentSmileNode(string id) : base(id)
        {
        }

        public string[] Parameters
        {
            get
            {
                return _parameters;
            }
            set
            {
                value.MustNotBeNullOrEmpty(nameof(value));
                _parameters = value;
            }
        }
    }
}