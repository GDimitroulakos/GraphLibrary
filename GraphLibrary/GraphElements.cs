using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using GraphLibrary.Generics;

#pragma warning disable 1587

/// <summary>
/// Defines 5 things:
/// 1) Port Mode Enumeration : Its an enumeration that describes the node port direction
/// It has 3 distinct values SOURCE, TARGET (directed graphs ) and TWOWAY (undirected graphs )
/// 2) CGraphNode class : The base class for graph node modelling.
/// 3) CNodePort class : The base class for port modelling
/// 4) CGraphEdge : The base class for edge modelling
/// 5) CGraph : The base class for graph modelling
///
/// Glossary:
/// Native Edges: Edges that belong to the same graph
///
/// TODO (1): TYPED NODES
/// TODO (1): What extension do we need to support typed nodes. As I understand we need some
/// TODO (1): kind of member representing the type of node. This member variable has an impact
/// TODO (1): on the Accept method. Depending on the type the appropriate Visit method must be
/// TODO (1): called. Thus we need a function to identify the node type an call the appropriate
/// TODO (1): Visit() method. Thus the interface must also contain an Accept function that will
/// TODO (1): override the base function
///
/// </summary>
namespace GraphLibrary {

    #region Graph Node
    /// <summary>
    /// Represents a graph node.
    /// Labelling : The node can be labelled in 4 ways:
    /// 1) By default during construction and the label is stored in the default graph labeller object
    /// 2) Exclusively by setting its label with the SetLabel(string) <see cref="SetLabel(string)"/> method
    /// The label in this case is stored in the default graph labeller object
    /// 3) Exclusively by setting its label with the SetLabel(CGraphLabelling) <see cref="SetLabel(CGraphLabeling<CGraphNode>)"/> method
    /// The label in this case is taken from the specified labeller object 
    /// 4) Globally by the graph object using the function SetNodeLabelContext <see cref="CGraph.SetNodeLabelContext"/>
    /// by using the appropriate labeller object or the default labeller object
    /// 
    /// Uniqueness : A node can be a member a single graph ( Simpliest case )
    /// </summary>
    [Serializable]
    public class CGraphNode : CGraphPrimitive {
        /// <summary>
        /// Stores the outgoing edges 
        /// </summary>
        private List<CGraphEdge> m_OutgoingEdges;

        /// <summary>
        /// Stores the ingoing edges 
        /// </summary>
        private List<CGraphEdge> m_IngoingEdges;

        /// <summary>
        /// Stores the successor nodes of the current node
        /// </summary>
        private List<CGraphNode> m_Successors;

        /// <summary>
        /// Stores the predecessor nodes of the current node
        /// </summary>
        private List<CGraphNode> m_Predecessors;

        // <summary>
        /// Initializes a new instance of the <see cref="CGraphNode"/> class.
        /// It is called by the graph class
        /// </summary>
        /// <param name="ownerGraph">The owner graph.</param>
        public CGraphNode(CGraph ownerGraph) : base(GraphElementType.ET_NODE, ownerGraph) {

            // Native graph information
            m_Predecessors = new List<CGraphNode>();
            m_Successors = new List<CGraphNode>();
            m_IngoingEdges = new List<CGraphEdge>();
            m_OutgoingEdges = new List<CGraphEdge>();
        }

        /// <summary>
        /// No argument contructor
        /// </summary>
        public CGraphNode() : this(null) {
        }

        // It is called by the CGraph object while inserting an edge to the graph. It updates
        // the relevant member variables of the class for the current node
        internal void AddOutgoingGraphEdge(CGraphEdge edge) {
            // Since the edge is outgoing the source of the edge is the current node
            // hence, the edge.target node belongs to a successor of the current node
            m_Successors.Add(edge.M_Target);
            m_OutgoingEdges.Add(edge);
            // if the graph is undirected the edge.target also belongs to the
            // predecessors of the node since the direction is non-determined
            if (edge.M_EdgeType == GraphType.GT_UNDIRECTED) {
                m_Predecessors.Add(edge.M_Target);
                m_IngoingEdges.Add(edge);
            }
        }

        /// <summary>
        /// Removes the outgoing graph edge. It is called by the CGraph object while
        /// deleting an edge to the graph. It updates the relevant member variables
        /// of the class for the current node
        /// </summary>
        /// <param name="edge">The edge to be removed</param>
        internal void RemoveOutgoingGraphEdge(CGraphEdge edge) {
            // Since the edge is outgoing this node is the source of the edge
            // hence, the the edge.target node belongs to a successor of the current node
            m_Successors.Remove(edge.M_Target);
            m_OutgoingEdges.Remove(edge);
            // if the graph is undirected the edge.target also belongs to the
            // predecessors of the node since the direction is non-determined
            if (edge.M_EdgeType == GraphType.GT_UNDIRECTED) {
                m_Predecessors.Remove(edge.M_Target);
                m_IngoingEdges.Remove(edge);
            }
        }

        /// <summary>
        /// Removes the ingoing graph edge. It is called by the CGraph object while
        /// deleting an edge to the graph. It updates the relevant member variables
        /// of the class
        /// </summary>
        /// <param name="edge">The edge to be removed</param>
        internal void RemoveIngoingGraphEdge(CGraphEdge edge) {
            // Since the edge is ingoing the target of the edge is the current node
            // hence, the  edge.source node belongs to the predecessors of the current node
            m_Predecessors.Remove(edge.M_Source);
            m_IngoingEdges.Remove(edge);
            // if the graph is undirected the edge.source also belongs to the
            // predecessors of the node since the direction is non-determined
            if (edge.M_EdgeType == GraphType.GT_UNDIRECTED) {
                m_Successors.Remove(edge.M_Source);
                m_OutgoingEdges.Remove(edge);
            }
        }

        // It is called by the CGraph object while inserting an edge to the graph. It updates
        // the relevant member variables of the class
        internal void AddIngoingGraphEdge(CGraphEdge edge) {
            // Since the edge is ingoing the target of the edge is the current node
            // hence, the  edge.source node belongs to the predecessors of the current node
            m_Predecessors.Add(edge.M_Source);
            m_IngoingEdges.Add(edge);
            // if the graph is undirected the edge.source also belongs to the
            // predecessors of the node since the direction is non-determined
            if (edge.M_EdgeType == GraphType.GT_UNDIRECTED) {
                m_Successors.Add(edge.M_Source);
                m_OutgoingEdges.Add(edge);
            }
        }

        /// <summary>
        /// Returns the i-th successor in the list
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Returns the successor at the given sequence index or null
        /// if index >= NumberOfSuccessors </returns>
        public CGraphNode Successor(int index) {
            if (index < m_Successors.Count) {
                return m_Successors[index];
            }
            else {
                return null;
            }
        }

        public List<CGraphEdge> OutgoingEdges {
            get { return m_OutgoingEdges; }
        }

        /// <summary>
        /// Returns the i-th predecessor in the list of predecessors
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public CGraphNode Predeccessor(int index) {
            if (index < m_Predecessors.Count) {
                return m_Predecessors[index];
            }
            else {
                return null;
            }
        }

        public List<CGraphEdge> IncomingEdges {
            get { return m_IngoingEdges; }
        }



        #region Node Properties

        /// <summary>
        /// Returns the number of native (nodes that belong to the same graph
        /// as the current node) successors
        /// </summary>
        public int M_NumberOfSuccessors {
            get {
                return m_Successors.Count;
            }
        }

        /// <summary>
        /// Returns the Node's native successors in a list. The list is allocated
        /// inside the getter's code
        /// </summary>
        public List<CGraphNode> M_Successors {
            get {
                List<CGraphNode> successors = new List<CGraphNode>();
                foreach (CGraphNode suc in m_Successors) {
                    successors.Add(suc);
                }
                return successors;
            }
        }

        /// <summary>
        /// Returns the number of native (nodes that belong to the same graph
        /// as the current node) predecessors
        /// </summary>
        public int M_NumberOfPredecessors {
            get {
                return m_Predecessors.Count;
            }
        }

        /// <summary>
        /// Returns the Node's native predecessors in a list
        /// </summary>
        public List<CGraphNode> M_Predecessors {
            get {
                List<CGraphNode> predecessors = new List<CGraphNode>();
                foreach (CGraphNode pred in m_Predecessors) {
                    predecessors.Add(pred);
                }
                return predecessors;
            }
        }

        #endregion

        /// <summary>
        /// Sets the label for the current node using the name specified
        /// in the given labeller. After this call the node exposes the 
        /// given label from the M_Label property. This function is better
        /// to be called for all graph nodes in order to avoid complexity 
        /// and inconsistency. For this reason it is an internal method.
        /// To do this call the method 
        /// void CGraph::SetNodeLabelContext(object labelContructor=null)
        /// <see cref="CGraph.SetNodeLabelContext"/>
        /// </summary>
        /// <param name="labeler">The labeler.</param>
        internal void SetLabel(CGraphLabeling<CGraphNode> labeler) {
            M_Label = labeler.Label(this);
        }

        /// <summary>
        /// Sets the node label and updates the default labeller object for the graph
        /// The label is verified to be unique
        /// </summary>
        /// <param name="label"></param>
        public override void SetLabel(string label) {
            //1. Check the given label with the labeller object
            // Produce the given warnings if something goes wrong
            M_OwnerGraph.SetNodeLabel(this, label);

            //2. Set M_Label property
            M_Label = label;
        }


        /// <summary>
        /// Returns a <see cref="System.String" /> that represents the label of the node
        /// given by the specifier labeler object <see cref="CGraphLabeling{T}" />
        /// </summary>
        /// <param name="labeler">The labeler.</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public string ToString(object labeler) {
            return m_graph.GetNodeLabel(labeler, this);
        }
    }
    #endregion



    #region Graph Edge

    /// <summary>
    /// A graph edges interconnects to Nodes ports that in sequence belong to a node
    /// The CGraphEdge class stores a reference to the source and target ports. And
    /// can provide through properties the source and target ports and nodes. It also
    /// provides given the port or node at the one side of the edge the corresponding
    /// port or node at the side.
    /// </summary>
    [Serializable]
    public class CGraphEdge : CGraphPrimitive {
        #region MemberVariables

        /// <summary>
        /// The m_source node
        /// </summary>
        private CGraphNode m_sourceNode;

        /// <summary>
        /// The m_sink node
        /// </summary>
        private CGraphNode m_sinkNode;

        /// <summary>
        /// Type of edge can be directed or undirected
        /// </summary>
        private GraphType m_edgeType;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="CGraphEdge" /> class. It creates an edge that
        /// connects the two nodes gives as parameters.
        /// </summary>
        /// <param name="sourceNode">The source node.</param>
        /// <param name="sinkNode">The sink node.</param>
        /// <param name="ownerGraph">The owner graph.</param>
        /// <param name="edgeType">Type of the edge can be directed or undirected (optional default is DIRECTED).</param>
        /// <exception cref="NullReferenceException">At least one of the nodes is null</exception>
        public CGraphEdge(CGraphNode sourceNode, CGraphNode sinkNode, CGraph ownerGraph,
            GraphType edgeType = GraphType.GT_DIRECTED) : base(GraphElementType.ET_EDGE, ownerGraph) {

            if (sourceNode == null || sinkNode == null) {
                throw new NullReferenceException("At least one of the nodes is null");
            }
            // Source and Sink nodes
            m_sourceNode = sourceNode;
            m_sinkNode = sinkNode;

            // Type of edge ( directed or undirected )
            m_edgeType = edgeType;
        }

        public CGraphEdge() : base(GraphElementType.ET_EDGE, null) {

        }

        /// <summary>
        /// Sets the label for the current node using the name specified
        /// in the given labeller. After this call the node exposes the 
        /// given label from the M_Label property. This function is better
        /// to be called for all graph nodes in order to avoid complexity 
        /// and inconsistency. For this reason it is an internal method.
        /// To do this call the method 
        /// void CGraph::SetNodeLabelContext(object labelContructor=null)
        /// <see cref="CGraph.SetEdgeLabelContext"/>
        /// </summary>
        /// <param name="labeler">The labeler.</param>
        internal void SetLabel(CGraphLabeling<CGraphEdge> labeler) {
            M_Label = labeler.Label(this);
        }

        /// <summary>
        /// Sets the edge label and updates the default labeller object for the graph
        /// </summary>
        /// <param name="label"></param>
        public override void SetLabel(string label) {
            //1. Check the given label with the labeller object
            // Produce the given warnings if something goes wrong
            M_OwnerGraph.SetEdgeLabel(this, label);

            //2. Set M_Label property
            M_Label = label;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents the label of the node
        /// given by the specifier labeler object <see cref="CGraphLabeling{T}" />
        /// </summary>
        /// <param name="labeler">The labeler.</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public string ToString(object labeler) {
            return m_graph.GetEdgeLabel(labeler, this);
        }
        
        #region Graph Edge Properties

        /// <summary>
        /// Returns true if the edge is directed
        /// </summary>
        private bool M_IsDirected {
            get { return m_edgeType == GraphType.GT_DIRECTED; }
            set { m_edgeType = (value ? GraphType.GT_DIRECTED : GraphType.GT_UNDIRECTED); }
        }

        /// <summary>
        /// Represents the source node for directed graphs  or one of the edge's
        /// nodes of the undirected graph
        /// </summary>
        /// <value>
        /// Returns a reference to the source node
        /// </value>
        public CGraphNode M_Source {
            get { return m_sourceNode; }
            set { m_sourceNode = value; }
        }

        /// <summary>
        /// Represents the target node for directed graphs  or one of the edge's
        /// nodes of the undirected graph
        /// </summary>
        /// <value>
        /// Returns a reference to the target node
        /// </value>
        public CGraphNode M_Target {
            get { return m_sinkNode; }
            set { m_sinkNode = value; }
        }

        /// <summary>
        /// Represents the edge type. Can be directed or undirected
        /// </summary>
        /// <value>
        /// The type of the m_ edge.
        /// </value>
        public GraphType M_EdgeType {
            get { return m_edgeType; }
            set { m_edgeType = value; }
        }

        #endregion
    }

    #endregion


    #region Graph

    /// TODO : The graph requires a flag that indicates if the graph changed its structure

    /// <summary>
    /// The Graph holds the list of nodes and the list of edges as well as the type of
    /// graph ( directed / undirected ). 
    /// </summary>
    [Serializable]
    public class CGraph : CGraphPrimitive, ICloneable {


        /// <summary>
        /// This class represents the operation of merging two graphs. It also holds the node
        ///  and edge mapping between the initial individual graphs and the resulting graph
        /// </summary>
        public class CMergeGraphOperation {
            public enum MergeOptions { MO_DEFAULT = 0, MO_PRESERVELABELS = 1 }
            private CGraph m_resultGraph;
            private List<CGraph> m_initialGraphs;
            private Dictionary<CGraphNode, CGraphNode> m_nodeMapping;
            private Dictionary<CGraphEdge, CGraphEdge> m_edgeMapping;

            private Options<MergeOptions> m_mergeOptions;
            /// <summary>
            /// The MergeGraphOperation constructor can take optionally the resulting
            /// graph which can be any CGraph descendant object. In cases where the 
            /// resulting graph is a descendant of the CGraph class it is mandatory
            /// that the designer provide an object of the resulting graph otherwise
            /// the mergeGraph method will produce a CGraph-typed result. It is the
            /// designer's responsibility to provide an empty object if he wants
            /// </summary>
            /// <param name="resultGraph"></param>
            public CMergeGraphOperation(CGraph resultGraph = null, int? mergeOptions = null) {
                m_resultGraph = resultGraph;
                m_initialGraphs = new List<CGraph>();
                m_nodeMapping = new Dictionary<CGraphNode, CGraphNode>();
                m_edgeMapping = new Dictionary<CGraphEdge, CGraphEdge>();
                if (mergeOptions != null) {
                    m_mergeOptions = new Options<MergeOptions>(mergeOptions);
                }
            }

            /// <summary>
            /// Returns the node in the new graph that corresponds to
            /// the originalNode of the original graph
            /// </summary>
            /// <param name="oldNewNode"></param>
            /// <returns></returns>
            public CGraphNode GetMirrorNode(CGraphNode originalNode) {
                return m_nodeMapping[originalNode];
            }

            public CGraphEdge GetMirrorEdge(CGraphEdge originalEdge) {
                return m_edgeMapping[originalEdge];
            }

            /// <summary>
            /// Merges the existing one with the graph given as a parameter
            /// </summary>
            /// <param name="graph">The given graph</param>
            /// <returns></returns>
            public CGraph MergeGraph(CGraph graph) {
                CGraphNode newnode;
                // 1. Create graph
                if (m_resultGraph == null) {
                    m_resultGraph = CreateGraph();
                }

                // 2. Add the nodes of the given graph to the original graph
                CIt_GraphNodes it = new CIt_GraphNodes(graph);

                for (it.Begin(); !it.End(); it.Next()) {
                    if (m_mergeOptions.IsSet(MergeOptions.MO_PRESERVELABELS)) {
                        newnode = m_resultGraph.CreateGraphNode<CGraphNode>(it.M_CurrentItem.M_Label);
                    }
                    else {
                        newnode = m_resultGraph.CreateGraphNode<CGraphNode>();
                    }

                    // Correspondence between merged graph "newnode" and the given 
                    // graph node. Map[givengraph.node] -> merged.node
                    m_nodeMapping[it.M_CurrentItem] = newnode;
                }

                // 3. Add the corresponding edges of the given graph to the original
                CIt_GraphEdges itEdge = new CIt_GraphEdges(graph);

                for (itEdge.Begin(); !itEdge.End(); itEdge.Next()) {

                    CGraphEdge newEdge = m_resultGraph.AddGraphEdge<CGraphEdge, CGraphNode>(m_nodeMapping[itEdge.M_CurrentItem.M_Source],
                        m_nodeMapping[itEdge.M_CurrentItem.M_Target],
                        GraphType.GT_DIRECTED);

                    m_edgeMapping[itEdge.M_CurrentItem] = newEdge;
                }

                return m_resultGraph;
            }

            /// <summary>
            /// Transfers the information from the specified graph's element type information under the
            /// specified key to the resulting graph corresponding element type. The information is stored in the
            /// resulting graph as temporary operation hence the resulting graph itself is used as key
            /// </summary>
            /// <param name="graph">The original graph which was merged into the resulting graph</param>
            /// <param name="elementType">The graph element type where the information is stored</param>
            /// <param name="key">The under which the information becomes accessible in the merged and the old graph</param>
            public void MergeGraphInfo(CGraph graph, GraphElementType elementType, object key) {
                switch (elementType) {
                    case GraphElementType.ET_NODE:
                        foreach (CGraphNode graphNode in graph.m_graphNodes) {
                            m_nodeMapping[graphNode][key] = graphNode[key];   // deep copy????
                        }
                        break;
                    case GraphElementType.ET_EDGE:
                        foreach (CGraphEdge graphEdge in graph.m_graphEdges) {
                            m_edgeMapping[graphEdge][key] = graphEdge[key];  // deep copy;
                        }
                        break;
                        //case GraphElementType.ET_GRAPH:
                        //    m_resultGraph[key] = graph[key];        // deep copy;;;
                        // break;
                }
            }
        }

        /// <summary>
        /// This is a clone relation between the initial graph and its clone. This relation
        /// is valid until the initial graph is modified
        /// </summary>
        public class CCloneGraphOperation {
            private CGraph m_graphClone;
            private CGraph m_initialGraph = null;
            private Dictionary<CGraphNode, CGraphNode> m_nodeMapping=null;
            private Dictionary<CGraphEdge, CGraphEdge> m_edgeMapping=null;
            private bool m_structuralClone;

            /// <summary>
            /// The CloneGraphOperation constructor can take optionally the resulting
            /// graph which can be any CGraph descentant object. In cases where the 
            /// resulting graph is a descentand of the CGraph class it is mandatory
            /// that the designer provide an object of the resulting graph otherwise
            /// the cloneGraph method will produce a CGraph-typed result. It is the
            /// designer's responsibility to provide an empty object
            /// </summary>
            /// <param name="resultGraph"></param>
            public CCloneGraphOperation(bool structuralClone=false,CGraph resultGraph = null) {
                m_graphClone = resultGraph;
                m_structuralClone = structuralClone;
                if (!m_structuralClone) {
                    m_nodeMapping = new Dictionary<CGraphNode, CGraphNode>();
                    m_edgeMapping = new Dictionary<CGraphEdge, CGraphEdge>();
                }
            }

            /// <summary>
            /// This method clones the input graph and provides the node and edge mapping between the 
            /// initial and the clone graph. The node and edge mapping tables are allocated and created
            /// inside the method. Technically the method can applied to any graph even if it has cicles
            /// The edge predicates indicates which nodes to copy in the cloned graphs. If it is null the
            /// whole set of nodes and edges are copied from the initial graph to the cloned graph
            /// </summary>
            /// <param name="nodeMapping">Node mapping table </param>
            /// <param name="edgeMapping">Edge mapping table</param>
            /// <returns>The clone graph</returns>
            public CGraph CloneGraph(CGraph initialGraph,  List<CGraphNode> edgePredicates = null) {
                CIt_GraphNodes it1 = new CIt_GraphNodes(initialGraph);

                m_initialGraph = initialGraph;
                if (m_graphClone == null) {
                    m_graphClone = CreateGraph();
                }

                for (it1.Begin(); !it1.End(); it1.Next()) {
                    // Create the node and associate it with the node of the initial graph from which it came.
                    if (edgePredicates == null || (edgePredicates != null && edgePredicates.Contains(it1.M_CurrentItem))) {
                        if (!m_structuralClone) {
                            m_nodeMapping[it1.M_CurrentItem] =
                                m_graphClone.CreateGraphNode<CGraphNode>(it1.M_CurrentItem.M_Label);
                        }
                        else {
                            m_graphClone.m_graphNodes.Add(it1.M_CurrentItem);
                        }
                    }
                }


                CIt_GraphEdges it2 = new CIt_GraphEdges(m_initialGraph);
                for (it2.Begin(); !it2.End(); it2.Next()) {
                    // Drawn the edge between the two nodes if both nodes are included in the edgePredicates list
                    if (edgePredicates == null ||
                        (edgePredicates != null &&
                        edgePredicates.Contains(it2.M_CurrentItem.M_Source) &&
                        edgePredicates.Contains(it2.M_CurrentItem.M_Target))) {
                        if (!m_structuralClone) {
                            m_edgeMapping[it2.M_CurrentItem] = m_graphClone.AddGraphEdge<CGraphEdge, CGraphNode>(
                                m_nodeMapping[it2.M_CurrentItem.M_Source],
                                m_nodeMapping[it2.M_CurrentItem.M_Target],
                                GraphType.GT_DIRECTED);
                        }
                        else {
                            m_graphClone.m_graphEdges.Add(it2.M_CurrentItem);
                        }
                    }
                }
                return m_graphClone;
            }

            /// <summary>
            /// This method clones the specified information of the initial graph to the cloned graph
            /// The information is specified by the graph element type and the key
            /// </summary>
            /// <param name="elementType">The graph elements type of which the information is cloned</param>
            /// <param name="key">The key to the information data structure</param>
            public void CloneGraphInfo(GraphElementType elementType, object key) {

                switch (elementType) {
                    case GraphElementType.ET_NODE:
                        foreach (CGraphNode graphNode in m_initialGraph.m_graphNodes) {
                            m_nodeMapping[graphNode][key] = graphNode[key];   // deep copy???? deep copy requires clone constructor for the information object. Thus it is decided to create a shallow copy
                        }
                        break;
                    case GraphElementType.ET_EDGE:
                        foreach (CGraphEdge graphEdge in m_initialGraph.m_graphEdges) {
                            m_edgeMapping[graphEdge][key] = graphEdge[key];  // deep copy;
                        }
                        break;
                    case GraphElementType.ET_GRAPH:
                        m_graphClone[key] = m_initialGraph[key];        // deep copy;;;
                        break;
                }
            }

        }

        /// <summary>
        /// List of graph nodes
        /// </summary>
        protected List<CGraphNode> m_graphNodes;
        /// <summary>
        /// List of graph's native edges. All the edges of a graph
        /// are considered as native. 
        /// </summary>
        protected List<CGraphEdge> m_graphEdges;

        /// <summary>
        /// The GraphType enumeration members describes the graph in terms of
        /// the types of edges ( directed, undirected, mixed )
        /// </summary>
        protected GraphType m_graphType;

        /// <summary>
        /// This variable holds the node labels from different contexts. A context can
        /// be something abstract i.e an algorithm, a printer etc. What type of object
        /// is used as a key depends on the designer. The context can be the graph
        /// itself indicating that the graph is labelled by on its own in which case
        /// the key for accessing the node labels is the "this" pointer
        /// </summary>
        [NonSerialized]
        private Dictionary<object, CGraphLabeling<CGraphNode>> m_nodeLabels;

        /// <summary>
        /// This variable holds the edge labels from different contexts. A context can
        /// be something abstract i.e an algorithm, a printer etc. What type of object
        /// is used as a key depends on the designer. The context can be the graph
        /// itself indicating that the graph is labelled by on its own in which case
        /// the key for accessing the node labels is the "this" pointer
        /// </summary>
        [NonSerialized]
        private Dictionary<object, CGraphLabeling<CGraphEdge>> m_edgeLabels;

        /// <summary>
        /// This variable holds the graph's labels from different contexts. A context can
        /// be something abstract i.e an algorithm, a printer etc. What type of object
        /// is used as a key depends on the designer. The context can be the graph
        /// itself indicating that the graph is labelled by on its own in which case
        /// the key for accessing the node labels is the "this" pointer
        /// </summary>
        private Dictionary<object, string> m_graphLabels;

        private static int ms_serialNumberCounter = 0;

        private int m_graphSerialNumber;

        private int m_storageReservationKeyCounter = 0;

        //TODO: Support Node/Edge Label copying between contructors

        /// <summary>
        /// Contains a list of graph printers that initiate by the call to Generate method
        /// </summary>
        [NonSerialized]
        protected List<CGraphPrinter> m_graphPrinters = null;

        /// <summary>
        /// Holds the type of info under a given key for nodes, edges and the graph.
        /// </summary>
        internal Dictionary<object, Type> m_nkeyToInfoTypeRecord = new Dictionary<object, Type>();
        internal Dictionary<object, Type> m_ekeyToInfoTypeRecord = new Dictionary<object, Type>();
        internal Dictionary<object, Type> m_gkeyToInfoTypeRecord = new Dictionary<object, Type>();



        /// <summary>
        /// Initializes a new instance of the <see cref="CGraph"/> class.
        /// </summary>
        protected CGraph() : base(GraphElementType.ET_GRAPH) {
            m_graphNodes = new List<CGraphNode>();
            m_graphEdges = new List<CGraphEdge>();
            m_graphPrinters = new List<CGraphPrinter>();
            m_nodeLabels = new Dictionary<object, CGraphLabeling<CGraphNode>>();
            m_edgeLabels = new Dictionary<object, CGraphLabeling<CGraphEdge>>();
            m_graphLabels = new Dictionary<object, string>();
            m_graphType = GraphType.GT_DIRECTED;
            m_graph = this;

            // Create native node labeller
            SetGraphNodeLabelling(this, new CGraphLabeling<CGraphNode>(this));

            // Create native edge labeller
            SetGraphEdgeLabelling(this, new CGraphLabeling<CGraphEdge>(this));

            m_graphSerialNumber = ms_serialNumberCounter++;

            // Create native graph label
            SetLabel("G" + m_graphSerialNumber.ToString());
        }

        protected CGraph(CGraph graph) : this() {

        }

        public void SerializeInBinary(string filename) {
            BinaryFormatter saver = new BinaryFormatter();

            using (Stream stream = new FileStream(filename, FileMode.Create, FileAccess.Write)) {
                saver.Serialize(stream, this);
            }
        }

        public CGraph DeserializeInBinary(string filename) {
            BinaryFormatter saver = new BinaryFormatter();

            using (Stream stream = new FileStream(filename, FileMode.Open, FileAccess.Read)) {
                return (CGraph)saver.Deserialize(stream);
            }
        }

        [OnDeserializing()]
        internal void OnDeserializingMethod(StreamingContext context) {
            m_graphPrinters = new List<CGraphPrinter>();
            m_nodeLabels = new Dictionary<object, CGraphLabeling<CGraphNode>>();
            m_edgeLabels = new Dictionary<object, CGraphLabeling<CGraphEdge>>();
        }

        /// <summary>
        /// Creates a new graph. The CGraph constructor is protected and hence
        /// this method acts as a constructor. ownergraph parameter is null
        /// only this function creates the top level graph. If the ownergraph
        /// is not null the graph is labelled with the default ie. its serialnumber
        /// </summary> 
        /// <returns>The new graph</returns>
        public static CGraph CreateGraph() {
            CGraph newGraph = new CGraph();
            return newGraph;
        }

        /// <summary>
        /// This method forms a clone of the given graph by the following way:
        /// 1) It keeps the nodes and edges of the given graph 
        /// 2) It keeps the info of the given graph
        /// 3) Creates a new instance of graph for the new graph
        /// This method is suitable in cases where we want to have an instance of
        /// a graph that is changing over time since it uses the same nodes, edges
        /// and info of the original graph while it uses a new graph object to
        /// represent the structure of the graph.
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        public static CGraph TemporalClone(CGraph graph) {

            CGraph clone;
            CCloneGraphOperation cloneop = new CCloneGraphOperation(true);
            clone = cloneop.CloneGraph(graph);
           foreach (object key in graph.m_gkeyToInfoTypeRecord.Keys) {
                cloneop.CloneGraphInfo(GraphElementType.ET_GRAPH, key);
            }
            return clone;
        }


        /// <summary>
        /// This method clone the graph itself and performs a shallow copy
        /// of its info
        /// </summary>
        /// <returns></returns>
        public object Clone() {
            CGraph clone;
            CCloneGraphOperation cloneop = new CCloneGraphOperation();
            clone = cloneop.CloneGraph(this);
            foreach (object key in m_nkeyToInfoTypeRecord.Keys) {
                cloneop.CloneGraphInfo(GraphElementType.ET_NODE, key);
            }
            foreach (object key in m_ekeyToInfoTypeRecord.Keys) {
                cloneop.CloneGraphInfo(GraphElementType.ET_EDGE, key);
            }
            foreach (object key in m_gkeyToInfoTypeRecord.Keys) {
                cloneop.CloneGraphInfo(GraphElementType.ET_GRAPH, key);
            }
            return clone;
        }

        /// <summary>
        /// Returns true if the node belongs to the graph
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public bool IsANodeOfGraph(CGraphNode node) {
            bool yes = false;
            foreach (CGraphNode graphNode in m_graphNodes) {
                if (node == graphNode) {
                    return true;
                }
            }
            return yes;
        }

        /// <summary>
        /// Realizes a merge operation of the current graph with another graph provided by
        /// method parameter and provides the result via a CMergeGraphOperation object 
        /// which contains the mapping information for edges and nodes. The operation can
        /// be applied multiple times for merging multiple graphs to the existing one
        /// for nodes and edges. Also the designer should also call MergeGraphInfo on the
        /// returned CMergeGraphOperation object to copy any information he wants
        /// </summary>
        /// <param name="g">The graph to be merged with the existing</param>
        /// <returns></returns>
        public CMergeGraphOperation Merge(CGraph g, CMergeGraphOperation.MergeOptions options) {
            CMergeGraphOperation op = new CMergeGraphOperation(this, (int?)options);

            // Merge nodes and edges
            op.MergeGraph(g);

            return op;
        }

        /// <summary>
        /// Creates the graph from CSV file. The file contains comma separated objects
        /// in brackets ( {} ) of the graph ie. nodes and edges. The object contents are
        /// described in a comma separated list of values. For example
        /// 1) Nodes are described as { Nodename  }
        /// 2) Edges are described ad { Nodename1, Nodename2  }
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns>The new graph</returns>
        public static CGraph CreateGraphFromCSV(string filename, CGraph ownerGraph = null) {
            CGraph newGraph = new CGraph();
            // Read CSV file and create

            // Open a text file
            using (StreamReader csv = File.OpenText(filename)) {
                CGraphNode sourceNode, sinkNode;


                // Read the input file into a string
                string csvData = csv.ReadToEnd();

                // Identify graph objects which are described as
                // curly bracketed comma separated list of values
                Match m = Regex.Match(csvData, @"\{(\ *(\d+|\w)\ *,)+?\ *(\d+|\w)\ *\}");
                while (m.Success) {
                    MatchCollection mCol = Regex.Matches(m.Value, @"\d+|\w");
                    if (mCol.Count == 1) {
                        // This is a graph node
                        newGraph.CreateGraphNode<CGraphNode>(mCol[0].Value);
                    }
                    else if (mCol.Count == 2) {
                        // This is an edge node
                        // Create the nodes if not already exist
                        sourceNode = newGraph.Node(mCol[0].Value, newGraph);
                        if (sourceNode == null) {
                            sourceNode = newGraph.CreateGraphNode<CGraphNode>(mCol[0].Value);
                        }
                        sinkNode = newGraph.Node(mCol[1].Value, newGraph);
                        if (sinkNode == null) {
                            sinkNode = newGraph.CreateGraphNode<CGraphNode>(mCol[1].Value);
                        }

                        // Add the edge
                        newGraph.AddGraphEdge<CGraphEdge, CGraphNode>(sourceNode, sinkNode, null);
                    }
                    else {
                        // Syntax error in graph description file
                        Console.WriteLine("Syntax Error in CSV file");
                    }
                    m = m.NextMatch();
                }
            }
            return newGraph;
        }

        /// <summary>
        /// Creates and adds a node to the graph. Gives the default label
        /// </summary>
        /// <returns>Returns a reference to the newnode</returns>
        public virtual N CreateGraphNode<N>() where N : CGraphNode, new() {
            N newnode = new N() { M_OwnerGraph = this };
            m_graphNodes.Add(newnode);
            newnode.SetLabel(GenerateNodeLabel(newnode));
            return newnode;
        }
        /// <summary>
        /// prefixes the label of a given element with a given prefix
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="element"></param>
        public void PrefixElementLabel(string prefix, CGraphPrimitive element) {
            element.SetLabel(prefix + element.M_Label);
        }
        /// <summary>
        /// Prefixes the given element type elements with the given prefix
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="elementType">type of elements to prefix</param>
        public void PrefixGraphElementLabels(string prefix, GraphElementType elementType) {
            switch (elementType) {
                case GraphElementType.ET_EDGE:
                    CIt_GraphEdges ite = new CIt_GraphEdges(this);
                    for (ite.Begin(); !ite.End(); ite.Next()) {
                        PrefixElementLabel(prefix, ite.M_CurrentItem);
                    }
                    break;
                case GraphElementType.ET_NODE:
                    CIt_GraphNodes itn = new CIt_GraphNodes(this);
                    for (itn.Begin(); !itn.End(); itn.Next()) {
                        PrefixElementLabel(prefix, itn.M_CurrentItem);
                    }
                    break;
                case GraphElementType.ET_GRAPH:
                    PrefixElementLabel(prefix, this);
                    break;
            }
        }
        /// <summary>
        /// postfixes the label of a given element with a given postfix
        /// </summary>
        /// <param name="postfix"></param>
        /// <param name="element"></param>
        public void PostfixElementLabel(string postfix, CGraphPrimitive element) {
            element.SetLabel(element.M_Label + postfix);
        }

        /// <summary>
        /// Generates a label for a Node. 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        internal string GenerateNodeLabel(CGraphNode node) {
            if (m_graphNodes.Contains(node)) {
                return "G" + M_SerialNumber.ToString() + "_" + "N" + ms_UniqueSerialNumberCounter++;
            }
            else {
                throw new Exception("Node does not exist");
            }
        }

        /// <summary>
        /// Creates the graph node and gives it the specified label. The
        /// method  investigates for dublicate labels. 
        /// </summary>
        /// <param name="label">The label.</param>
        /// <returns>The new node</returns>
        public virtual N CreateGraphNode<N>(string label) where N : CGraphNode, new() {
            N newnode = new N() { M_OwnerGraph = this };
            m_graphNodes.Add(newnode);
            newnode.SetLabel(label);
            return newnode;
        }

        /// <summary>
        /// Returns a node label given for a specified contructor. There is no setter for this
        /// method. The setter must be implemented in the labelContructor which is is responsible
        /// for the labels
        /// ( algorithm ,printer etc)
        /// </summary>
        /// <param name="labelContructor">The contructor.</param>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public string GetNodeLabel(object labelContructor, CGraphNode node) {
            if (m_nodeLabels.ContainsKey(labelContructor)) {
                return m_nodeLabels[labelContructor].Label(node);
            }
            return null;
        }

        /// <summary>
        /// Returns an edge label given for a specified contructor ( algorithm ,printer etc).
        /// There is no setter for this method. The setter must be implemented in the
        /// labelContructor which is is responsible for the labels
        /// </summary>
        /// <param name="labelContructor">The contructor.</param>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public string GetEdgeLabel(object labelContructor, CGraphEdge edge) {
            if (m_nodeLabels.ContainsKey(labelContructor)) {
                return m_edgeLabels[labelContructor].Label(edge);
            }
            return null;
        }

        /// <summary>
        /// This method returns the graph nodes that don't have predecessors.
        /// The list parameter must be initialized by the caller
        /// </summary>
        /// <param name="rootsOut">Output list of root nodes. It must be allocated
        /// from the caller</param>
        /// <returns></returns>
        public int GetRootNodes(List<CGraphNode> rootsOut) {
            int nRoots = 0;
            foreach (CGraphNode node in m_graphNodes) {
                if (node.M_NumberOfPredecessors == 0) {
                    nRoots++;
                    rootsOut.Add(node);
                }
            }
            return nRoots;
        }




        /// <summary>
        /// Gets the label of the specified node using the host graph as label contructor.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public string GetNodeLabel(CGraphNode node) {
            return m_nodeLabels[this].Label(node);
        }

        /// <summary>
        /// Gets the label of the specified node using the host graph as label contructor.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public string GetEdgeLabel(CGraphEdge edge) {
            return m_edgeLabels[this].Label(edge);
        }
        #region Labelling Methods
        /// <summary>
        /// Sets the label of the specified edge for using the host graph as label contructor.
        /// </summary>
        /// <param name="edge">The edge.</param>
        /// <param name="label">The label.</param>
        public void SetEdgeLabel(CGraphEdge edge, string label) {
            m_edgeLabels[this].SetLabel(edge, label);
            edge.SetLabel(m_edgeLabels[this]);
        }

        internal string GenerateEdgeLabel(CGraphEdge edge) {
            if (m_graphEdges.Contains(edge)) {
                return "G" + M_SerialNumber.ToString() + "_" + "E" + m_graphEdges.IndexOf(edge);
            }
            else {
                throw new Exception("Node does not exist");
            }
        }

        /// <summary>
        /// Sets the label context for all edges of the graph
        /// </summary>
        /// <param name="labelContructor">The label contructor.</param>
        public void SetEdgeLabelContext(object labelContructor) {
            CGraphLabeling<CGraphEdge> labeller = labelContructor as CGraphLabeling<CGraphEdge>;
            if (labeller != null && labeller.M_Graph == this) {
                // labelContructor is a CGraphLabelling object for the current graph
                foreach (CGraphEdge edge in m_graphEdges) {
                    edge.SetLabel(labeller);
                }
            }
            else if (labelContructor != null && m_nodeLabels.ContainsKey(labelContructor)) {
                // labelContructor is a user defined object. Check SetGraphNodeLabelling method
                // about how to insert a new label contructor
                foreach (CGraphEdge edge in m_graphEdges) {
                    edge.SetLabel(m_edgeLabels[labelContructor]);
                }
            }
            else {
                // default node labelling
                foreach (CGraphEdge edge in m_graphEdges) {
                    edge.SetLabel(m_edgeLabels[this]);
                }
            }
        }

        /// <summary>
        /// Adds the graph edge labelling.
        /// </summary>
        /// <param name="labelsContructor">The labels contructor.</param>
        /// <param name="edgelabeling">The edgelabeling.</param>
        public void SetGraphEdgeLabelling(object labelsContructor, CGraphLabeling<CGraphEdge> edgelabeling) {
            if (edgelabeling.M_Graph == this) {
                // Check if labelling refers to the current graph
                m_edgeLabels[labelsContructor] = edgelabeling;
            }
            else {
                throw new Exception("Labeller is not for this graph");
            }
        }

        /// <summary>
        /// Adds the graph node labelling Contructor. If there is already a labelling object 
        /// for the given contructor it is overwrited
        /// </summary>
        /// <param name="labelsHandler">The labels handler.</param>
        /// <param name="nodelabeling">The nodelabeling.</param>
        public void SetGraphNodeLabelling(object labelsContructor, CGraphLabeling<CGraphNode> nodelabeling) {
            if (nodelabeling.M_Graph == this) {
                // Check if labelling refers to the current graph
                m_nodeLabels[labelsContructor] = nodelabeling;
            }
            else {
                throw new Exception("Labeller is not for this graph");
            }
        }

        /// <summary>
        /// Sets the label of the specified node using the host graph as label contructor.
        /// This is called privetly by the CGraphNode object
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="label">The label.</param>
        internal void SetNodeLabel(CGraphNode node, string label) {
            m_nodeLabels[this].SetLabel(node, label);
            node.SetLabel(m_nodeLabels[this]);
        }

        /// <summary>
        /// Sets the label context for all nodes of the graph. The context can be
        /// any of the three
        /// 1. A labelling object of class CGraphLabelling<CGraphNode> which directly
        /// names the nodes. If an algorithm incorporates multiple naming schemes this
        /// is the valid source for labels
        /// 2. A labbeling  object of class CGraphLabelling<CGraphNode> is indireclty 
        /// provided by a labelling provider. This can be for example an algorithm or
        /// other user defined type
        /// 3. The default labelling of graph
        /// </summary>
        /// <param name="labelContructor">The label contructor.</param>
        public void SetNodeLabelContext(object labelContructor = null) {
            CGraphLabeling<CGraphNode> labeller = labelContructor as CGraphLabeling<CGraphNode>;
            if (labeller != null && labeller.M_Graph == this) {
                // labelContructor is a CGraphLabelling object for the current graph
                foreach (CGraphNode node in m_graphNodes) {
                    node.SetLabel(labeller);
                }
            }
            else if (labelContructor != null && m_nodeLabels.ContainsKey(labelContructor)) {
                // This case gives the key for the labeling provider thus labelContractor is the 
                // key to access the CGraphLabeling<CGraphNode> node 
                // labelContructor is a user defined object which is used as a key. 
                // Check SetGraphNodeLabelling method about how to insert a new label contructor
                foreach (CGraphNode node in m_graphNodes) {
                    node.SetLabel(m_nodeLabels[labelContructor]);
                }
            }
            else {
                // default node labelling
                foreach (CGraphNode node in m_graphNodes) {
                    node.SetLabel(m_nodeLabels[this]);
                }
            }
        }

        public override void SetLabel(string label) {
            //1. Set M_Label property
            M_Label = label;

            //2. Update label info
            m_graphLabels[this] = label;
        }
        #endregion

        /// <summary>
        /// Adds a native edge to the graph and label it with the default ie its serialnumber.
        /// </summary>
        /// <param name="source">A reference to the source node</param>
        /// <param name="target">A reference to the target node</param>
        /// <param name="edgetype">The edgetype.</param>
        /// <returns> A reference to the new edge</returns>
        /// <exception cref="System.NullReferenceException">At least one of the input nodes has a null reference</exception>
        public virtual E AddGraphEdge<E, N>(N source, N target,
            GraphType edgetype = GraphType.GT_DIRECTED) where E : CGraphEdge, new()
                                                       where N : CGraphNode {
            E newGraphEdge;

            if (source == null || target == null) {
                throw new NullReferenceException("At least one of the input nodes has a null reference");
            }

            if (edgetype != GraphType.GT_DIRECTED && edgetype != GraphType.GT_UNDIRECTED) {
                throw new Exception("Wrong edge type");
            }

            if (source.M_OwnerGraph != target.M_OwnerGraph) {
                throw new Exception("Wrong edge type");
            }

            // Add edge
            newGraphEdge = new E() {
                M_Source = source,
                M_Target = target,
                M_EdgeType = edgetype,
                M_OwnerGraph = this
            };

            // Update the nodes
            source.AddOutgoingGraphEdge(newGraphEdge);
            target.AddIngoingGraphEdge(newGraphEdge);

            // Add edge to the graph
            m_graphEdges.Add(newGraphEdge);

            // Update graph type based on the new edge type
            if (edgetype == GraphType.GT_DIRECTED) {
                if (m_graphType == GraphType.GT_DIRECTED ||
                    m_graphType == GraphType.GT_NA) {
                    m_graphType = GraphType.GT_DIRECTED;
                }
                else if (m_graphType == GraphType.GT_UNDIRECTED ||
                      m_graphType == GraphType.GT_MIXED) {
                    m_graphType = GraphType.GT_MIXED;
                }
            }
            else if (edgetype == GraphType.GT_UNDIRECTED) {
                if (m_graphType == GraphType.GT_DIRECTED ||
                    m_graphType == GraphType.GT_MIXED) {
                    m_graphType = GraphType.GT_MIXED;
                }
                else if (m_graphType == GraphType.GT_UNDIRECTED ||
                      m_graphType == GraphType.GT_NA) {
                    m_graphType = GraphType.GT_UNDIRECTED;
                }
            }

            // Set the default label
            newGraphEdge.SetLabel(GenerateEdgeLabel(newGraphEdge));

            return newGraphEdge;
        }

        /// <summary>
        /// Adds a native edge to the graph with the given label.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="label">The label.</param>
        /// <param name="edgetype">The edgetype.</param>
        /// <returns></returns>
        public virtual E AddGraphEdge<E, N>(N source, N target, string label = null,
            GraphType edgetype = GraphType.GT_DIRECTED) where E : CGraphEdge, new()
                                                        where N : CGraphNode {
            E newEdge = AddGraphEdge<E, N>(source, target, edgetype);
            if (label != null) {
                newEdge.SetLabel(label);
            }
            return newEdge;
        }


        /// TODO: Remove the label as well
        /// <summary>
        /// Removes a node from the graph.
        /// </summary>
        /// <param name="node">The node to be removed</param>
        public void RemoveNode(CGraphNode node) {

            // Apart from removing a node, it is necessary to also remove
            // the edges with which is connected. So...
            // first remove the native edges
            foreach (CGraphEdge e in m_graphEdges) {
                if (e.M_Target == node || e.M_Source == node) {
                    RemoveNativeGraphEdge(e);
                }
            }
            foreach (var nodeLabelsKey in m_nodeLabels.Keys) {
                m_nodeLabels[nodeLabelsKey].RemoveElement(node);
            }
            // remove node from graph
            m_graphNodes.Remove(node);

        }

        /// TODO: Remove edge label as well
        /// <summary>
        /// Deletes a native graph edge.
        /// </summary>
        /// <param name="edge">The edge.</param>
        public void RemoveNativeGraphEdge(CGraphEdge edge) {
            int dircnt = 0, undircnt = 0;

            // Delete the edge from list
            m_graphEdges.Remove(edge);

            // Update the graph type
            foreach (CGraphEdge e in m_graphEdges) {
                if (e.M_EdgeType == GraphType.GT_DIRECTED) {
                    dircnt++;
                }
                else if (e.M_EdgeType == GraphType.GT_UNDIRECTED) {
                    undircnt++;
                }
            }

            if (dircnt > 0 && undircnt == 0) {
                m_graphType = GraphType.GT_DIRECTED;
            }
            else if (dircnt == 0 && undircnt > 0) {
                m_graphType = GraphType.GT_UNDIRECTED;
            }
            else if (dircnt > 0 && undircnt > 0) {
                m_graphType = GraphType.GT_MIXED;
            }
            else {
                m_graphType = GraphType.GT_NA;
            }

            // Update the nodes
            edge.M_Source.RemoveOutgoingGraphEdge(edge);
            edge.M_Target.RemoveIngoingGraphEdge(edge);

        }

        /// <summary>
        /// Returns a node at the specified index or raises an exception when a
        /// non existing node is indexed
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>A reference to the indexed node</returns>
        public CGraphNode Node(int index) {
            if (index < m_graphNodes.Count) {
                return m_graphNodes[index];
            }
            else {
                throw new IndexOutOfRangeException();
            }
        }

        /// <summary>
        /// Determines whether the graph contains the specified element
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>true if the node belongs to the graph</returns>
        public GraphElementType Contains(CGraphPrimitive element) {
            if (element.M_ElementType == GraphElementType.ET_NODE) {
                foreach (CGraphNode n in m_graphNodes) {
                    if (ReferenceEquals(element, n)) {
                        return GraphElementType.ET_NODE;
                    }
                }
            }
            else if (element.M_ElementType == GraphElementType.ET_EDGE) {
                foreach (CGraphEdge e in m_graphEdges) {
                    if (ReferenceEquals(element, e)) {
                        return GraphElementType.ET_EDGE;
                    }
                }
            }
            return GraphElementType.ET_NA;
        }

        /// <summary>
        /// Returns the node with the given label from the given label context.
        /// Different contexts may assign different labels to the nodes. By
        /// default a node is labelled only by its serialnumber which is unique.
        /// In the default case call public CGraphNode Node(int index) method
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="labelContextKey">The object that gives label to the node</param>
        /// <returns></returns>
        public CGraphNode Node(string label, object labelContextKey) {
            // Get the labeller for the given context
            CGraphLabeling<CGraphNode> labels = m_nodeLabels[labelContextKey];

            // Search for the given label in the labeller context
            foreach (CGraphNode node in m_graphNodes) {
                if (labels.Label(node) == label) {
                    return node;
                }
            }
            return null;
        }

        /// <summary>
        /// Returns the edge with the given label from the given label context.
        /// Different contexts may assign different labels to the edges. By
        /// default a edge is labelled only by its serialnumber which is unique.
        /// In the default case call public CGraphEdge Edge(int source, int target)
        /// method
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="labelContextKey">The label context key.</param>
        /// <returns></returns>
        public CGraphEdge Edge(string label, object labelContextKey) {
            CGraphLabeling<CGraphEdge> labels = m_edgeLabels[labelContextKey];
            foreach (CGraphEdge edge in m_graphEdges) {
                if (labels.Label(edge) == label) {
                    return edge;
                }
            }
            return null;
        }

        /// <summary>
        /// Returns an edge at the specified index or raises an exception otherwise
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>A reference to the indexed edge</returns>
        internal CGraphEdge Edge(int index) {
            if (index < m_graphEdges.Count) {
                return m_graphEdges[index];
            }
            else {
                throw new IndexOutOfRangeException();
            }
        }

        /// <summary>
        /// Returns the NATIVE edge between the <see cref="source"/> and the
        /// <see cref="target"/> nodes. It returns the first edge it finds
        /// with the specified source and sink. If parallel edges are supported
        /// use the function Edges that returns a list of edges having the same
        /// source and target nodes. The function returns null if it doesn't find
        /// the edge.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public CGraphEdge Edge(CGraphNode source, CGraphNode target) {
            foreach (CGraphEdge g in m_graphEdges) {
                if (ReferenceEquals(g.M_Source, source) &&
                    ReferenceEquals(g.M_Target, target)) {
                    return g;
                }
            }
            return null;
        }

        /// <summary>
        /// Creates a random graph of the given type and number of nodes and edges.
        /// The method creates <see cref="NumVertices" /> and creates <see cref="NumEdges" />
        /// random edges between them.
        /// This function is made according to the algorithm
        /// Program 17.12 Random graph generator (random edges) in the book
        /// Graph Algorithms Sedgewick 2002
        /// Notice that the final number of edges inserted to the graph is smaller or equal to
        /// the requested number because some edges might be excluded due to the two switches
        /// loops and paralleledges.
        /// Assumes that the graph is empty
        /// </summary>
        /// <param name="NumVertices">The number vertices.</param>
        /// <param name="NumEdges">The number edges.</param>
        /// <param name="graphtype">The graph type can be directed (default) or undirected</param>
        /// <param name="loops">if set to <c>true</c> loop edges are allowed to be generated. Default is false</param>
        /// <param name="paralleledges">if set to <c>true</c> parallel edges are allowed to be generated. Default is false</param>
        public void GenerateGraph_RandomEdges(int NumVertices, int NumEdges,
            GraphType graphtype = GraphType.GT_DIRECTED, bool loops = false, bool paralleledges = false) {
            int i;
            CGraphNode newnode;

            // For more information for the random class look in
            // https://msdn.microsoft.com/en-us/library/system.random(v=vs.110).aspx
            Random r = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);

            // Create vertices
            for (i = 0; i < NumVertices; i++) {
                newnode = CreateGraphNode<CGraphNode>();
            }
            // Create edges
            for (i = 0; i < NumEdges; i++) {
                CGraphNode v, w;
                v = Node(r.Next(0, NumVertices));
                w = Node(r.Next(0, NumVertices));
                // Add always the edge except in case where some of the following conditions are true
                // For more info look at ..\MyWork\MyNotes\Programming_Logic_Conditions_vxx.docx
                if (// CONDITION 1: true when v and w are diffrent nodes. loops is a switch
                     (!ReferenceEquals(v, w) || loops) &&
                     // CONDITION 2: true when v->w edge doesn't exist. paralleledges is a switch
                     (ReferenceEquals(Edge(v, w), null) || paralleledges) &&
                     // CONDITION 3: true when w->v edge doesn't exist. paralleledges and m_graphType == GraphType.GT_DIRECTED is a switch
                     (ReferenceEquals(Edge(w, v), null) || paralleledges || m_graphType == GraphType.GT_DIRECTED)
                   ) {
                    AddGraphEdge<CGraphEdge, CGraphNode>(v, w, graphtype);
                }
            }
        }

        /// <summary>
        /// Creates a random graph with the given number of vertices.
        /// The algorithm considers every possible pair of nodes and draws an
        /// edge between them based on some probability that can be defined
        /// by the designer. The main characteristic of this algorithm is that
        /// all pairs of nodes are considered however not all possible edges are
        /// drawn. This gives the possibility to draw graphs that the edges exhibit
        /// some properties ( i.e locality : connect mostly nearby nodes etc )
        /// In a directed graph Parallel edges cannot exist because its pair is
        /// considered once
        /// Assumes that the graph is empty
        /// </summary>
        /// <param name="NumVertices">The number vertices.</param>
        /// <param name="NumEdges">The number edges.</param>
        /// <param name="graphtype">The graphtype.</param>
        /// <param name="loops">if set to <c>true</c> loops are allowed.</param>
        /// <param name="paralleledges">if set to <c>true</c> parallel edges are allowed (use only for undirected).</param>
        public void GenerateGraph_RandomGraph(int NumVertices, int NumEdges,
            GraphType graphtype = GraphType.GT_DIRECTED, bool loops = false, bool paralleledges = false) {
            int i, j;
            double p;
            CGraphNode v, w;

            // Creates a Random object and feeds it with the current time to ensure
            // diffrent behaviour in each execution
            // For more information for the random class look in
            // https://msdn.microsoft.com/en-us/library/system.random(v=vs.110).aspx
            Random r = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            // Create the requested number of vertices
            for (i = 0; i < NumVertices; i++) {
                CreateGraphNode<CGraphNode>();
            }
            // Probability function
            p = (2.0 * NumEdges) / (NumVertices * (NumVertices - 1));

            // Insert edges
            for (i = 0; i < NumVertices; i++) {
                for (j = 0; j <= i; j++) {
                    v = Node(i);
                    w = Node(j);
                    if (
                        // CONDITION 1: If the possibility allows the edge
                        (r.NextDouble() < p) &&
                        // CONDITION 2: If it isn't a self edge
                        (i != j || loops) &&
                        // CONDITION 3: In case of undirected graph check if a parallel edge exists
                        (ReferenceEquals(Edge(w, v), null) || paralleledges || m_graphType == GraphType.GT_UNDIRECTED)
                        ) {
                        AddGraphEdge<CGraphEdge, CGraphNode>(v, w, graphtype);
                    }
                }
            }
        }

        /// <summary>
        /// Prints the graph into a StringBuilder object.
        /// Optionally the header and footer of the .dot file can be ommited for use of the
        /// graph edges in the multi layer graph printer.
        /// </summary>
        /// <param name="onlyedges">if set to <c>true</c> [onlyedges].</param>
        /// <returns></returns>
        internal StringBuilder Print(CGraphPrinter graphPrinter) {
            if (graphPrinter != null) {
                return graphPrinter.Print();
            }
            return null;
        }

        /// <summary>
        /// Calls the tool that renders the graph to some representable media
        /// </summary>
        /// <param name="filepath">The filepath.</param>
        /// <param name="executeGenerator">if set to <c>true</c> [execute generator].</param>
        public void Generate(string filepath, bool executeGenerator = true) {
            foreach (CGraphPrinter prnt in m_graphPrinters) {
                prnt.Generate(filepath, executeGenerator);
            }
        }

        /// <summary>
        /// Registers a printer to the graph. The printer is added if it doesn't already exists
        /// and if it is not null
        /// </summary>
        /// <param name="printer">The printer object</param>
        public void RegisterGraphPrinter(CGraphPrinter printer) {
            if (printer != null && !m_graphPrinters.Contains(printer)) {
                m_graphPrinters.Add(printer);
            }
        }

        /// <summary>
        /// Returns the node labeller used by the graph
        /// </summary>
        /// <returns></returns>
        public CGraphLabeling<CGraphNode> GetNativeNodeLabelling() {
            return m_nodeLabels[this];
        }

        /// <summary>
        /// Returns the node labeller used by the graph
        /// </summary>
        /// <returns></returns>
        public CGraphLabeling<CGraphEdge> GetNativeEdgeLabelling() {
            return m_edgeLabels[this];
        }

        public int GetStorageReservationKey() {
            return m_storageReservationKeyCounter++;
        }

        /// <summary>
        /// Represents the number of graph nodes
        /// </summary>
        /// <value>
        /// Returns the number of graph nodes.
        /// </value>
        public int M_NumberOfNodes {
            get { return m_graphNodes.Count; }
        }

        /// <summary>
        /// Represents the number of graph edges
        /// </summary>
        /// <value>
        /// Returns the number of graph edges.
        /// </value>
        public int M_NumberOfEdges {
            get { return m_graphEdges.Count; }
        }

        /// <summary>
        /// Gets the type of graph.
        /// </summary>
        /// <value>
        /// Returns one of the values of GraphType enumeration based
        /// on the type of edges that the graph has.
        /// </value>
        public GraphType M_GraphType {
            get { return m_graphType; }
        }

        public int M_SerialNumber { get { return m_graphSerialNumber; } }

    }
    #endregion

}
