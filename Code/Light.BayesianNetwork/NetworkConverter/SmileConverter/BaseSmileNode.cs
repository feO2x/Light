using Light.GuardClauses;

namespace Light.BayesianNetwork.NetworkConverter.SmileConverter
{
    public abstract class BaseSmileNode
    {
        private string _name;
        private string _diagType;
        private string[] _outcomeIds;

        protected BaseSmileNode(string id)
        {
            id.MustNotBeNullOrEmpty(nameof(id));

            Id = id;
        }

        public string Id { get; }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                value.MustNotBeNullOrEmpty(nameof(value));
                _name = value;
            }
        }

        public string DiagType
        {
            get
            {
                return _diagType;
            }
            set
            {
                value.MustNotBeNullOrEmpty(nameof(value));
                _diagType = value;
            }
        }

        public string[] OutcomeIds
        {
            get
            {
                return _outcomeIds;
            }
            set
            {
                value.MustNotBeNullOrEmpty(nameof(value));
                _outcomeIds = value;
            }
        }
    }
}