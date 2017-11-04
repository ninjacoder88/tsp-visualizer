using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using TravelingSalesPersonVisualizer.Models;

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

            _viewModel = new ViewModel();

            BindingOperations.SetBinding(TextBoxNodeCount, TextBox.TextProperty, new Binding() { Source = _viewModel, Path = new PropertyPath(nameof(_viewModel.RequestedNodeCount)), Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            BindingOperations.SetBinding(TextBoxEdgeCount, TextBox.TextProperty, new Binding() { Source = _viewModel, Path = new PropertyPath(nameof(_viewModel.RequestedEdgeCount)), Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
        }

        private void AddNode(int x, int y, Brush brush)
        {
            Ellipse ellipse = new Ellipse() {Width = 15, Height = 15, Fill = brush};

            //Rectangle rectangle = new Rectangle {Width = 15, Height = 15, Fill = brush};
            Canvas.SetLeft(ellipse, x);
            Canvas.SetTop(ellipse, y);
            MainCanvas.Children.Add(ellipse);
        }

        private void ButtonGenerateGraph_Click(object sender, RoutedEventArgs e)
        {
            MainCanvas.Children.Clear();

            Graph graph = _viewModel.GenerateGraph((int)MainCanvas.ActualWidth, (int)MainCanvas.ActualHeight);

            Brush nodeBrush = new SolidColorBrush(Color.FromRgb(0, 0, 255));
            Brush edgeBrush = new SolidColorBrush(Color.FromRgb(255, 0, 0));

            foreach (var graphNode in graph.Nodes)
            {
                AddNode(graphNode.X, graphNode.Y, nodeBrush);
            }

            foreach (var graphEdge in graph.Edges)
            {
                Line line = new Line(){X1 = graphEdge.Start.X+7.5, X2 = graphEdge.End.X + 7.5, Y1 = graphEdge.Start.Y + 7.5, Y2 = graphEdge.End.Y + 7.5, Stroke = edgeBrush, StrokeThickness = graphEdge.Weight};
                MainCanvas.Children.Add(line);
            }
        }

        private ViewModel _viewModel;
    }
}