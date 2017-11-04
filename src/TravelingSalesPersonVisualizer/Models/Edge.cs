namespace TravelingSalesPersonVisualizer.Models
{
    public class Edge
    {
        public Edge(Node start, Node end, int weight)
        {
            Start = start;
            End = end;
            Weight = weight;
            Name = $"{start.Name} - {end.Name}";
        }

        public Node End { get; }

        public string Name { get; }

        public Node Start { get; }

        public int Weight { get; }
    }
}