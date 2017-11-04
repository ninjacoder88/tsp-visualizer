using System;
using System.Collections.Generic;
using System.Linq;
using TravelingSalesPersonVisualizer.Models;

namespace TravelingSalesPersonVisualizer
{
    public class GraphBuilder
    {
        public Graph BuildGraph(int requestedNodeCount, int requestedEdgeCount, int maxX, int maxY)
        {
            if (requestedEdgeCount > (requestedNodeCount * (requestedNodeCount - 1)) / 2)
            {
                throw new GraphBuilderException("The number of requested edges exceeds the maximum number of allowed edges based on the number of nodes");
            }

            Graph graph = new Graph();

            Random random = new Random();

            for (int i = 0; i < requestedNodeCount; i++)
            {
                int x = random.Next(10, maxX-10);
                int y = random.Next(10, maxY-10);
                graph.Nodes.Add(new Node(x, y, i.ToString()));
            }

            int edgeCount = 0;
            while (edgeCount < requestedEdgeCount)
            {
                List<int> nodeIds = Enumerable.Range(0, requestedNodeCount).ToList();

                int n1 = nodeIds[random.Next(0, nodeIds.Count)];
                nodeIds.Remove(n1);
                Node startNode = graph.Nodes[n1];

                if (startNode.Edges.Count == requestedNodeCount - 1)
                {
                    continue;
                }

                Node endNode = null;

                while (true)
                {
                    int n2 = nodeIds[random.Next(0, nodeIds.Count)];
                    nodeIds.Remove(n2);

                    endNode = graph.Nodes[n2];

                    if (graph.Edges.Any(x => x.Start == startNode && x.End == endNode))
                    {
                        //regenerate n2
                        continue;
                    }
                    if (graph.Edges.Any(x => x.Start == endNode && x.End == startNode))
                    {
                        //regeneate n2
                        continue;
                    }
                    break;
                }

                int edgeWeight = random.Next(1, 5);
                var edge = new Edge(startNode, endNode, edgeWeight);
                graph.Edges.Add(edge);
                startNode.Edges.Add(edge);
                endNode.Edges.Add(edge);
                edgeCount++;
            }

            return graph;
        }
    }
}