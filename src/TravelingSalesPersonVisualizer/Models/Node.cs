using System.Collections.Generic;
using System.Windows.Documents;

namespace TravelingSalesPersonVisualizer.Models
{
    public class Node
    {
        public Node(int x, int y, string name)
        {
            X = x;
            Y = y;
            Name = name;
            Edges = new List<Edge>();
        }

        public int X { get; }

        public int Y { get; }

        public string Name { get; }

        public List<Edge> Edges { get; }
    }
}