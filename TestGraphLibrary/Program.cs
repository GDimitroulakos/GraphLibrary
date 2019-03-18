using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using GraphLibrary;
using GraphLibrary.Aglorithms;
using GraphLibrary.Algorithms;
using GraphLibrary.Generics;


namespace TestGraphLibrary {
    class Program {
        static void Main(string[] args) {
			
            CGraph graph1 = CGraph.CreateGraph();
            CGraph graph2 = CGraph.CreateGraph();
            CGraph rootGraph = CGraph.CreateGraph();

            // CREATE A GRAPH FROM A CSV FILE
            // We call the CreateGraphFromCSV where the edges are given as comma separated
            // tuples in curly brackets. Sole nodes can be given inside curle brackets. This
            // method assigns node labels as given in the file using the native graph labeller
            CGraph graph3 = CGraph.CreateGraphFromCSV("csvGraph.txt");
            CGraph.CCloneGraphOperation cloner = new CGraph.CCloneGraphOperation();
            CGraph graph3Clone = cloner.CloneGraph(graph3);
            

            CGraph.CMergeGraphOperation joiner = new CGraph.CMergeGraphOperation();
            joiner.MergeGraph(graph3);

            CGraph mergedGraph = joiner.MergeGraph(graph3Clone);
            Console.WriteLine("{0}", mergedGraph.ToString());
            // The graph can be printed using the specified graph printer which optionally can
            // take a label contructor. If the label contractor is ommited that printer will recieve
            // labels from the native graph labeller. In this case this what it happens
            graph3.RegisterGraphPrinter(new CGraphVizPrinter(graph3));
            graph3Clone.RegisterGraphPrinter(new CGraphVizPrinter(graph3Clone));
            mergedGraph.RegisterGraphPrinter(new CGraphVizPrinter(mergedGraph));
            // The graph uses the registered printers to print the graph to the specified output
            graph3.Generate(@"E:\MyPrivateWork\MyApps\MyLibraries\GraphLibrary\TestGraphLibrary\bin\Debug\test3.dot", true);
            graph3Clone.Generate(@"E:\MyPrivateWork\MyApps\MyLibraries\GraphLibrary\TestGraphLibrary\bin\Debug\test3Clone.dot", true);
            mergedGraph.Generate(@"E:\MyPrivateWork\MyApps\MyLibraries\GraphLibrary\TestGraphLibrary\bin\Debug\MergeTest.dot", true);
            /*
                        AlgorithmDataRecord ioargs = 
                            GAlg_LeaderFinder_Builder.Create().
                                Input_SourceGraph(graph3).
                            End();

                        GAlg_LeaderFinder bbFinder = new GAlg_LeaderFinder(ioargs);
                        bbFinder.Init();
            /*
                        GAlg_BasicBlocks bbCreate = new GAlg_BasicBlocks(graph3,bbFinder);
                        bbCreate.Init();

                        // We call the CreateGraphFromCSV where the edges are given as comma separated
                        // tuples in curly brackets. Sole nodes can be given inside curle brackets. This
                        // method assigns node labels as given in the file using the native graph labeller
                        CGraph graph4 = CGraph.CreateGraphFromCSV("csvGraph1.txt");

                        graph4.RegisterGraphPrinter(new CGraphVizPrinter(graph4));
                        // The graph uses the registered printers to print the graph to the specified output
                        graph4.Generate(@"E:\MyPrivateWork\MyApps\MyLibraries\GraphLibrary\TestGraphLibrary\bin\Debug\test4.dot", true);

                        GAlg_ExtendedBasicBlockFinder ebbFinder = new GAlg_ExtendedBasicBlockFinder(graph4);
                        ebbFinder.Init();

                        // We call the CreateGraphFromCSV where the edges are given as comma separated
                        // tuples in curly brackets. Sole nodes can be given inside curle brackets. This
                        // method assigns node labels as given in the file using the native graph labeller
                        CGraph graph5 = CGraph.CreateGraphFromCSV("csvGraph2.txt");

                        graph5.RegisterGraphPrinter(new CGraphVizPrinter(graph5));
                        // The graph uses the registered printers to print the graph to the specified output
                        graph5.Generate(@"E:\MyPrivateWork\MyApps\MyLibraries\GraphLibrary\TestGraphLibrary\bin\Debug\csvGraph2.dot", true);

                        GAlg_DFSSpanningTree dfsSpanningTree = new GAlg_DFSSpanningTree(graph5);
                        dfsSpanningTree.Init();

                        dfsSpanningTree.m_dfsSpanningTree.RegisterGraphPrinter(new CDFSSpanningTreeGraphvizPrinter(dfsSpanningTree.m_dfsSpanningTree));
                        dfsSpanningTree.m_dfsSpanningTree.Generate(@"E:\MyPrivateWork\MyApps\MyLibraries\GraphLibrary\TestGraphLibrary\bin\Debug\ST.dot", true);

                        // CREATE A GRAPH FROM RANDOM GRAPH GENERATORS
                        // A random graph with the specified number of nodes and edges can be generated
                        // using the GenerateGraph_RandomGraph method. The nodes are labelled in the native
                        // graph labeller using their serial numbers
                        graph1.GenerateGraph_RandomGraph(10,40,GraphType.GT_UNDIRECTED);
                        // The graph can be printed using the specified graph printer which optionally can
                        // take a label contructor. If the label contractor is ommited that printer will recieve
                        // labels from the native graph labeller. In this case this what it happens
                        graph1.RegisterGraphPrinter(new CGraphVizPrinter(graph1));
                        // The graph uses the registered printers to print the graph to the specified output
                        //graph1.Generate(@"E:\MyPrivateWork\MyApps\MyLibraries\GraphLibrary\TestGraphLibrary\bin\Debug\graph1.dot", true);

                        graph2.GenerateGraph_RandomGraph(10,40,GraphType.GT_UNDIRECTED);
                        graph2.RegisterGraphPrinter(new CGraphVizPrinter(graph2));
                        //graph2.Generate(@"E:\MyPrivateWork\MyApps\MyLibraries\GraphLibrary\TestGraphLibrary\bin\Debug\graph2.dot", true);*/



        }
    }
}
