using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using GraphLibrary.Algorithms;
using GraphLibrary.Generics;

namespace GraphLibrary.Aglorithms{

    /// <summary>
    /// This algorithm ( visitor ) performs a DFS traversal over a graph (directed
    /// or undirected ). It records the paths from which the algorithm passes by 
    /// coloring nodes along them. The algorith works also for non-connected graphs
    /// 1) White (0) : Not  yet visited 
    /// 2) Gray (1) : Visited but not all of its neighbours
    /// 3) Black (2) : Visited and all of its neighbours  
    /// </summary>
    public class GAlg_EdgeOrientedDFS : CGraphAlgorithm<CGraphEdge> {

        /// <summary>
        /// Initializes a new instance of the <see cref="GAlg_EdgeOrientedDFS"/> class.
        /// </summary>
        /// <param name="graph">The graph.</param>
        public GAlg_EdgeOrientedDFS(AlgorithmDataRecord io_args) : base(io_args) { }

        public override void SetAlgorithmicInterface(AlgorithmDataRecord info) {
            throw new NotImplementedException();
        }

        public override void Init(){
            //Initialize the algorithm information
            CIt_GraphEdges itg = new CIt_GraphEdges(m_sourceGraphs[0]);

            for (itg.Begin(); !itg.End(); itg.Next()) {
                // Mark the edges unvisited
                //CreateInfo(itg.M_CurrentItem,new EdgeInfo_DFS(0),this);
            }

            // Iterate over all possible edges of the graph
            for (itg.Begin(); !itg.End(); itg.Next()) {
                Visit(itg.M_CurrentItem);
            }
        }

        /// <summary>
        /// The Visit method refers to the action the algorithm performs to a specific edge
        /// Most of the times the Visit function is called recursively.
        /// </summary>
        /// <param name="edge"></param>
        /// <returns>
        /// The parameter refers to the type of the return result
        /// </returns>
        public override CGraphEdge Visit(CGraphEdge edge) {
            CIt_Successors itn = new CIt_Successors(edge.M_Target); 
            CGraphEdge visitedEdge;

            // Mark the edge as visited
            SetColor(edge,1);

            for (itn.Begin(); !itn.End(); itn.Next()) {
                //visitedEdge = Edge(edge.M_Target, itn.M_CurrentItem);
                //if (Color(visitedEdge) == 0) {
                //    Visit(visitedEdge);
                //}
            }
            return edge;
        }

        #region Information Queries

        /// <summary>
        /// Returns the color of the specified node
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>
        /// [int]: Returns the color of the specified node
        /// </returns>
        public  int Color(CGraphEdge edge) {
            return ((EdgeInfo_DFS)Info(edge,this)).m_color;          
        }

        /// <summary>
        /// Sets the color of the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="color">The color.</param>
         public void SetColor(CGraphEdge edge, int color) {
             EdgeInfo_DFS info = (EdgeInfo_DFS)Info(edge, this);
             info.m_color = color;
         }
        #endregion
    }

    public class ProcessBeforeEventArgs : EventArgs {
        public CGraphNode M_Node { get; set; }
    }

    public class ProcessAfterEventArgs : EventArgs {
        public CGraphNode M_Node { get; set; }
    }

    public class ProcessSuccBeforeEventArgs : EventArgs {
        public CGraphNode M_Node { get; set; }
        public CGraphNode M_Successor { get; set; }
    }

    public class ProcessSuccAfterEventArgs : EventArgs {
        public CGraphNode M_Node { get; set; }
        public CGraphNode M_Successor { get; set; }

    }

    /// <summary>
    /// This algorithm ( visitor ) performs a DFS traversal over a graph (directed
    /// or undirected ). It records the paths from which the algorithm passes by 
    /// coloring nodes along them. The algorith works also for non-connected graphs
    /// 1) White (0) : Not  yet visited 
    /// 2) Gray (1) : Visited but not all of its neighbours
    /// 3) Black (2) : Visited and all of its neighbours  
    /// </summary>
    public class GAlg_NodeOrientedDFS : CGraphAlgorithm<CGraphNode> {
        // Initiates DFS from nodes having no predecessors
        private bool m_startAtRootNodes = false;

        // Inform other classes for the DFS algorithm's events
        // Events are built according to :
        // https://msdn.microsoft.com/en-us/library/edzehd2t(v=vs.110).aspx
        public event EventHandler e_Init;
        public event EventHandler<ProcessBeforeEventArgs> e_ProcessBefore;
        public event EventHandler<ProcessAfterEventArgs> e_ProcessAfter;
        public event EventHandler<ProcessSuccBeforeEventArgs> e_ProcessSuccBefore;
        public event EventHandler<ProcessSuccAfterEventArgs> e_ProcessSuccAfter;
        public event EventHandler e_BypassNode;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="graph">Graph to which the algorithm is applied</param>
        public GAlg_NodeOrientedDFS(AlgorithmDataRecord io_args) : base(io_args) {
            e_Init += eventHandler1;

        }

        public override void SetAlgorithmicInterface(AlgorithmDataRecord info) {
            throw new NotImplementedException();
        }

        protected virtual void OnInit(EventArgs args) {
            if (e_Init != null) {
                e_Init(this, args);
            }
        }

        protected virtual void OnProcessBefore(ProcessBeforeEventArgs args) {
            if (e_ProcessBefore != null) {
                e_ProcessBefore(this, args);
            }
        }

        protected virtual void OnProcessAfter(ProcessAfterEventArgs args) {
            if (e_ProcessAfter != null) {
                e_ProcessAfter(this, args);
            }
        }

        protected virtual void OnProcessSuccBefore(ProcessSuccBeforeEventArgs args) {
            if (e_ProcessSuccBefore != null) {
                e_ProcessSuccBefore(this, args);
            }
        }

        protected virtual void OnProcessSuccAfter(ProcessSuccAfterEventArgs args) {
            if (e_ProcessSuccAfter != null) {
                e_ProcessSuccAfter(this, args);
            }
        }

        protected virtual void OnBypassNode(EventArgs args) {
            if (e_BypassNode != null) {
                e_BypassNode(this, args);
            }
        }

        public override void Init() {
           
            AbstractGraphIterator<CGraphNode> it1;
            if (m_startAtRootNodes){
                it1 = new CIt_GraphRootNodes(m_sourceGraphs[0]);
            }
            else{
                it1 = new CIt_GraphNodes(m_sourceGraphs[0]);
            }

            // Paint the nodes white color (0)
            for (it1.Begin(); !it1.End(); it1.Next()){
               //CreateInfo(it1.M_CurrentItem, new NodeInfo_DFS(0),this);
            }

            // Start at a random node. If the graph is not completely traversed
            // the Visit function will return and another node ( not already 
            // visited ) will be used as starting node. This works fine for 
            // undirected graphs. For tree graphs it is recomended to start
            // from the tree root node because a different visiting pattern is
            // produced for different starting nodes
            for (it1.Begin(); !it1.End(); it1.Next()){
                if (Color(it1.M_CurrentItem) == 0) {
                    Visit(it1.M_CurrentItem);
                }
            }
            
            // EVENT : Raising the OnInit event
            OnInit(EventArgs.Empty);
        }

        public override CGraphNode Visit(CGraphNode node) {
            CIt_Successors it = new CIt_Successors(node);
            
            // Paint the node gray (1) at first visit
            SetColor(node,1);

            // EVENT : Raising on OnNodeEnter
            OnProcessBefore(new ProcessBeforeEventArgs(){M_Node = node});
            
            // Visit adjacent nodes
            for (it.Begin(); !it.End(); it.Next()) {
                if (Color(it.M_CurrentItem) == 0) {
                    // VISIT WHITE NODES
                    OnProcessSuccBefore(new ProcessSuccBeforeEventArgs() {
                        M_Node = node, M_Successor = it.M_CurrentItem});
                    Visit(it.M_CurrentItem);
                    OnProcessSuccAfter(new ProcessSuccAfterEventArgs() {
                        M_Node = node, M_Successor = it.M_CurrentItem
                    });
                }
                else {
                    // EVENT : Raise the OnBypassingNode event
                    OnBypassNode(EventArgs.Empty);
                }
            }
            // Paint the node black (2) when all adjacent nodes are visited
            SetColor(node, 2);

            // EVENT : Raising on OnNodeExit
            OnProcessAfter(new ProcessAfterEventArgs(){M_Node = node});

            return node;
        }

        /// <summary>
        /// Indicates whether the DFS start from nodes without predecessors
        /// </summary>
        /// <value>
        /// <c>true</c> if the algorithm starts at nodes without predecessors;
        /// otherwise, <c>false</c>.
        /// </value>
        public bool M_StartAtRootNodes {
            get { return m_startAtRootNodes; }
            set { m_startAtRootNodes = value; }
        }

        #region Information Queries

        /// <summary>
        /// Returns the color of the specified node
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>
        /// [int]: Returns the color of the specified node
        /// </returns>
        public  int Color(CGraphNode node) {
            return ((NodeInfo_DFS)Info(node,this)).m_color;          
        }

        /// <summary>
        /// Sets the color of the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="color">The color.</param>
         public void SetColor(CGraphNode node, int color) {
             NodeInfo_DFS info = (NodeInfo_DFS)Info(node,this);
             info.m_color = color;
         }
        #endregion

         /// <summary>
         /// Returns the specified node information. 
         /// </summary>
         /// <param name="node">The node.</param>
         /// <param name="key">The key.</param>
         /// <returns></returns>
        public NodeInfo_DFS NodeInfo(CGraphNode node, object key) {
            //return (NodeInfo_DFS) Info(m_sourceGraphs[0],node, key);
        }

        public void eventHandler1(object sender, EventArgs arg) {
            
        }
        
    }

    public enum EdgeTypeDFS { na, tree, forward, back, cross}

    public class GAlg_DFSSpanningTree : CGraphAlgorithm<CGraphNode> {
        public CDFSSpanningTree m_dfsSpanningTree;

        private CIntergraphElementDependences m_Mappping;

        
        private int m_preorderCounter = 0;

        private int m_postorderCounter = 0;

        public GAlg_DFSSpanningTree(AlgorithmDataRecord io_args) : base(io_args) {
            m_Mappping = new CIntergraphElementDependences();
        }

        public override void SetAlgorithmicInterface(AlgorithmDataRecord info) {
            throw new NotImplementedException();
        }

        public override void Init() {
            CGraphNode newSTNode;
            m_dfsSpanningTree = CDFSSpanningTree.CreateGraph();
            CIt_GraphNodes it = new CIt_GraphNodes(m_sourceGraphs[0]);
            CIt_GraphRootNodes it1 = new CIt_GraphRootNodes(m_sourceGraphs[0]);
           

            // Paint the nodes white color (0)
            for (it.Begin(); !it.End(); it.Next()){
                //CreateInfo(it.M_CurrentItem, 
                //    new NodeInfo_DFS(){ m_color = 0},
                //    this);
                newSTNode = m_dfsSpanningTree.CreateGraphNode("L" + it.M_CurrentItem.M_Label);
                newSTNode[m_dfsSpanningTree] = new NodeInfo_DFSSpanningTree() {M_Preorder = 0, M_Postorder = 0};
                m_Mappping.SetOriginalToDerivedNode(it.M_CurrentItem,newSTNode);
            }

            // Start at a random node. If the graph is not completely traversed
            // the Visit function will return and another node ( not already 
            // visited ) will be used as starting node. This works fine for 
            // undirected graphs. For tree graphs it is recomended to start
            // from the tree root node because a different visiting pattern is
            // produced for different starting nodes
            for (it1.Begin(); !it1.End(); it1.Next()){
                if (Color(it1.M_CurrentItem) == 0){
                    Visit(it1.M_CurrentItem);
                }
            }
        }

        public override CGraphNode Visit(CGraphNode node) {
            CGraphEdge newSTedge;
            // Paint the node gray (1) at first visit
            SetColor(node,1);

            SetPreOrderNumber(m_Mappping.DerivedNode(node),++m_preorderCounter);

            CIt_Successors it = new CIt_Successors(node);
            for (it.Begin(); !it.End(); it.Next()) {
                if (Color(it.M_CurrentItem) == 0) {
                    Visit(it.M_CurrentItem);
                    newSTedge=m_dfsSpanningTree.AddGraphEdge(m_Mappping.DerivedNode(node), m_Mappping.DerivedNode(it.M_CurrentItem), EdgeTypeDFS.tree);
                    m_Mappping.SetOriginalToDerivedEdge(m_sourceGraphs[0].Edge(node, it.M_CurrentItem),newSTedge);
                }
                else if (PreOrderNumber(m_Mappping.DerivedNode(node)) < PreOrderNumber(m_Mappping.DerivedNode(it.M_CurrentItem))) {
                    // The successor has already been visited from another path prior to the current node
                    newSTedge=m_dfsSpanningTree.AddGraphEdge(m_Mappping.DerivedNode(node), m_Mappping.DerivedNode(it.M_CurrentItem), EdgeTypeDFS.forward);
                    m_Mappping.SetOriginalToDerivedEdge(m_sourceGraphs[0].Edge(node, it.M_CurrentItem), newSTedge);
                }
                else if (PostOrderNumber(m_Mappping.DerivedNode(it.M_CurrentItem)) == 0) {
                    // The successor (which is an actual predecessor) has already been visited and not assigned a postorder number
                    // which means that both nodes have been visited from the same path. Hence it is a backedge and not a cross edge... 
                    newSTedge=m_dfsSpanningTree.AddGraphEdge(m_Mappping.DerivedNode(node), m_Mappping.DerivedNode(it.M_CurrentItem), EdgeTypeDFS.back);
                    m_Mappping.SetOriginalToDerivedEdge(m_sourceGraphs[0].Edge(node, it.M_CurrentItem), newSTedge);
                }
                else {
                    //...otherwise it is a cross edge
                    newSTedge=m_dfsSpanningTree.AddGraphEdge(m_Mappping.DerivedNode(node), m_Mappping.DerivedNode(it.M_CurrentItem), EdgeTypeDFS.cross);
                    m_Mappping.SetOriginalToDerivedEdge(m_sourceGraphs[0].Edge(node, it.M_CurrentItem), newSTedge);
                }
            }

            SetPostOrderNumber(m_Mappping.DerivedNode(node),++m_postorderCounter);

            return node;
        }

        protected int Color(CGraphNode node) {
            return ((NodeInfo_DFS) Info(node, this)).m_color;
        }

        protected void SetColor(CGraphNode node, int color) {
            ((NodeInfo_DFS) Info(node, this)).m_color = color;
        }

        protected int PreOrderNumber(CGraphNode node) {
            return ((NodeInfo_DFSSpanningTree) node[m_dfsSpanningTree]).M_Preorder;
        }

        protected void SetPreOrderNumber(CGraphNode node, int num) {
            ((NodeInfo_DFSSpanningTree)node[m_dfsSpanningTree]).M_Preorder = num;
        }

        protected int PostOrderNumber(CGraphNode node) {
            return ((NodeInfo_DFSSpanningTree)node[m_dfsSpanningTree]).M_Postorder;
        }

        protected void SetPostOrderNumber(CGraphNode node, int num) {
            ((NodeInfo_DFSSpanningTree)node[m_dfsSpanningTree]).M_Postorder = num;
        }
    }
}
