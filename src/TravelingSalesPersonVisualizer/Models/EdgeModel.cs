namespace TravelingSalesPersonVisualizer.Models
{
    public class EdgeModel
    {
        public EdgeModel(NodeModel start, NodeModel end, int weight)
        {
            Start = start;
            End = end;
            Weight = weight;
            Name = $"{start.Name} - {end.Name}";
        }

        public NodeModel End { get; }

        public string Name { get; }

        public NodeModel Start { get; }

        public int Weight { get; }
    }
}