using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Light.GuardClauses;
using System.Xml.Linq;
using Light.BayesianNetwork.NaiveBayes;

namespace Light.BayesianNetwork.NetworkConverter.SmileConverter
{
    public class SmileToLightNetworkConverter : IToLightNetworkConverter
    {
        private readonly ParentSmileNodesParser _parentSmileNodesParser;
        private readonly ChildSmileNodesParser _childSmileNodesParser;
        private readonly BayesianNetwork _network;
        private readonly NaiveBayesRandomVariableNodeBuilder _nodeBuilder;

        public SmileToLightNetworkConverter(BayesianNetwork network, NaiveBayesRandomVariableNodeBuilder nodeBuilder, ParentSmileNodesParser parentSmileNodesParser, ChildSmileNodesParser childSmileNodesParser) 
        {
            network.MustNotBeNull(nameof(network));
            nodeBuilder.MustNotBeNull(nameof(nodeBuilder));
            parentSmileNodesParser.MustNotBeNull(nameof(parentSmileNodesParser));
            childSmileNodesParser.MustNotBeNull(nameof(childSmileNodesParser));

            _network = network;
            _nodeBuilder = nodeBuilder;
            _parentSmileNodesParser = parentSmileNodesParser;
            _childSmileNodesParser = childSmileNodesParser;
        }

        public BayesianNetwork ConvertNetwork(string smileXml)
        {
            XDocument networkXDocument = XDocument.Load(smileXml);

            _network.Name = GetNetworkIdentifier(networkXDocument);
            

            return _network;
        }

        public IList<NaiveBayesRandomVariableNodeDecorator> ConvertNodesAndAddToNetwork(XDocument network)
        {
            network.MustNotBeNull(nameof(network));

            var parentSmileNodes = _parentSmileNodesParser.Parse(network);
            var childSmileNodes = _childSmileNodesParser.Parse(network);

            if (parentSmileNodes.Count != 1)
                throw new Exception($"Expected one network parent node but parsed {parentSmileNodes.Count}.");

            var networkParentNodes = parentSmileNodes.Select(smileNode =>
            {
                var node = _nodeBuilder.Build();
                node.Name = smileNode.Name;
                
                if(smileNode.OutcomeIds.Length != smileNode.Parameters.Length)
                    throw new ArgumentException($"Node {smileNode} outcome identifiers {smileNode.OutcomeIds} and parameters {smileNode.Parameters} differ in size.");

                var nodeOutcomes = smileNode
                                    .Parameters
                                    .Select(p =>
                                    {
                                        var probability = p;
                                        if(probability.Contains("."))
                                            probability = p.Replace('.', ',');
                                        return OutcomeProbability.FromValue(float.Parse(probability));
                                    })
                                    .Select((outcomeProbability, i) => new Outcome(Guid.NewGuid(), node) {Name = smileNode.OutcomeIds[i], CurrentProbabilityValue = outcomeProbability}).ToList();

                node.AddOutcomes(nodeOutcomes);

                return node;
            }).ToList();

            if(networkParentNodes.Count != 1)
                throw new Exception($"Expected one network parent node but converted {networkParentNodes.Count}.");

            var parentNode = networkParentNodes.First();
            var bayesNetwork = parentNode.Network;
            bayesNetwork.NetworkParentNode = parentNode;

            var nodesIdsForFurtherParentParsing = new List<string>();

            var networkChildNodes = ConvertAllSmileChildsToRandomVariableNodes(childSmileNodes.ToList(), networkParentNodes,
                nodesIdsForFurtherParentParsing);

            var networkNodes = networkParentNodes.Concat(networkChildNodes).ToList();

            foreach (var nodeId in nodesIdsForFurtherParentParsing)
            {
                var node = networkNodes.First(n => n.Name == nodeId);
                var smileNode = childSmileNodes.First(n => n.Id == nodeId);
                AddMissingSmileNodeValuesToExistingRandomVariableNode(smileNode, networkNodes,
                    nodesIdsForFurtherParentParsing, node);
            }

            return networkNodes;
        }

        private void AddMissingSmileNodeValuesToExistingRandomVariableNode(ChildSmileNode smileNode,
            List<NaiveBayesRandomVariableNodeDecorator> networkNodes, List<string> nodesIdsForFurtherParentParsing,
            NaiveBayesRandomVariableNodeDecorator nodeToAddParsedValues)
        {
            ConvertSmileChildNodeToRandomVariableNode(smileNode, networkNodes,
                nodesIdsForFurtherParentParsing, nodeToAddParsedValues);
        }

        private NaiveBayesRandomVariableNodeDecorator ConvertSmileChildNodeToRandomVariableNode(ChildSmileNode smileNode,
            List<NaiveBayesRandomVariableNodeDecorator> nodesPossibleParentNodes, List<string> nodesIdsForFurtherParentParsing,
            NaiveBayesRandomVariableNodeDecorator nodeToAddParsedValues = null)
        {
            if (smileNode.ParentsIds.Length != 1)
                throw new ArgumentException(
                    $"Expected node {smileNode} to have 1 parent but has {smileNode.ParentsIds.Length}.");

            var childsParentNodeId = smileNode.ParentsIds[0];
            var childsParentNode = nodesPossibleParentNodes.Where(n => n.Name == childsParentNodeId).ToList();

            if (childsParentNode.Count == 0)
            {
                nodesIdsForFurtherParentParsing.Add(smileNode.Name);
                var nodeForFurtherParsing = _nodeBuilder.Build();
                nodeForFurtherParsing.Name = smileNode.Name;
                return nodeForFurtherParsing;
            }

            if (childsParentNode.First().Outcomes.Count * smileNode.OutcomeIds.Length != smileNode.Probabilities.Length)
                throw new ArgumentException("Expected that every parent ids make up a probability outcome ids ");

            if (nodeToAddParsedValues == null)
                nodeToAddParsedValues = _nodeBuilder.Build();
            nodeToAddParsedValues.Name = smileNode.Name;

            var parentId = smileNode.ParentsIds[0];
            var parentNode = nodesPossibleParentNodes.First(n => n.Name == parentId);

            if (parentNode == null)
                throw new ArgumentException($"Expected that network {nodeToAddParsedValues.Network} has parent with id {parentId} but the parent could not be found.");

            nodeToAddParsedValues.ConnectParent(parentNode);



            var nodeOutcomes = smileNode
                .OutcomeIds
                .Select(o => OutcomeProbability.DefaultMin)
                .Select(
                    (outcomeProbability, i) => new Outcome(Guid.NewGuid(), nodeToAddParsedValues) { Name = smileNode.OutcomeIds[i], CurrentProbabilityValue = outcomeProbability })
                .ToList();

            nodeToAddParsedValues.AddOutcomes(nodeOutcomes);

            for (var i = 0; i < nodeToAddParsedValues.ParentNodes.First().Outcomes.Count; i++)
            {
                var parentNodeOutcome = nodeToAddParsedValues.ParentNodes.First().Outcomes[i];

                for (var j = 0; j < nodeToAddParsedValues.Outcomes.Count; j++)
                {
                    var outcomeCombination = new OutcomeCombination(parentNodeOutcome, nodeToAddParsedValues.Outcomes[j]);
                    var probabilityValueString = smileNode.Probabilities[i * nodeToAddParsedValues.Outcomes.Count + j];
                    float probabilityValue;
                    float.TryParse(probabilityValueString, NumberStyles.Float, CultureInfo.InvariantCulture,
                        out probabilityValue);

                    nodeToAddParsedValues.ProbabilityTable.Add(outcomeCombination, probabilityValue);
                }
            }

            return nodeToAddParsedValues;
        }

        private List<NaiveBayesRandomVariableNodeDecorator> ConvertAllSmileChildsToRandomVariableNodes(List<ChildSmileNode> childSmileNodes, List<NaiveBayesRandomVariableNodeDecorator> networkParentNodes, List<string> nodesIdsForFurtherParentParsing)
        {
            var networkChildNodes = childSmileNodes.Select(n => ConvertSmileChildNodeToRandomVariableNode(n, networkParentNodes, nodesIdsForFurtherParentParsing)).ToList();

            return networkChildNodes;
        } 

        private string GetNetworkIdentifier(XDocument networkXDocument)
        {
            networkXDocument.Root.MustNotBeNull(nameof(networkXDocument.Root));

            // ReSharper disable once PossibleNullReferenceException
            return networkXDocument.Root.Attribute("id").Value;
        }

        public IList<BaseSmileNode> GetSmileNetworkNodes(XDocument networkXDocument)
        {
            List<BaseSmileNode> nodes;

            nodes = _parentSmileNodesParser.Parse(networkXDocument).Select(n => n as BaseSmileNode).ToList();
            nodes = nodes.Concat(_childSmileNodesParser.Parse(networkXDocument).Select(n => n as BaseSmileNode).ToList()).ToList();

            return nodes ;
        }

    }
}
