using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TravelingSalesPersonVisualizer.Models
{
    public class Graph
    {
        public Graph()
        {
            Nodes = new List<Node>();
            Edges = new List<Edge>();
        }

        public int NodeCount => Nodes.Count;

        public int EdgeCount => Edges.Count;

        public List<Node> Nodes { get; }

        public List<Edge> Edges { get; }
    }
}