using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using TravelingSalesPersonVisualizer.Models;
using TravelingSalesPersonVisualizer.Utilities;

namespace TravelingSalesPersonVisualizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ButtonGenerateGraph.Click += ButtonGenerateGraph_Click;
            ButtonUploadGraph.Click += ButtonUploadGraph_Click;
            ButtonSolve.Click += ButtonSolve_Click;

            //IGraphBuilder graphBuilder = new UploadGraphBuilder(@"C:\temp\nodes.csv", @"C:\temp\edges.csv");
            //IGraphBuilder graphBuilder = new RandomSingleEdgeBetweenNodeGraphBuilder();
            _viewModel = new ViewModel();

            EventLogGrid.ItemsSource = _viewModel.Logs;

            BindingHelper.BindTextBox(TextBoxNodeCount, _viewModel, nameof(_viewModel.RequestedNodeCount));
            BindingHelper.BindTextBox(TextBoxEdgeCount, _viewModel, nameof(_viewModel.RequestedEdgeCount));
            BindingHelper.BindTextBox(TextBoxNodeFilePath, _viewModel, nameof(_viewModel.NodeFilePath));
            BindingHelper.BindTextBox(TextBoxEdgeFilePath, _viewModel, nameof(_viewModel.EdgeFilePath));

            NodeEllipse = new Dictionary<NodeModel, Ellipse>();
            EdgeLine = new Dictionary<EdgeModel, Line>();
        }

        private void ButtonUploadGraph_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.GenerateUploadedGraph();
        }

        private Dictionary<EdgeModel, Line> EdgeLine { get; }

        private Dictionary<NodeModel, Ellipse> NodeEllipse { get; }

        private Ellipse AddNode(int x, int y, Brush brush)
        {
            Ellipse ellipse = new Ellipse {Width = 20, Height = 20, Fill = brush};

            Canvas.SetLeft(ellipse, x);
            Canvas.SetTop(ellipse, y);
            MainCanvas.Children.Add(ellipse);
            return ellipse;
        }

        private void ButtonGenerateGraph_Click(object sender, RoutedEventArgs e)
        {
            MainCanvas.Children.Clear();
            NodeEllipse.Clear();
            EdgeLine.Clear();

            _viewModel.GenerateRandomGraph((int) MainCanvas.ActualWidth, (int) MainCanvas.ActualHeight);

            Brush nodeBrush = (SolidColorBrush) new BrushConverter().ConvertFrom("#507EA5");
            Brush edgeBrush = (SolidColorBrush) new BrushConverter().ConvertFrom("#B9BCBF");

            SolidColorBrush brush = new SolidColorBrush(Color.FromRgb(255, 255, 255));

            foreach (var graphEdge in _viewModel.Graph.Edges)
            {
                var line = new Line
                               {
                                   X1 = graphEdge.Start.X + 7.5,
                                   X2 = graphEdge.End.X + 7.5,
                                   Y1 = graphEdge.Start.Y + 7.5,
                                   Y2 = graphEdge.End.Y + 7.5,
                                   Stroke = edgeBrush,
                                   StrokeThickness = graphEdge.Weight
                               };
                EdgeLine.Add(graphEdge, line);
                MainCanvas.Children.Add(line);
            }

            foreach (var graphNode in _viewModel.Graph.Nodes)
            {
                var ellipse = AddNode(graphNode.X, graphNode.Y, nodeBrush);
                NodeEllipse.Add(graphNode, ellipse);

                TextBlock textBlock = new TextBlock();
                textBlock.Text = graphNode.Name;
                textBlock.Foreground = brush;
                textBlock.FontSize = 15;
                textBlock.ToolTip = graphNode.Name;
                Canvas.SetLeft(textBlock, graphNode.X + 5);
                Canvas.SetTop(textBlock, graphNode.Y - 2.5 );
                MainCanvas.Children.Add(textBlock);
            }
        }

        private void ButtonSolve_Click(object sender, RoutedEventArgs e)
        {
            var solutionModels = _viewModel.SolveGrapsh();
            var lastSolution = solutionModels.Last();

            Brush traversedEdgeBrush = new SolidColorBrush(Color.FromRgb(0, 255, 0));

            foreach (var lastSolutionEdge in lastSolution.Edges)
            {
                EdgeLine[lastSolutionEdge].Stroke = traversedEdgeBrush;
            }
        }

        private readonly ViewModel _viewModel;
    }
}