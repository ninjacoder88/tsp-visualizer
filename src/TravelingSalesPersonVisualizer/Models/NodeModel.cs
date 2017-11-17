using System.Collections.Generic;
using System.Windows.Documents;

namespace TravelingSalesPersonVisualizer.Models
{
    public class NodeModel
    {
        public NodeModel(int x, int y, string name)
        {
            X = x;
            Y = y;
            Name = name;
            Edges = new List<EdgeModel>();
        }

        public int X { get; }

        public int Y { get; }

        public string Name { get; }

        public List<EdgeModel> Edges { get; }
    }
}