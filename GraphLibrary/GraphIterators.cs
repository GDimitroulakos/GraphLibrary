using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphLibrary.Generics;


// TODO 1: Verify DFS BFS iterators

namespace GraphLibrary {

    #region Outgoing Edges Iterator
    /// <summary>
    /// Iterates over the native successor nodes of a node
    /// </summary>
    public class CIt_NodeOutgoingEdges : AbstractGraphIterator<CGraphEdge> {
        /// <summary>
        /// Node to which the iterator refers
        /// </summary>
        CGraphNode m_node;

        /// <summary>
        /// The m_outgoing edges
        /// </summary>
        private List<CGraphEdge> m_outgoingEdges;

        /// <summary>
        /// Iterator
        /// </summary>
        int m_it;

        /// <summary>
        /// Constructor. Takes the node to which iterator refers
        /// </summary>
        /// <param name="node">Node to which the iterator is applied</param>
        public CIt_NodeOutgoingEdges(CGraphNode node) {
            m_node = node;
            m_outgoingEdges = node.OutgoingEdges;
        }


        /// <summary>
        /// Points to the first valid item or to null if there is no one
        /// </summary>
        /// <returns></returns>
        public override CGraphEdge Begin() {
            m_it = 0;
            m_iterations = 0;
            m_currentItem = m_outgoingEdges[m_it++];
            return m_currentItem;
        }

        /// <summary>
        /// End() function runs after Begin() and Next() functions and
        /// before we enter into the loop body. It validates that the
        /// iterator points to a proper item
        /// </summary>
        /// <returns></returns>
        public override bool End() {
            if (m_currentItem == null) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Points to the next valid item or to null if there isn't one
        /// </summary>
        /// <returns>Returns the current item</returns>
        public override CGraphEdge Next() {
            m_iterations++;
            return m_currentItem = m_outgoingEdges[m_it++];
        }
    }
    #endregion


    #region Successors Iterator
    /// <summary>
    /// Iterates over the native successor nodes of a node
    /// </summary>
    public class CIt_Successors : AbstractGraphIterator<CGraphNode> {
        /// <summary>
        /// Node to which the iterator refers
        /// </summary>
        CGraphNode m_node;

        /// <summary>
        /// Iterator
        /// </summary>
        int m_it;

        /// <summary>
        /// Constructor. Takes the node to which iterator refers
        /// </summary>
        /// <param name="node">Node to which the iterator is applied</param>
        public CIt_Successors(CGraphNode node) {
            m_node = node;
        }


        /// <summary>
        /// Points to the first valid item or to null if there is no one
        /// </summary>
        /// <returns></returns>
        public override CGraphNode Begin() {
            m_it = 0;
            m_iterations = 0;
            m_currentItem = m_node.Successor(m_it++);
            return m_currentItem;
        }

        /// <summary>
        /// End() function runs after Begin() and Next() functions and
        /// before we enter into the loop body. It validates that the
        /// iterator points to a proper item
        /// </summary>
        /// <returns></returns>
        public override bool End() {
            if (m_currentItem == null) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Points to the next valid item or to null if there isn't one
        /// </summary>
        /// <returns>Returns the current item</returns>
        public override CGraphNode Next() {
            m_iterations++;
            return m_currentItem = m_node.Successor(m_it++);
        }
    }
    #endregion

    #region Predecessors Iterator
    /// <summary>
    /// Iterates over the predecessor nodes of a node
    /// </summary>
    public class CIt_Predecessors : AbstractGraphIterator<CGraphNode> {
        /// <summary>
        /// Node to which the iterator refers
        /// </summary>
        CGraphNode m_node;

        /// <summary>
        /// Iterator
        /// </summary>
        int m_it;

        /// <summary>
        /// Constructor. Takes the node to which it refers
        /// </summary>
        /// <param name="node">Node to which the iterator is applied</param>
        public CIt_Predecessors(CGraphNode node) {
            m_node = node;
        }

        /// <summary>
        /// Points to the first valid item if to null if there is no one
        /// </summary>
        /// <returns></returns>
        public override CGraphNode Begin() {
            m_it = 0;
            m_iterations = 0;
            m_currentItem = m_node.Predeccessor(m_it++);
            return m_currentItem;
        }

        /// <summary>
        /// End() function runs after Begin() and Next() functions and
        /// before we enter into the loop body. It validates that the
        /// iterator points to a proper item
        /// </summary>
        /// <returns></returns>
        public override bool End() {
            if (m_currentItem == null) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Points to the next valid item or to null if there isn't one
        /// </summary>
        /// <returns></returns>
        public override CGraphNode Next() {
            m_iterations++;
            return m_currentItem = m_node.Predeccessor(m_it++);
        }
    }
    #endregion

    #region Graph Edges Iterator
    /// <summary>
    /// Iterates over the graph edges
    /// </summary>
    public class CIt_GraphEdges : AbstractGraphIterator<CGraphEdge> {

        /// <summary>
        /// Graph over which we iteratting
        /// </summary>
        CGraph m_graph;

        /// <summary>
        /// Current iterator index
        /// </summary>
        int m_it;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="graph"></param>
        public CIt_GraphEdges(CGraph graph) {
            m_graph = graph;
        }

        /// <summary>
        /// Points to the first valid item if to null if there is no one
        /// </summary>
        /// <returns></returns>
        public override CGraphEdge Begin() {
            m_it = 0;
            m_iterations = 0;
            if (m_graph.M_NumberOfEdges > 0) {
                m_currentItem = m_graph.Edge(m_it);
            }
            else {
                m_currentItem = null;
            }
            return m_currentItem;
        }

        /// <summary>
        /// End() function runs after Begin() and Next() functions and
        /// before we enter into the loop body. It validates that the
        /// iterator points to a proper item
        /// </summary>
        /// <returns></returns>
        public override bool End() {
            if (m_currentItem == null) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Points to the next valid item or to null if there isn't one
        /// </summary>
        /// <returns>Returns the current item</returns
        public override CGraphEdge Next() {
            m_it++;
            m_iterations++;
            if (m_it < m_graph.M_NumberOfEdges) {
                m_currentItem = m_graph.Edge(m_it);
            }
            else {
                m_currentItem = null;
            }
            return m_currentItem;
        }
    }

    #endregion

    #region Graph Nodes Iterator
    /// <summary>
    /// Iterates over the graph nodes
    /// </summary>
    public class CIt_GraphNodes : AbstractGraphIterator<CGraphNode> {

        /// <summary>
        /// Graph over which we iterating
        /// </summary>
        CGraph m_graph;

        /// <summary>
        /// The initial graph given for iteration
        /// </summary>
        private CGraph m_initialGraph;

        /// <summary>
        /// Current iterator index
        /// </summary>
        int m_it;

        private bool m_lastItem;

        // Becomes true when current item is the last item
        public bool M_LastItem => m_lastItem;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="graph">The given graph</param>
        /// <param name="freeze">This parameter determines if the iteration will happen on the given graph or a clone of it. In case the graph is changing during the iteration and the objective is to iterate on the initial graph then set freeze to true</param>
        public CIt_GraphNodes(CGraph graph, bool freeze=false) {
            if (!freeze) {
                m_graph = graph;
            }
            else {
                // Iteration happens on the clone which can't be changed externally
                m_graph = CGraph.CloneGraph(graph);
                // The graph that is given from externally and can be change
                // during iteration
                m_initialGraph = graph;
            }
        }

        /// <summary>
        /// Points to the first valid item if to null if there is no one
        /// </summary>
        /// <returns></returns>
        public override CGraphNode Begin() {
            m_it = 0;
            m_iterations = 0;
            m_lastItem = false;
            if (m_graph.M_NumberOfNodes > 0) {
                m_currentItem = m_graph.Node(m_it);
                if (m_graph.M_NumberOfNodes == 1) {
                    m_lastItem = true;
                }
            }
            else {
                m_currentItem = null;
            }

            return m_currentItem;
        }

        /// <summary>
        /// End() function runs after Begin() and Next() functions and
        /// before we enter into the loop body. It validates that the
        /// iterator points to a proper item
        /// </summary>
        /// <returns></returns>
        public override bool End() {
            if (m_currentItem == null) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Points to the next valid item or to null if there isn't one
        /// </summary>
        /// <returns>Returns the current item</returns
        public override CGraphNode Next() {
            m_it++;
            m_iterations++;
            if (m_it < m_graph.M_NumberOfNodes) {
                m_currentItem = m_graph.Node(m_it);
                if (m_it == m_graph.M_NumberOfNodes - 1) {
                    m_lastItem = true;
                }
            }
            else {
                m_currentItem = null;
            }
            return m_currentItem;
        }
    }
    #endregion

    #region Graph Root Nodes Iterator
    /// <summary>
    /// Iterates over the graph's nodes not having predecessors
    /// </summary>
    public class CIt_GraphRootNodes : AbstractGraphIterator<CGraphNode> {

        /// <summary>
        /// Reference to the graph
        /// </summary>
        private CGraph m_graph;

        /// <summary>
        /// iterator
        /// </summary>
        int m_it = 0;

        public CIt_GraphRootNodes(CGraph mGraph) {
            m_graph = mGraph;
        }

        /// <summary>
        /// Executes once at the start of the iteration and before the End() method
        /// Initializes the iterator variable ( realized in the subclasses ) and the
        /// m_currentItem to point to the first element of the item set. If the set is
        /// empty the m_currentItem points to null. After the Begin() method the
        /// End() method executes before going into the loop body. Thus the loop
        /// terminates if the item set is empty before ever running the loop body
        /// The transition logic for finding the first item in the items set is
        /// implemented in the subclasses
        /// </summary>
        /// <returns></returns>
        public override CGraphNode Begin() {
            m_it = 0;
            m_iterations = 0;
            m_currentItem = null;
            while (m_it < m_graph.M_NumberOfNodes &&                      // while not search all the nodes AND...
                   m_graph.Node(m_it).M_NumberOfPredecessors != 0) {   // while not discover nodes without predecessors
                m_it++;
            }
            if (m_it < m_graph.M_NumberOfNodes) {
                m_currentItem = m_graph.Node(m_it);
                return m_currentItem;
            }
            return m_currentItem;
        }

        /// <summary>
        /// Executes after the Begin() or Next() methods and before the Loop Body
        /// Returns true if the m_currentItem is null. This case arises in the following
        /// scenarios:
        /// 1) The item set is empty thus, after the Begin() method call the End() identifies
        /// that m_currentItem points to null
        /// 2) The item set is not empty and the iterator went outside the boundaries of the
        /// item set after the call to Next() method (Next() method set m_currentItem to null)
        /// </summary>
        /// <returns></returns>
        public override bool End() {
            if (m_currentItem == null) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Executes after the execution of the loop body and before the End() method
        /// Increases the iteration variable and set the m_currentItem to point to the current
        /// item of the item set. In case where the end of the item set is reached the m_currentItem
        /// points to null which is identified by the End() methods that is executed next.
        /// The transition logic from item to item is implemented from the subclasses.
        /// </summary>
        /// <returns></returns>
        public override CGraphNode Next() {
            m_it++;
            m_iterations++;
            while (m_it < m_graph.M_NumberOfNodes &&                       // while not search all the nodes AND...
                   m_graph.Node(m_it).M_NumberOfPredecessors != 0) {        // while not discover nodes without predecessors
                m_it++;
            }
            if (m_it < m_graph.M_NumberOfNodes) {
                m_currentItem = m_graph.Node(m_it);
                return m_currentItem;
            }
            else {
                m_currentItem = null;
            }
            return m_currentItem;
        }
    }
    #endregion

    #region Graph Leaf Nodes Iterator
    /// <summary>
    /// Iterates over the graph's nodes not having successors
    /// </summary>
    public class CIt_GraphLeafNodes : AbstractGraphIterator<CGraphNode> {

        /// <summary>
        /// Reference to the graph
        /// </summary>
        private CGraph m_graph;

        /// <summary>
        /// iterator
        /// </summary>
        int m_it = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="CIt_GraphLeafNodes"/> class.
        /// </summary>
        /// <param name="mGraph">The m graph.</param>
        public CIt_GraphLeafNodes(CGraph mGraph) {
            m_graph = mGraph;
        }

        /// <summary>
        /// Executes once at the start of the iteration and before the End() method
        /// Initializes the iterator variable ( realized in the subclasses ) and the
        /// m_currentItem to point to the first element of the item set. If the set is
        /// empty the m_currentItem points to null. After the Begin() method the
        /// End() method executes before going into the loop body. Thus the loop
        /// terminates if the item set is empty before ever running the loop body
        /// The transition logic for finding the first item in the items set is
        /// implemented in the subclasses
        /// </summary>
        /// <returns></returns>
        public override CGraphNode Begin() {
            m_it = 0;
            m_iterations = 0;
            m_currentItem = null;
            while (m_it < m_graph.M_NumberOfNodes &&                      // while not search all the nodes AND...
                   m_graph.Node(m_it).M_NumberOfSuccessors != 0) {         // while not discover nodes without successors
                m_it++;
            }
            if (m_it < m_graph.M_NumberOfNodes) {
                m_currentItem = m_graph.Node(m_it);
                return m_currentItem;
            }
            return m_currentItem;
        }

        /// <summary>
        /// Executes after the Begin() or Next() methods and before the Loop Body
        /// Returns true if the m_currentItem is null. This case arises in the following
        /// scenarios:
        /// 1) The item set is empty thus, after the Begin() method call the End() identifies
        /// that m_currentItem points to null
        /// 2) The item set is not empty and the iterator went outside the boundaries of the
        /// item set after the call to Next() method (Next() method set m_currentItem to null)
        /// </summary>
        /// <returns></returns>
        public override bool End() {
            if (m_currentItem == null) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Executes after the execution of the loop body and before the End() method
        /// Increases the iteration variable and set the m_currentItem to point to the current
        /// item of the item set. In case where the end of the item set is reached the m_currentItem
        /// points to null which is identified by the End() methods that is executed next.
        /// The transition logic from item to item is implemented from the subclasses.
        /// </summary>
        /// <returns></returns>
        public override CGraphNode Next() {
            m_it++;
            m_iterations++;
            while (m_it < m_graph.M_NumberOfNodes &&                       // while not search all the nodes AND...
                   m_graph.Node(m_it).M_NumberOfSuccessors != 0) {        // while not discover nodes without successors
                m_it++;
            }
            if (m_it < m_graph.M_NumberOfNodes) {
                m_currentItem = m_graph.Node(m_it);
                return m_currentItem;
            }
            return m_currentItem;
        }
    }
    #endregion

}