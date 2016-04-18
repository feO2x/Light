using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Light.GuardClauses;

namespace Light.BayesianNetwork.NetworkConverter.SmileConverter
{
    public abstract class BaseSmileNodesParser<T> where T : BaseSmileNode
    {
        protected List<BaseSmileNode> ParseBaseSmileNodesInformation(XDocument network)
        {
            //todo: needed for getting name from xml extensions
            //var networkNodeExtensions = network.Descendants("extensions").Descendants().Where(nExt => nExt.Name == "node");
            var baseNodes = network.Descendants("nodes")
                .Descendants()
                .Where(n => n.Name == "noisymax" || n.Name == "cpt")
                .Select(n => new
                {
                    id = ParseSmileNodeAttribute(n, "id"),
                    //todo: use for further use?
                    /*name =
                        networkNodeExtensions.Where(
                            nExt => ParseSmileNodeAttribute(nExt, "id") == ParseSmileNodeAttribute(n, "id"))
                            .Descendants("name")
                            .First()
                            .Value,*/
                    diagtype = ParseSmileNodeAttribute(n, "diagtype"),
                    outcomeIds =
                        n.Descendants("state").Select(nState => ParseSmileNodeAttribute(nState, "id")).ToArray()
                }).ToList();

                var bla = baseNodes.Select(n =>
                {
                    BaseSmileNode node = null;

                    if(n.diagtype == "target")
                        node = new ParentSmileNode(n.id);
                    else
                        node = new ChildSmileNode(n.id);

                    node.DiagType = n.diagtype;
                    node.OutcomeIds = n.outcomeIds;
                    node.Name = n.id;

                    return node;
                }).ToList();

            return bla;
        }

        private string ParseSmileNodeAttribute(XElement node, string attributeValue)
        {
            var attribute = "";
            if(node.Attribute(attributeValue) != null)
                attribute = node.Attribute(attributeValue).Value;

            if (attribute == "")
            {
                var possbileAttribute = node.Descendants(attributeValue).ToList();

                if (possbileAttribute.Count != 1)
                    throw new ArgumentException($"Parse {attributeValue} from node {node} failed. Expected one possbile attribute to parse but got {possbileAttribute.Count}.");

                attribute = possbileAttribute.First().Value;
            }


            if (attribute == "")
                throw new ArgumentException($"Parse {attributeValue} from node {node} failed. Node has no attribute {attributeValue}.");

            return attribute;
        }

        protected object ParseSmileNodeAttribute(XDocument network, string nodeId, string attributeValue)
        {
            network.MustNotBeNull(nameof(network));
            nodeId.MustNotBeNullOrEmpty(nameof(nodeId));
            attributeValue.MustNotBeNullOrEmpty(nameof(attributeValue));

            var node = network
                .Descendants("nodes")
                .Descendants()
                .First(n => n.Attribute("id") != null && n.Attribute("id").Value == nodeId);

            return ParseSmileNodeAttribute(node, attributeValue);
        }

        public abstract bool IsSuitableFor(XDocument network);
        public abstract IList<T> Parse(XDocument network);
    }

    public class ParentSmileNodesParser : BaseSmileNodesParser<ParentSmileNode>
    {
        public override IList<ParentSmileNode> Parse(XDocument network)
        {
            network.MustNotBeNull(nameof(network));

            var smileNodes = ParseBaseSmileNodesInformation(network).FindAll(n => n.DiagType == "target");
            var networkParentNodes = ParseNodeTypeSpecific(network, smileNodes);

            return networkParentNodes;
        }

        private List<ParentSmileNode> ParseNodeTypeSpecific(XDocument network, List<BaseSmileNode> nodes)
        {
            var parentNodes = nodes.FindAll(n => n.GetType() == typeof (ParentSmileNode)).Select(n =>
            {
                ParentSmileNode node = n as ParentSmileNode;
                var nodeParameters = ParseSmileNodeAttribute(network, n.Name, "parameters") as string;
                node.Parameters = nodeParameters.Split(' ');

                return node;
            }).ToList();

            return parentNodes;
        }

        public override bool IsSuitableFor(XDocument network)
        {
            network.MustNotBeNull(nameof(network));

            var parentNode = network.Descendants("noisymax").ToList();

            if (parentNode == null || parentNode.Count != 1)
                return false;

            var parameters = parentNode.First().Attribute("parameters");

            return parameters != null;
        }
    }

    public class ChildSmileNodesParser : BaseSmileNodesParser<ChildSmileNode>
    {
        public override bool IsSuitableFor(XDocument network)
        {
            network.MustNotBeNull(nameof(network));

            var childNodes = network.Descendants("cpt").ToList();

            if (childNodes == null || childNodes.Count > 0)
                return false;

            return !(from childNode in childNodes let nodeProbabilityAttribute = childNode.Attribute("probabilities") let nodeParentsAttribute = childNode.Attribute("parents")
                     where nodeProbabilityAttribute == null || nodeParentsAttribute == null
                     select nodeProbabilityAttribute).Any();
        }

        public override IList<ChildSmileNode> Parse(XDocument network)
        {
            network.MustNotBeNull(nameof(network));

            var smileNodes = ParseBaseSmileNodesInformation(network).FindAll(n => n.DiagType == "observation");
            var childSmileNodes = ParseNodeTypeSpecific(network, smileNodes);

            return childSmileNodes;
        }

        private List<ChildSmileNode> ParseNodeTypeSpecific(XDocument network, List<BaseSmileNode> nodes)
        {
            var childNodes = nodes.FindAll(n => n.GetType() == typeof(ChildSmileNode)).Select(n =>
            {
                ChildSmileNode node = n as ChildSmileNode;
                var parentsIds = ParseSmileNodeAttribute(network, n.Name, "parents") as string;
                var probabilities = ParseSmileNodeAttribute(network, n.Name, "probabilities") as string;

                node.ParentsIds = parentsIds.Split(' ');
                node.Probabilities = probabilities.Split(' ');

                return node;
            }).ToList();

            return childNodes;
        }
    }
}