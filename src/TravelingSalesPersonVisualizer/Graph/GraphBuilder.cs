using System;
using System.Collections.Generic;
using System.Linq;
using TravelingSalesPersonVisualizer.Models;

namespace TravelingSalesPersonVisualizer
{
    public class GraphBuilder
    {
        public GraphModel BuildGraph(int requestedNodeCount, int requestedEdgeCount, int maxX, int maxY)
        {
            if (requestedEdgeCount > (requestedNodeCount * (requestedNodeCount - 1)) / 2)
            {
                throw new GraphBuilderException("The number of requested edges exceeds the maximum number of allowed edges based on the number of nodes");
            }

            GraphModel graphModel = new GraphModel();

            Random random = new Random();

            for (int i = 0; i < requestedNodeCount; i++)
            {
                int x = random.Next(10, maxX-10);
                int y = random.Next(10, maxY-10);
                graphModel.Nodes.Add(new NodeModel(x, y, i.ToString()));
            }

            int edgeCount = 0;
            while (edgeCount < requestedEdgeCount)
            {
                List<int> nodeIds = Enumerable.Range(0, requestedNodeCount).ToList();

                int n1 = nodeIds[random.Next(0, nodeIds.Count)];
                nodeIds.Remove(n1);
                NodeModel startNode = graphModel.Nodes[n1];

                if (startNode.Edges.Count == requestedNodeCount - 1)
                {
                    continue;
                }

                NodeModel endNode = null;

                while (true)
                {
                    int n2 = nodeIds[random.Next(0, nodeIds.Count)];
                    nodeIds.Remove(n2);

                    endNode = graphModel.Nodes[n2];

                    if (graphModel.Edges.Any(x => x.Start == startNode && x.End == endNode))
                    {
                        //regenerate n2
                        continue;
                    }
                    if (graphModel.Edges.Any(x => x.Start == endNode && x.End == startNode))
                    {
                        //regeneate n2
                        continue;
                    }
                    break;
                }

                int edgeWeight = random.Next(1, 5);
                var edge = new EdgeModel(startNode, endNode, edgeWeight);
                graphModel.Edges.Add(edge);
                startNode.Edges.Add(edge);
                endNode.Edges.Add(edge);
                edgeCount++;
            }

            return graphModel;
        }
    }
}