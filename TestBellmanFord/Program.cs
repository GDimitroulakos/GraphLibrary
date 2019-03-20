using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphLibrary;
using GraphLibrary.Generics;
using GraphLibrary.Algorithms;

namespace TestBellmanFord {
    class Program {
        static void Main(string[] args) {

            // 1. Create a graph
            CGraph mgraph = CGraph.CreateGraph();

            CGraphNode x= mgraph.CreateGraphNode("x");
            CGraphNode y = mgraph.CreateGraphNode("y");
            CGraphNode z = mgraph.CreateGraphNode("z");
            CGraphNode t = mgraph.CreateGraphNode("t");
            CGraphNode s = mgraph.CreateGraphNode("s");

            mgraph.AddGraphEdge(t, x,GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge(t, y, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge(t, z, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge(x, t, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge(y, x, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge(y, z, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge(z, x, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge(z, s, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge(s, t, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge(s, y, GraphType.GT_DIRECTED);

            // 2. Associate weights with the edges of the graph
            CGraphQueryInfo weights = new CGraphQueryInfo(mgraph,255);

            weights.CreateInfo(y,x,-3);
            weights.CreateInfo(x, t, -2);
            weights.CreateInfo(t, x, 5);
            weights.CreateInfo(z, x, 7);
            weights.CreateInfo(y, z, 9);
            weights.CreateInfo(t, y, 8);
            weights.CreateInfo(t, z, -4);
            weights.CreateInfo(s, t, 6);
            weights.CreateInfo(s, y, 7);
            weights.CreateInfo(z, s, 2);

            // 3. Run the BellmanFord algorithm
            BellmanFord bl = new BellmanFord(mgraph,s,255);

            bl.Run();


        }
    }
}
