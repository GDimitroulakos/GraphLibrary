using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphLibrary;
using GraphLibrary.Algorithms;
using GraphLibrary.Generics;

namespace TestDepthFirstSearch {
    class Program {

        public static void TestCaseBook() {
            // 1. Create a graph
            CGraph mgraph = CGraph.CreateGraph();

            CGraphNode u = mgraph.CreateGraphNode<CGraphNode>("u");
            CGraphNode v = mgraph.CreateGraphNode<CGraphNode>("v");
            CGraphNode w = mgraph.CreateGraphNode<CGraphNode>("w");
            CGraphNode x = mgraph.CreateGraphNode<CGraphNode>("x");
            CGraphNode y = mgraph.CreateGraphNode<CGraphNode>("y");
            CGraphNode z = mgraph.CreateGraphNode<CGraphNode>("z");

            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(u, v, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(u, x, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(x, v, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(v, y, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(y, x, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(w, y, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(w, z, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(z, z, GraphType.GT_DIRECTED);

            DepthFirstSearch dfs = new DepthFirstSearch(mgraph);

            dfs.Run();

            DepthFirstSearchQueryInfo info = new DepthFirstSearchQueryInfo(mgraph, dfs);
            CIt_GraphNodes it = new CIt_GraphNodes(mgraph);
            for (it.Begin(); !it.End(); it.Next()) {
                Console.WriteLine("Node {0}: arrival ({1}) - departure ({2})",
                    it.M_CurrentItem.M_Label, info.Arrival(it.M_CurrentItem), info.Departure(it.M_CurrentItem));
            }


        } 

        static void Main(string[] args) {
            TestCaseBook();
        }
    }
}
