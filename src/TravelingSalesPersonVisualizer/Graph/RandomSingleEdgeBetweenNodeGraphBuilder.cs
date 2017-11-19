using System;
using System.Collections.Generic;
using System.Linq;
using TravelingSalesPersonVisualizer.Models;

namespace TravelingSalesPersonVisualizer
{
    public class RandomSingleEdgeBetweenNodeGraphBuilder
    {
        public RandomSingleEdgeBetweenNodeGraphBuilder()
        {
        }

        public GraphModel BuildGraph(int requestedNodeCount, int requestedEdgeCount, int maxX, int maxY)
        {
            if (requestedEdgeCount > (requestedNodeCount * (requestedNodeCount - 1)) / 2)
            {
                throw new GraphBuilderException("The number of requested edges exceeds the maximum number of allowed edges based on the number of nodes");
            }

            var graphModel = new GraphModel();
            var random = new Random();
            var neighborsDictionary = new Dictionary<NodeModel, List<NodeModel>>();

            for (int i = 0; i < requestedNodeCount; i++)
            {
                int x = random.Next(0, maxX);
                int y = random.Next(0, maxY);

                var node = new NodeModel(x, y, i.ToString());
                graphModel.Nodes.Add(node);
                neighborsDictionary.Add(node, new List<NodeModel>());
            }

            int edgeCount = 0;
            while (edgeCount < requestedEdgeCount)
            {
                var nodeIndicies = Enumerable.Range(0, requestedNodeCount).ToList();

                int startNodeIndex = nodeIndicies[random.Next(0, nodeIndicies.Count)];

                nodeIndicies.Remove(startNodeIndex);

                NodeModel startNode = graphModel.Nodes[startNodeIndex];

                if (startNode.Edges.Count == requestedNodeCount - 1)
                {
                    //maximum number of edges
                    continue;
                }

                NodeModel endNode;

                while (true)
                {
                    int endNodeIndex = nodeIndicies[random.Next(0, nodeIndicies.Count)];

                    nodeIndicies.Remove(endNodeIndex);

                    endNode = graphModel.Nodes[endNodeIndex];

                    if (neighborsDictionary[startNode].Contains(endNode))
                    {
                        continue;
                    }
                    if (neighborsDictionary[endNode].Contains(startNode))
                    {
                        continue;
                    }
                    break;
                }

                int edgeWeight = random.Next(1, 5);

                var edge = new EdgeModel(startNode, endNode, edgeWeight);
                graphModel.Edges.Add(edge);
                startNode.Edges.Add(edge);
                endNode.Edges.Add(edge);

                neighborsDictionary[startNode].Add(endNode);
                neighborsDictionary[endNode].Add(startNode);

                edgeCount++;
            }
            return graphModel;
        }

        //public void BuildGraphUnlimitedEdgesBetweenNodes()
        //{
        //    if (_requestedEdgeCount > (_requestedNodeCount * (_requestedNodeCount - 1)))
        //    {
        //        throw new GraphBuilderException("The number of requested edges exceeds the maximum number of allowed edges based on the number of nodes");
        //    }
        //}
    }
}