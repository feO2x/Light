using System;
using Light.GuardClauses;

namespace Light.BayesianNetwork.FrameworkExtensions
{
    public abstract class EntityWithName : GuidEntity
    {
        private string _name;

        protected EntityWithName(Guid id)
            : base(id)
        {
        }

        public string Name
        {
            get { return _name; }
            set
            {
                value.MustNotBeNullOrWhiteSpace(nameof(value));
                _name = value;
            }
        }

        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(_name) == false ? _name : Id.ToString();
        }
    }
}