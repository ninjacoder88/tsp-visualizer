using System;
using TravelingSalesPersonVisualizer.Models;

namespace TravelingSalesPersonVisualizer.AppEventArgs
{
    public class EdgeUpdatedEventArgs : EventArgs
    {
        public EdgeUpdatedEventArgs(EdgeModel edgeModel)
        {
            EdgeModel = edgeModel;
        }

        public EdgeModel EdgeModel { get; }
    }
}