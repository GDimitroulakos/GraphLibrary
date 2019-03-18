using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using GraphLibrary;
using GraphLibrary.Algorithms;
using GraphLibrary.Printers.GraphVizPrinter;

/*namespace GraphLibrary.Aglorithms.BasicBlocks {


   public class GAlg_BasicBlocks : CGraphAlgorithm<int> {
       // Source Info
        private CGraph m_sourceGraph;
        private int m_inputDataKey;
        private List<CGraphNode> m_leaders;

        // Output Info 
        // The outcome : The basic block graph
        private CCondensedGraph m_bbgraph;

        // Context
        // List of basic blocks holding the list of nodes
        private List<List<CGraphNode>> m_basicBlocks = new List<List<CGraphNode>>();

        private List<CGraphNode> m_currentBB;

        private CGraphNode m_currentNode;


        public GAlg_BasicBlocks(AlgorithmDataRecord io_args ) :base() {
           // m_sourceGraph = inputgraph;
           // m_inputDataKey = inputDataKey;
            m_leaders = (List<CGraphNode>) m_sourceGraph[m_inputDataKey];
        }

        
        public override void Init() {

            CIt_GraphNodes it = new CIt_GraphNodes(m_sourceGraph);

            // Initialize node info
            for (it.Begin(); !it.End(); it.Next()) {
                CreateTInfo(m_sourceGraph,it.M_CurrentItem,
                    new GAlg_BasicBlocksInfo(){M_isVisited = false} ,
                    0);
            }

            // Build basic blocks
            foreach (CGraphNode node in m_leaders) {
                // For each leader create a new basic block
                m_currentBB = new List<CGraphNode>();
                // add the new basic block to the lists of basic blocks
                m_basicBlocks.Add(m_currentBB);

                Visit(node);
            }

            // Create basic block graph
            m_bbgraph = CCondensedGraph.CreateGraph(m_sourceGraph, m_basicBlocks);
            m_bbgraph.RegisterGraphPrinter(new CCondensedGraphVizPrinter(m_bbgraph, 
                new CCondensedGraphVizLabelling("cluster",m_bbgraph)));
            m_bbgraph.Generate("basic block graph.dot",true);

        }

        public override int Visit(CGraphNode node) {
            m_currentNode = node;

            // add node to the basic block
            m_currentBB.Add(node);
            Info.M_isVisited = true;

            CIt_Successors it = new CIt_Successors(node);

            // Visit every successor of node that it is not a leader
            for (it.Begin(); !it.End(); it.Next()) {
                if (!NodeInfo(it.M_CurrentItem).M_IsLeader &&
                    !NodeInfo(it.M_CurrentItem).M_isVisited) {
                    Visit(it.M_CurrentItem);
                }
            }


            return 0;
        }

        public GAlg_BasicBlocksInfo Info {
            get { return ((GAlg_BasicBlocksInfo)Info(m_sourceGraph,m_currentNode, m_inputDataKey)); }
        }


        private GAlg_BasicBlocksInfo NodeInfo(CGraphNode node) {
           return ((GAlg_BasicBlocksInfo)Info(m_sourceGraph,node, m_inputDataKey));
        }

        public List<List<CGraphNode>> M_BasicBlocks {
            get { return m_basicBlocks; }
        }
    }

    public class GAlg_BasicBlocksInfo {
        private bool m_isLeader;
        private bool m_isVisited;

        public bool M_IsLeader {
            get { return m_isLeader; }
            set { m_isLeader = value; }
        }

        public bool M_isVisited {
            get { return m_isVisited; }
            set { m_isVisited = value; }
        }
    }

    public class GAlg_Builder<T> {
        
    }

#region Find the leaders

    public class GAlg_LeaderFinder_Builder : GAlg_Builder<GAlg_LeaderFinder_Builder> {
        private CGraph m_sourceGraph;

        private AlgorithmDataRecord m_ioArgs;

        private GAlg_LeaderFinder_Builder() {
            m_ioArgs = new AlgorithmDataRecord();
        }

        public GAlg_LeaderFinder_Builder Input_SourceGraph(CGraph sourceGraph) {
            m_sourceGraph = sourceGraph;
            m_ioArgs.AddInputArgument("inputGraph", sourceGraph);
            return this;
        }

        public AlgorithmDataRecord End() {
            return m_ioArgs;
        }

        public void Exe() {
            
        }

        public static GAlg_LeaderFinder_Builder Create() {
            return new GAlg_LeaderFinder_Builder();
        }
    }



    public class GAlg_LeaderFinder : CGraphAlgorithm<int, GAlg_LeaderFinder_Builder> {
        
        // Source Info
        private CGraph m_sourceGraph;

        // Output Info
        private CGraph m_outputGraph;
        private List<CGraphNode> m_leaders = new List<CGraphNode>();

        // Context
        private CGraphNode m_currentNode;
        private CGraphNode m_root; 

        
        public GAlg_LeaderFinder() {
            /*m_outputGraph = m_sourceGraph = io_args.GetIArgumentOriginGraph();
            AddSourceGraph(m_sourceGraph);
            AddOutputGraph(m_outputGraph);#1#
        }
        
        public override GAlg_LeaderFinder_Builder SetInput() {
            GAlg_LeaderFinder_Builder x = GAlg_LeaderFinder_Builder.Create();

            return x;
        }

        public override void Init() {
            CIt_GraphNodes it = new CIt_GraphNodes(m_sourceGraph);

            m_sourceGraph.SetNodeLabelContext(m_sourceGraph);

            CreateTInfo(new GAlg_LeaderFinder_OGraphInfo(),m_sourceGraph);
            for (it.Begin(); !it.End(); it.Next()) {
                // Initialize node information
                if (it.M_CurrentItem.M_NumberOfPredecessors != 0) {
                    // Mark other nodes as not leaders
                    CreateTmpInfo(new GAlg_LeaderFinder_TNodeInfo()
                    { M_isVisited = false }, it.M_CurrentItem, m_sourceGraph);
                    CreateTInfo(new GAlg_LeaderFinder_ONodeInfo()
                    {  M_IsLeader= false }, it.M_CurrentItem, m_sourceGraph);
                }
                else {
                    // Root nodes are a leaders
                    CreateTmpInfo(new GAlg_LeaderFinder_TNodeInfo()
                    { M_isVisited = false }, it.M_CurrentItem, m_sourceGraph);
                    m_root = it.M_CurrentItem;
                    CreateTInfo(new GAlg_LeaderFinder_ONodeInfo()
                    { M_IsLeader = true }, it.M_CurrentItem, m_sourceGraph);
                }
            }
            m_leaders.Add(m_root);
            OGraphInfo().M_Leaders = m_leaders;
            Visit(m_root);
            foreach (CGraphNode node in m_leaders) {
                Console.WriteLine($"Found leader:{node.M_Label} ");
            }
            // Create basic block graph
        }

        public override int Visit(CGraphNode node) {
            m_currentNode = node; // MANDATORY for Info property to work properly

            // Mark node as visited
            TmpNodeInfo().M_isVisited = true;

            // Check for join and nodes after a fork node as they are the leaders
            if (node.M_NumberOfPredecessors > 1) {
                // The case of a join node
                ONodeInfo(node).M_IsLeader = true;
                m_leaders.Add(node);
            }
            else if (node.M_NumberOfPredecessors == 1 && node.Predeccessor(0).M_NumberOfSuccessors > 1) {
                // The case of a node followed by a fork node
                ONodeInfo(node).M_IsLeader = true;
                m_leaders.Add(node);
            }

            CIt_Successors it  = new CIt_Successors(node);
            for (it.Begin(); !it.End(); it.Next()) {
                if (!TmpNodeInfo(it.M_CurrentItem).M_isVisited) {
                    Visit(it.M_CurrentItem);
                }
            }
            return 0;
        }

        public GAlg_LeaderFinder_TNodeInfo TmpNodeInfo() {
             return ((GAlg_LeaderFinder_TNodeInfo) TmpInfo(m_sourceGraph,m_currentNode)); 
        }
        public GAlg_LeaderFinder_TNodeInfo TmpNodeInfo(CGraphNode node) {
             return ((GAlg_LeaderFinder_TNodeInfo)TmpInfo(m_sourceGraph, node)); 
        }

        private GAlg_LeaderFinder_ONodeInfo ONodeInfo(CGraphNode node) {
            return ((GAlg_LeaderFinder_ONodeInfo)TInfo(m_sourceGraph,node));
        }
        private GAlg_LeaderFinder_OGraphInfo OGraphInfo() {
            return (GAlg_LeaderFinder_OGraphInfo) TInfo(m_sourceGraph);
        }

        public List<CGraphNode> M_Leaders {
            get { return m_leaders; }
        }

        public class GAlg_LeaderFinder_TNodeInfo {
            
            private bool m_isVisited;

            public bool M_isVisited {
                get { return m_isVisited; }
                set { m_isVisited = value; }
            }
        }

        public class GAlg_LeaderFinder_ONodeInfo {
            private bool m_isLeader;

            public bool M_IsLeader {
                get { return m_isLeader; }
                set { m_isLeader = value; }
            }
        }

        public class GAlg_LeaderFinder_OGraphInfo {
            private List<CGraphNode> m_leaders=null;

            public List<CGraphNode> M_Leaders {
                get { return m_leaders; }
                set { m_leaders = value; }
            }
        }

    }

    
    #endregion
}*/
