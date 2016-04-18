using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Light.GuardClauses;

namespace Light.BayesianNetwork.NetworkConverter.SmileConverter
{
    public class SmileNode
    {
        private string _nodeId;
        private string _nodeType;
        private string _probabilities;
        private string _name;
        private IList<string> _nodeOutcomeNames;

        public SmileNode(string nodeId)
        {
            _nodeId = nodeId;
            _nodeOutcomeNames = new List<string>();
        }

        public string Id => _nodeId;

        public string Type
        {
            get { return _nodeType; }
            set
            {
                value.MustNotBeNullOrEmpty(nameof(value));
                _nodeType = value;
            }
        }

        public string Probabilities
        {
            get { return _probabilities; }
            set
            {
                value.MustNotBeNullOrEmpty(nameof(value));
                _probabilities = value;
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                value.MustNotBeNullOrEmpty(nameof(value));
                _name = value;
            }
        }

        public IList<string> OutcomeNames
        {
            get { return _nodeOutcomeNames; }
            set
            {
                value.MustNotBeNullOrEmpty(nameof(value));
                _nodeOutcomeNames = value;
            }
        }
    }
}
