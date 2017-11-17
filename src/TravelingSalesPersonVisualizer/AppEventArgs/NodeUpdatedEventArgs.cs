using System;
using TravelingSalesPersonVisualizer.Models;

namespace TravelingSalesPersonVisualizer.AppEventArgs
{
    public class NodeUpdatedEventArgs : EventArgs
    {
        public NodeUpdatedEventArgs(NodeModel nodeModel)
        {
            NodeModel = nodeModel;
        }

        public NodeModel NodeModel { get; }
    }
}