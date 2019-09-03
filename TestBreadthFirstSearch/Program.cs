using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphLibrary;
using GraphLibrary.Algorithms;
using GraphLibrary.Generics;

namespace TestBreadthFirstSearch {
    class Program {

        public static void TestCaseBook() {
            // 1. Create a graph
            CGraph mgraph = CGraph.CreateGraph();

            CGraphNode r = mgraph.CreateGraphNode<CGraphNode>("r");
            CGraphNode s = mgraph.CreateGraphNode<CGraphNode>("s");
            CGraphNode t = mgraph.CreateGraphNode<CGraphNode>("t");
            CGraphNode u = mgraph.CreateGraphNode<CGraphNode>("u");
            CGraphNode v = mgraph.CreateGraphNode<CGraphNode>("v");
            CGraphNode w = mgraph.CreateGraphNode<CGraphNode>("w");
            CGraphNode x = mgraph.CreateGraphNode<CGraphNode>("x");
            CGraphNode y = mgraph.CreateGraphNode<CGraphNode>("y");

            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(r,s, GraphType.GT_UNDIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(r,v, GraphType.GT_UNDIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(s,w, GraphType.GT_UNDIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(w, t, GraphType.GT_UNDIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(w, x, GraphType.GT_UNDIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(t, x, GraphType.GT_UNDIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(t, u, GraphType.GT_UNDIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(x, u, GraphType.GT_UNDIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(x, y, GraphType.GT_UNDIRECTED);

            // 2. Create Algorithm
            BreadthFirstSearch bfs = new BreadthFirstSearch(s,mgraph);

            // 2. Associate weights with the edges of the graph
            CGraphQueryInfo<int, int, int> bfsData = new CGraphQueryInfo<int, int, int>(mgraph, bfs);

            bfs.Run();

            Console.WriteLine("Printing BFS Results with Source Node : {0}", s.M_Label);
            foreach (CGraphNode node in bfs.BFSNodes()) {
                Console.WriteLine("Node {0} distance: {1}", node.M_Label, bfs.Distance(node));
            }

            // Testing BreadthFirstSearchQueryInfo
            BreadthFirstSearchQueryInfo bfsInfo =new BreadthFirstSearchQueryInfo(mgraph,bfs);
            Console.WriteLine("Printing BFS Results with Source Node : {0}", s.M_Label);
            foreach (CGraphNode node in bfsInfo.BFSNodes()) {
                Console.WriteLine("Node {0} distance: {1}", node.M_Label, bfs.Distance(node));
            }
        }

        static void Main(string[] args) {
            TestCaseBook();
        }
    }
}
