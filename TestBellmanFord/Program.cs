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

        public static void TestCase3x3() {
            // 1. Create a graph
            CGraph mgraph = CGraph.CreateGraph();

            CGraphNode x1 = mgraph.CreateGraphNode<CGraphNode>("1");
            CGraphNode x2 = mgraph.CreateGraphNode<CGraphNode>("2");
            CGraphNode x3 = mgraph.CreateGraphNode<CGraphNode>("3");
            CGraphNode x4 = mgraph.CreateGraphNode<CGraphNode>("4");
            CGraphNode x5 = mgraph.CreateGraphNode<CGraphNode>("5");
            CGraphNode x6 = mgraph.CreateGraphNode<CGraphNode>("6");
            CGraphNode x7 = mgraph.CreateGraphNode<CGraphNode>("7");
            CGraphNode x8 = mgraph.CreateGraphNode<CGraphNode>("8");
            CGraphNode x9 = mgraph.CreateGraphNode<CGraphNode>("9");

            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(x1, x2, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(x2, x1, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(x1, x5, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(x5, x1, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(x1, x4, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(x4, x1, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(x2, x4, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(x4, x2, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(x2, x3, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(x3, x2, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(x2, x6, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(x6, x2, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(x2, x5, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(x5, x2, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(x3, x5, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(x5, x3, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(x3, x6, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(x6, x3, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(x4, x5, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(x5, x4, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(x4, x7, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(x7, x4, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(x4, x8, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(x8, x4, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(x5, x6, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(x6, x5, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(x5, x7, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(x7, x5, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(x5, x8, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(x8, x5, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(x5, x9, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(x9, x5, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(x6, x8, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(x8, x6, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(x6, x9, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(x9, x6, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(x7, x8, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(x8, x7, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(x8, x9, GraphType.GT_DIRECTED);
            mgraph.AddGraphEdge<CGraphEdge, CGraphNode>(x9, x8, GraphType.GT_DIRECTED);


            // 2. Associate weights with the edges of the graph
            CGraphQueryInfo<int, int, int> weights = new CGraphQueryInfo<int,int,int>(mgraph, 255);

            weights.CreateInfo(x1, x2, 1);
            weights.CreateInfo(x2, x1, 1);
            weights.CreateInfo(x1, x5, 1);
            weights.CreateInfo(x5, x1, 1);
            weights.CreateInfo(x1, x4, 1);
            weights.CreateInfo(x4, x1, 1);
            weights.CreateInfo(x2, x4, 3);
            weights.CreateInfo(x4, x2, 3);
            weights.CreateInfo(x2, x3, 1);
            weights.CreateInfo(x3, x2, 1);
            weights.CreateInfo(x2, x6, 3);
            weights.CreateInfo(x6, x2, 3);
            weights.CreateInfo(x2, x5, 1);
            weights.CreateInfo(x5, x2, 1);
            weights.CreateInfo(x3, x5, 3);
            weights.CreateInfo(x5, x3, 3);
            weights.CreateInfo(x3, x6, 1);
            weights.CreateInfo(x6, x3, 1);
            weights.CreateInfo(x4, x5, 1);
            weights.CreateInfo(x5, x4, 1);
            weights.CreateInfo(x4, x7, 1);
            weights.CreateInfo(x7, x4, 1);
            weights.CreateInfo(x4, x8, 3);
            weights.CreateInfo(x8, x4, 3);
            weights.CreateInfo(x5, x6, 1);
            weights.CreateInfo(x6, x5, 1);
            weights.CreateInfo(x5, x7, 3);
            weights.CreateInfo(x7, x5, 3);
            weights.CreateInfo(x5, x8, 1);
            weights.CreateInfo(x8, x5, 1);
            weights.CreateInfo(x5, x9, 3);
            weights.CreateInfo(x9, x5, 3);
            weights.CreateInfo(x6, x8, 3);
            weights.CreateInfo(x8, x6, 3);
            weights.CreateInfo(x6, x9, 1);
            weights.CreateInfo(x9, x6, 1);
            weights.CreateInfo(x7, x8, 1);
            weights.CreateInfo(x8, x7, 1);
            weights.CreateInfo(x8, x9, 1);
            weights.CreateInfo(x9, x8, 1);

            // 3. Run the BellmanFord algorithm
            BellmanFord bl = new BellmanFord(mgraph, x1, 255);

            bl.FindAllPairsShortestPaths();

            // 4. Print Paths
            BellmanFordQueryInfo shortestPath = new BellmanFordQueryInfo(mgraph, bl);
            CIt_GraphNodes i = new CIt_GraphNodes(mgraph);
            CIt_GraphNodes j = new CIt_GraphNodes(mgraph);
            Dictionary<CGraphNode, Dictionary<CGraphNode, Path>> paths =
                shortestPath.ShortestPaths();
            for (i.Begin(); !i.End(); i.Next())
            {
                Console.WriteLine();
                for (j.Begin(); !j.End(); j.Next())
                {
                    Console.WriteLine();
                    if (i.M_CurrentItem != j.M_CurrentItem)
                    {
                        Console.Write(paths[i.M_CurrentItem][j.M_CurrentItem]);
                    }
                }
            }

        }

        public static void BookExampleTestcase() {
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

            // 3. Run the BellmanFord algorithm
            BellmanFord bl = new BellmanFord(mgraph, s, 255);

            bl.FindAllPairsShortestPaths();

            // 4. Print Paths
            CGraphQueryInfo<int, int, Dictionary<CGraphNode, Dictionary<CGraphNode, Path>>> shortestPath = new CGraphQueryInfo<int, int, Dictionary<CGraphNode, Dictionary<CGraphNode, Path>>>(mgraph, bl);
            CIt_GraphNodes i = new CIt_GraphNodes(mgraph);
            CIt_GraphNodes j = new CIt_GraphNodes(mgraph);
            Dictionary<CGraphNode, Dictionary<CGraphNode, Path>> paths =
                (Dictionary<CGraphNode, Dictionary<CGraphNode, Path>>)(shortestPath.Info());
            for (i.Begin(); !i.End(); i.Next())
            {
                Console.WriteLine();
                for (j.Begin(); !j.End(); j.Next())
                {
                    Console.WriteLine();
                    if (i.M_CurrentItem != j.M_CurrentItem)
                    {
                        Console.Write(paths[i.M_CurrentItem][j.M_CurrentItem]);
                    }
                }
            }
        }

        static void Main(string[] args) {
            TestCase3x3();
        }
    }
}
