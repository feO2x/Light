using System.Collections.Generic;
using System.Xml.Linq;
using Light.BayesianNetwork.NaiveBayes;
using Light.BayesianNetwork.NetworkConverter.SmileConverter;

namespace Light.BayesianNetwork.NetworkConverter
{
    public interface IToLightNetworkConverter
    {
        BayesianNetwork ConvertNetwork(string smileXml);
        IList<NaiveBayesRandomVariableNodeDecorator> ConvertNodesAndAddToNetwork(XDocument network);
        IList<BaseSmileNode> GetSmileNetworkNodes(XDocument networkXDocument);
    }
}