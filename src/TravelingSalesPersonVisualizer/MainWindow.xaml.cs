using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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

            _viewModel = new ViewModel();

            EventLogGrid.ItemsSource = _viewModel.Logs;

            BindingHelper.BindTextBox(TextBoxNodeCount, _viewModel, nameof(_viewModel.RequestedNodeCount));
            BindingHelper.BindTextBox(TextBoxEdgeCount, _viewModel, nameof(_viewModel.RequestedEdgeCount));
            BindingHelper.BindTextBox(TextBoxNodeFilePath, _viewModel, nameof(_viewModel.NodeFilePath));
            BindingHelper.BindTextBox(TextBoxEdgeFilePath, _viewModel, nameof(_viewModel.EdgeFilePath));

            NodeEllipse = new Dictionary<NodeModel, Ellipse>();
            EdgeLine = new Dictionary<EdgeModel, Line>();
        }

        private Dictionary<EdgeModel, Line> EdgeLine { get; }

        private Dictionary<NodeModel, Ellipse> NodeEllipse { get; }

        private Line AddEdge(EdgeModel graphEdge, Brush edgeBrush)
        {
            var line =
                new Line
                    {
                        X1 = graphEdge.Start.X + 7.5,
                        X2 = graphEdge.End.X + 7.5,
                        Y1 = graphEdge.Start.Y + 7.5,
                        Y2 = graphEdge.End.Y + 7.5,
                        Stroke = edgeBrush,
                        StrokeThickness = graphEdge.Weight,
                        ToolTip = graphEdge.Name
                    };
            MainCanvas.Children.Add(line);
            return line;
        }

        private Ellipse AddNode(string name, int x, int y, Brush nodeBrush, Brush textBrush, Brush textBackgroundBrush)
        {
            Ellipse ellipse = new Ellipse {Width = 20, Height = 20, Fill = nodeBrush, ToolTip = name};
            Canvas.SetLeft(ellipse, x);
            Canvas.SetTop(ellipse, y);
            MainCanvas.Children.Add(ellipse);

            TextBlock textBlock = new TextBlock();
            textBlock.Text = name;
            textBlock.Foreground = textBrush;
            textBlock.FontSize = 15;
            textBlock.Background = textBackgroundBrush;
            Canvas.SetLeft(textBlock, x+15);
            Canvas.SetTop(textBlock, y+15);
            MainCanvas.Children.Add(textBlock);

            return ellipse;
        }

        private void ButtonGenerateGraph_Click(object sender, RoutedEventArgs e)
        {
            Clear();

            _viewModel.GenerateRandomGraph((int) MainCanvas.ActualWidth - 10, (int) MainCanvas.ActualHeight - 10);

            DrawIt();
        }

        private void ButtonSolve_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.TrySolve();
            var bestSolution = _viewModel.Solutions.OrderBy(x => x.Total).First();

            Brush traversedEdgeBrush = new SolidColorBrush(Color.FromRgb(0, 255, 0));

            foreach (var lastSolutionEdge in bestSolution.Edges)
            {
                EdgeLine[lastSolutionEdge].Stroke = traversedEdgeBrush;
            }
        }

        private void ButtonUploadGraph_Click(object sender, RoutedEventArgs e)
        {
            Clear();

            _viewModel.GenerateUploadedGraph();

            DrawIt();
        }

        private void Clear()
        {
            MainCanvas.Children.Clear();
            NodeEllipse.Clear();
            EdgeLine.Clear();
        }

        private void DrawIt()
        {
            Brush nodeBrush = (SolidColorBrush) new BrushConverter().ConvertFrom("#507EA5");
            Brush edgeBrush = (SolidColorBrush) new BrushConverter().ConvertFrom("#B9BCBF");

            SolidColorBrush textBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            SolidColorBrush textBackgroundBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#97B7D2");

            foreach (var graphEdge in _viewModel.Graph.Edges)
            {
                var line = AddEdge(graphEdge, edgeBrush);
                EdgeLine.Add(graphEdge, line);
            }

            foreach (var graphNode in _viewModel.Graph.Nodes)
            {
                var ellipse = AddNode(graphNode.Name, graphNode.X, graphNode.Y, nodeBrush, textBrush, textBackgroundBrush);
                NodeEllipse.Add(graphNode, ellipse);
            }
        }

        private readonly ViewModel _viewModel;
    }
}