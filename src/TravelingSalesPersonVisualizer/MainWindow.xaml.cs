using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

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
        }

        private void AddNode(int x, int y, Brush brush)
        {
            Rectangle rectangle = new Rectangle {Width = 15, Height = 15, Fill = brush};
            Canvas.SetLeft(rectangle, x);
            Canvas.SetTop(rectangle, y);
            MaintCanvas.Children.Add(rectangle);
        }

        private void ButtonGenerateGraph_Click(object sender, RoutedEventArgs e)
        {
            Brush nodeBrush = new SolidColorBrush(Color.FromRgb(0, 0, 255));

            AddNode(10, 100, nodeBrush);
        }

        private ViewModel _viewModel;
    }
}