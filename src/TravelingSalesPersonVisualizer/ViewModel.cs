using System.Collections.ObjectModel;
using TravelingSalesPersonVisualizer.Models;

namespace TravelingSalesPersonVisualizer
{
    public class ViewModel
    {
        public ViewModel()
        {
            Nodes = new ObservableCollection<Node>();
        }

        public ObservableCollection<Node> Nodes { get; }
    }
}