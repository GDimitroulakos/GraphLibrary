using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using GraphLibrary;
using GraphLibrary.Generics;

namespace TestSerialization {
    class Program {
        static void Main(string[] args) {
            // 1. Create a graph
            CGraph mgraph = CGraph.CreateGraph();

            CGraphNode x = mgraph.CreateGraphNode<CGraphNode>("x");
            CGraphNode y = mgraph.CreateGraphNode<CGraphNode>("y");
            CGraphNode z = mgraph.CreateGraphNode<CGraphNode>("z");
            CGraphNode t = mgraph.CreateGraphNode<CGraphNode>("t");
            CGraphNode s = mgraph.CreateGraphNode<CGraphNode>("s");

            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(t, x, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(t, y, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(t, z, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(x, t, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(y, x, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(y, z, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(z, x, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(z, s, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(s, t, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(s, y, GraphType.GT_DIRECTED);

            // 2. Associate weights with the edges of the graph
            CGraphQueryInfo<int, int, int> weights = new CGraphQueryInfo<int, int, int>(mgraph, 255);

            weights.CreateInfo(y, x, -3);
            weights.CreateInfo(x, t, -2);
            weights.CreateInfo(t, x, 5);
            weights.CreateInfo(z, x, 7);
            weights.CreateInfo(y, z, 9);
            weights.CreateInfo(t, y, 8);
            weights.CreateInfo(t, z, -4);
            weights.CreateInfo(s, t, 6);
            weights.CreateInfo(s, y, 7);
            weights.CreateInfo(z, s, 2);

            mgraph.RegisterGraphPrinter(new CGraphVizPrinter(mgraph));
            // The graph uses the registered printers to print the graph to the specified output
            mgraph.Generate(@"D:\MyPrivateWork\MyApps\MyApplications\EDUFLEX\GraphLibrary\TestSerialization\bin\Debug\test.dot", true);

            BinaryFormatter saver = new BinaryFormatter();
            XmlSerializer xmlsaver = new XmlSerializer(typeof(CGraph));

            using ( Stream stream= new FileStream("graph.g",FileMode.Create,FileAccess.Write)) {
                saver.Serialize(stream,mgraph);
            }

            CGraph deserializedGraph;
            
            using (Stream stream = new FileStream("graph.g", FileMode.Open, FileAccess.Read)) {
                deserializedGraph = (CGraph)saver.Deserialize(stream);
            }

            deserializedGraph.RegisterGraphPrinter(new CGraphVizPrinter(mgraph));
            // The graph uses the registered printers to print the graph to the specified output
            deserializedGraph.Generate(@"D:\MyPrivateWork\MyApps\MyApplications\EDUFLEX\GraphLibrary\TestSerialization\bin\Debug\regentest.dot", true);

            using (Stream stream = new FileStream("graph.xml", FileMode.Create, FileAccess.Write)) {
                xmlsaver.Serialize(stream, mgraph);
            }

        }
    }
}
