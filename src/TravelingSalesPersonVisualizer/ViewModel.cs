using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TravelingSalesPersonVisualizer.AppEventArgs;
using TravelingSalesPersonVisualizer.Models;

namespace TravelingSalesPersonVisualizer
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModel()
        {
        }

        public GraphModel Graph { get; private set; }

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

        public void GenerateGraph(int maxX, int maxY)
        {
            Graph = new GraphBuilder().BuildGraph(RequestedNodeCount, RequestedEdgeCount, maxX, maxY);
        }

        public IEnumerable<SolutionModel> SolveGrapsh()
        {
            //able to revisit nodes
            //able to revisit edges
            //best solution (distance, nodes)

            List<SolutionModel> solutionModels = new List<SolutionModel>();

            int solutionsAttempted = 0;
            while (true)
            {
                if (solutionsAttempted == Graph.NodeCount)
                {
                    return solutionModels;
                }

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
                    //"Trying again".Dump();
                    solution.Solved = false;
                    solutionsAttempted++;
                    continue;
                }
                solution.Solved = true;
                break;
            }
            return solutionModels;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private (NodeModel node, EdgeModel edge) Go(SolutionModel solutionModel, NodeModel currentNode)
        {
            //currentNode.Name.Dump();

            NodeModel closetNode = null;
            EdgeModel traversedEdge = null;
            double minDistance = double.MaxValue;
            foreach (var edge in currentNode.Edges)
            {
                //$"Considering {edge.Name}".Dump();

                NodeModel otherNode = edge.Start == currentNode ? edge.End : edge.Start;
                if (solutionModel.Nodes.Contains(otherNode))
                {
                    //"Node already visited".Dump();
                    continue;
                }

                if (solutionModel.Edges.Contains(edge))
                {
                    //"Edge already traversed".Dump();
                    continue;
                }

                //double distance = Math.Sqrt(Math.Pow(edge.End.X - edge.Start.X, 2) + Math.Pow(edge.End.Y - edge.Start.Y, 2));
                double distance = (Math.Sqrt(Math.Pow(edge.End.X - edge.Start.X, 2) + Math.Pow(edge.End.Y - edge.Start.Y, 2))) * edge.Weight;
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closetNode = otherNode;
                    traversedEdge = edge;
                }
            }
            //var output = closetNode == null ? "End" : $"Chose {closetNode.Name} Traversed {traversedEdge.Name}";
            //output.Dump();
            return (closetNode, traversedEdge);
        }

        private int _requestedEdgeCount;
        private int _requestedNodeCount;
    }
}