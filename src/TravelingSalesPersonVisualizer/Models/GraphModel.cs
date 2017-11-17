using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TravelingSalesPersonVisualizer.Models
{
    public class GraphModel
    {
        public GraphModel()
        {
            Nodes = new List<NodeModel>();
            Edges = new List<EdgeModel>();
        }

        public int NodeCount => Nodes.Count;

        public int EdgeCount => Edges.Count;

        public List<NodeModel> Nodes { get; }

        public List<EdgeModel> Edges { get; }
    }
}