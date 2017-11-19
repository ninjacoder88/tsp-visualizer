using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TravelingSalesPersonVisualizer.Models;

namespace TravelingSalesPersonVisualizer
{
    public class UploadGraphBuilder
    {
        public UploadGraphBuilder(string nodeFileName, string edgeFileName)
        {
            _nodeFileName = nodeFileName;
            _edgeFileName = edgeFileName;
        }

        public GraphModel BuildGraph()
        {
            GraphModel graphModel = new GraphModel();

            List<string> nodeFileLines = File.ReadAllLines(_nodeFileName).ToList();

            foreach (var nodeFileLine in nodeFileLines)
            {
                var splitFileLine = nodeFileLine.Split(new[] {","}, StringSplitOptions.None);
                string nodeName = splitFileLine[0].Trim();
                int x = int.Parse(splitFileLine[1]);
                int y = int.Parse(splitFileLine[2]);

                NodeModel nodeModel = new NodeModel(x, y, nodeName);
                graphModel.Nodes.Add(nodeModel);
            }

            List<string> edgeFileLines = File.ReadAllLines(_edgeFileName).ToList();

            foreach (var edgeFileLine in edgeFileLines)
            {
                var splitFileLine = edgeFileLine.Split(new[] {","}, StringSplitOptions.None);
                string startNodeName = splitFileLine[0].Trim();
                string endNodeName = splitFileLine[1].Trim();
                int weight = int.Parse(splitFileLine[2]);

                NodeModel startNodeModel = graphModel.Nodes.Single(x => x.Name == startNodeName);
                NodeModel endNodeModel = graphModel.Nodes.Single(x => x.Name == endNodeName);

                EdgeModel edgeModel = new EdgeModel(startNodeModel, endNodeModel, weight);
                graphModel.Edges.Add(edgeModel);
            }

            return graphModel;
        }

        private readonly string _edgeFileName;
        private readonly string _nodeFileName;
    }
}