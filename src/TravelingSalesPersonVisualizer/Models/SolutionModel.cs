using System.Collections.Generic;
using System.Linq;

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

        public decimal Total { get; set; }
    }
}