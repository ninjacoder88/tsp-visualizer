using System.ComponentModel;
using System.Runtime.CompilerServices;
using TravelingSalesPersonVisualizer.Models;

namespace TravelingSalesPersonVisualizer
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModel()
        {
        }

        public int RequestedEdgeCount
        {
            get => _requestedEdgeCount;
            set
            {
                _requestedEdgeCount = value;
                OnPropertyChanged();
            }
        }

        public int RequestedNodeCount
        {
            get => _requestedNodeCount;
            set
            {
                _requestedNodeCount = value;
                OnPropertyChanged();
            }
        }

        public Graph GenerateGraph(int maxX, int maxY)
        {
            return new GraphBuilder().BuildGraph(RequestedNodeCount, RequestedEdgeCount, maxX, maxY);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int _requestedEdgeCount;
        private int _requestedNodeCount;
    }
}