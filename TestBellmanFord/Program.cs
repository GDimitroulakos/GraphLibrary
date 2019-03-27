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

            CGraphNode x= mgraph.CreateGraphNode<CGraphNode>("x");
            CGraphNode y = mgraph.CreateGraphNode<CGraphNode>("y");
            CGraphNode z = mgraph.CreateGraphNode<CGraphNode>("z");
            CGraphNode t = mgraph.CreateGraphNode<CGraphNode>("t");
            CGraphNode s = mgraph.CreateGraphNode<CGraphNode>("s");

            mgraph.AddGraphEdge<CGraphEdge,CGraphNode>(t, x,GraphType.GT_DIRECTED);
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

            bl.FindAllPairsShortestPaths();

            // 4. Print Paths
            CGraphQueryInfo shortestPath = new CGraphQueryInfo(mgraph,bl);
            CIt_GraphNodes i = new CIt_GraphNodes(mgraph);
            CIt_GraphNodes j = new CIt_GraphNodes(mgraph);
            Dictionary<CGraphNode, Dictionary<CGraphNode, Path>> paths =
                (Dictionary<CGraphNode, Dictionary<CGraphNode, Path>>)(shortestPath.Info());
            for (i.Begin(); !i.End(); i.Next()) {
                Console.WriteLine();
                for (j.Begin(); !j.End(); j.Next()) {
                    Console.WriteLine();
                    if (i.M_CurrentItem != j.M_CurrentItem) {
                        Console.Write(paths[i.M_CurrentItem][j.M_CurrentItem]);
                    }
                }
            }
        }
    }
}
