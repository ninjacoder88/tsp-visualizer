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

        public void GenerateRandomGraph(int maxX, int maxY)
        {
            Graph = new RandomSingleEdgeBetweenNodeGraphBuilder().BuildGraph(RequestedNodeCount, RequestedEdgeCount, maxX, maxY);
        }

        public void GenerateUploadedGraph()
        {
            Graph = new UploadGraphBuilder(NodeFilePath, EdgeFilePath).BuildGraph();
        }

        public IEnumerable<SolutionModel> SolveGrapsh()
        {
            //able to revisit nodes
            //able to revisit edges
            //best solution (distance, nodes)

            Logs.Clear();

            var solutionModels = new List<SolutionModel>();

            int solutionsAttempted = 0;
            while (true)
            {
                if (solutionsAttempted == Graph.NodeCount)
                {
                    return solutionModels;
                }

                Log($"STARTING ATTEMPT {solutionsAttempted + 1}");

                SolutionModel solution = new SolutionModel();
                solutionModels.Add(solution);

                var currentNode = Graph.Nodes[solutionsAttempted];
                solution.Nodes.Add(currentNode);

                int discoveredNodes = 1;
                while (true)
                {
                    var result = Go(solution, currentNode);
                    if (result.node == null || result.edge == null)
                    {
                        solution.Solved = false;
                        break;
                    }
                    solution.Edges.Add(result.edge);
                    solution.Nodes.Add(result.node);

                    currentNode = result.node;
                    discoveredNodes++;
                }

                if (discoveredNodes != Graph.NodeCount)
                {
                    solution.Solved = false;
                    solutionsAttempted++;
                    continue;
                }
                solution.Solved = true;
                break;
            }
            return solutionModels;
        }

        public void TrySolve()
        {
            foreach (var graphNode in Graph.Nodes)
            {
                SolutionModel solutionModel = new SolutionModel();

                Log($"Starting at Node: {graphNode.Name}");

                var allNodes = Graph.Nodes.ToList();
                allNodes.Remove(graphNode);

                var currentNode = graphNode;
                solutionModel.Nodes.Add(currentNode);

                FindNeighbors(currentNode);
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void FindNeighbors(NodeModel currentNode)
        {
            foreach (var currentNodeEdge in currentNode.Edges)
            {
            }
        }

        private (NodeModel node, EdgeModel edge) Go(SolutionModel solutionModel, NodeModel currentNode)
        {
            Log($"Considering {currentNode.Name}");

            NodeModel closetNode = null;
            EdgeModel traversedEdge = null;
            double minWeightedDistance = double.MaxValue;
            foreach (var edge in currentNode.Edges)
            {
                Log($"Considering {edge.Name}");

                NodeModel otherNode = edge.Start == currentNode ? edge.End : edge.Start;
                if (solutionModel.Nodes.Contains(otherNode))
                {
                    Log($"Node {otherNode.Name} already visited");
                    continue;
                }

                if (solutionModel.Edges.Contains(edge))
                {
                    Log($"Edge {edge.Name} already traversed");
                    continue;
                }

                double distance = Math.Sqrt(Math.Pow(edge.End.X - edge.Start.X, 2) + Math.Pow(edge.End.Y - edge.Start.Y, 2));
                double weightedDistance = distance * edge.Weight;

                Log($"{distance.ToString("F2")} * {edge.Weight} = {weightedDistance.ToString("F2")}");

                if (weightedDistance < minWeightedDistance)
                {
                    Log($"New minimum = Node: {otherNode.Name} Edge: {edge.Name}");
                    minWeightedDistance = weightedDistance;
                    closetNode = otherNode;
                    traversedEdge = edge;
                }
            }

            return (closetNode, traversedEdge);
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