using System.Collections.Generic;

namespace Light.BayesianNetwork
{
    public interface IBayesianNetwork
    {
        INode RootNode { get; }

        IReadOnlyList<INode> Nodes { get; }

        void AddNode(INode newNode);
        void RemoveNode(INode nodeToRemove);

        void EstablishLink(INode parentNode, INode childNode);
        void RemoveLink(INode parentNode, INode childNode);
    }
}