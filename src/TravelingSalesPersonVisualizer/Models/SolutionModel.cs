using System.Collections.Generic;

namespace TravelingSalesPersonVisualizer.Models
{
    public class SolutionModel
    {
        public SolutionModel()
        {
            Nodes = new List<NodeModel>();
            Edges = new List<EdgeModel>();
        }

        public IList<EdgeModel> Edges { get; }

        public IList<NodeModel> Nodes { get; }

        public bool Solved { get; set; }
    }
}