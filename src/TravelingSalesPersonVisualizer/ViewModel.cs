using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using TravelingSalesPersonVisualizer.Models;

namespace TravelingSalesPersonVisualizer
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModel()
        {
            Logs = new ObservableCollection<EventLogModel>();
            Solutions = new ObservableCollection<SolutionModel>();
        }

        public string EdgeFilePath
        {
            get => _edgeFilePath;
            set
            {
                _edgeFilePath = value;
                OnPropertyChanged();
            }
        }

        public GraphModel Graph { get; private set; }

        public ObservableCollection<EventLogModel> Logs { get; }

        public string NodeFilePath
        {
            get => _nodeFilePath;
            set
            {
                _nodeFilePath = value;
                OnPropertyChanged();
            }
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

        public ObservableCollection<SolutionModel> Solutions { get; }

        public void GenerateRandomGraph(int maxX, int maxY)
        {
            Graph = new RandomSingleEdgeBetweenNodeGraphBuilder().BuildGraph(RequestedNodeCount, RequestedEdgeCount, maxX, maxY);
        }

        public void GenerateUploadedGraph()
        {
            Graph = new UploadGraphBuilder(NodeFilePath, EdgeFilePath).BuildGraph();
        }

        private void Clear()
        {
            Logs.Clear();
            Solutions.Clear();
        }

        public void TrySolve()
        {
            Clear();

            foreach (var graphNode in Graph.Nodes)
            {
                Log($"STARTING AT {graphNode.Name}");
                SolutionModel solutionModel = new SolutionModel();

                var remainingNodes = Graph.Nodes.ToList();

                Do(graphNode, remainingNodes, solutionModel);

                Solutions.Add(solutionModel);
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Do(NodeModel currentNode, List<NodeModel> remainingNodes, SolutionModel solutionModel)
        {
            if (!remainingNodes.Any())
            {
                Log($"Solved - {solutionModel.Total}");
                solutionModel.Solved = true;
                return;
            }

            if (currentNode == null)
            {
                Log($"Unsolved - {solutionModel.Total}");
                solutionModel.Solved = false;
                return;
            }

            NodeModel closetNode = null;
            EdgeModel traversedEdge = null;
            double minWeightedDistance = double.MaxValue;

            foreach (var edge in currentNode.Edges)
            {
                NodeModel otherNode = edge.Start == currentNode ? edge.End : edge.Start;

                if (!remainingNodes.Contains(otherNode))
                {
                    continue;
                }

                double distance = Math.Sqrt(Math.Pow(edge.End.X - edge.Start.X, 2) + Math.Pow(edge.End.Y - edge.Start.Y, 2));
                double weightedDistance = distance * edge.Weight;

                if (weightedDistance < minWeightedDistance)
                {
                    //Log($"New minimum = Node: {otherNode.Name} Edge: {edge.Name}");
                    minWeightedDistance = weightedDistance;
                    closetNode = otherNode;
                    traversedEdge = edge;
                }
            }

            remainingNodes.Remove(currentNode);

            if (closetNode != null)
            {
                solutionModel.Nodes.Add(closetNode);
                solutionModel.Edges.Add(traversedEdge);
                solutionModel.Total += (decimal) minWeightedDistance;
            }

            Do(closetNode, remainingNodes, solutionModel);
        }

        private void Log(string text)
        {
            Logs.Add(new EventLogModel(text));
        }

        private string _edgeFilePath;
        private string _nodeFilePath;
        private int _requestedEdgeCount;
        private int _requestedNodeCount;
    }
}