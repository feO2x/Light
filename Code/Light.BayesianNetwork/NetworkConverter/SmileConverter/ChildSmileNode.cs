using Light.GuardClauses;

namespace Light.BayesianNetwork.NetworkConverter.SmileConverter
{
    public class ChildSmileNode : BaseSmileNode
    {
        private string[] _parentsIds;
        private string[] _probabilities;

        public ChildSmileNode(string id) : base(id)
        {
        }

        public string[] ParentsIds
        {
            get
            {
                return _parentsIds;
            }
            set
            {
                value.MustNotBeNullOrEmpty(nameof(value));
                _parentsIds = value;
            }
        }

        public string[] Probabilities
        {
            get
            {
                return _probabilities;
            }
            set
            {
                value.MustNotBeNullOrEmpty(nameof(value));
                _probabilities = value;
            }
        }
    }
}