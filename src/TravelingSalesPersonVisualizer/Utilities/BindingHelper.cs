using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace TravelingSalesPersonVisualizer.Utilities
{
    public static class BindingHelper
    {
        public static void BindTextBox(DependencyObject dependencyObject, object dataSource, string propertyName)
        {
            BindingOperations.SetBinding(dependencyObject, TextBox.TextProperty,
                                         new Binding
                                             {
                                                 Source = dataSource,
                                                 Path = new PropertyPath(propertyName),
                                                 Mode = BindingMode.TwoWay,
                                                 UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                                             });
        }
    }
}