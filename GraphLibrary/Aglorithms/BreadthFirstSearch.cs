using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary {

    public enum NodeColor {
        NC_WHITE, NC_BLACK, NC_GRAY
    }
    /// <summary>
    /// Breadth First Search algorithm works for both directed and undirected
    /// graphs. Requires a node from which the algorithm does the traversal.
    /// ALGORITHM DATA:
    /// BFS derived data are acquired using the algorithm object as a key
    /// 
    /// </summary>
    public class BreadthFirstSearch : CGraphAlgorithm<int> {
        // Declares the vertex from which the algorithm start
        private CGraphNode m_source;
        private CGraph m_graph;
        private CGraphQueryInfo m_BFSData;
        private Queue<CGraphNode> m_Q;
        private List<CGraphNode> m_nodeVisitList;

        public BreadthFirstSearch(CGraphNode mSource, CGraph mGraph) {
            m_source = mSource;
            m_graph = mGraph;
            m_BFSData = new CGraphQueryInfo(mGraph,this);
            m_Q = new Queue<CGraphNode>();
            m_nodeVisitList = new List<CGraphNode>();
        }

        public override void Init() {
            CIt_GraphNodes it  = new CIt_GraphNodes(m_graph);
            m_BFSData.CreateInfo(m_nodeVisitList);
            for (it.Begin(); !it.End(); it.Next()) {
                if (it.M_CurrentItem != m_source) {
                    m_BFSData.CreateInfo(it.M_CurrentItem,
                        new BFSNodeInfo() {MDistance = -1, MColor = NodeColor.NC_WHITE});
                }
                else {
                    m_BFSData.CreateInfo(it.M_CurrentItem,
                        new BFSNodeInfo() { MDistance = 0, MColor = NodeColor.NC_GRAY });
                }
            }
            m_Q.Enqueue(m_source);
        }

        public override int Run() {
            CGraphNode u;
            Init();

            while (m_Q.Count != 0) {
                u = m_Q.Dequeue();
                m_nodeVisitList.Add(u);
                CIt_Predecessors adj = new CIt_Predecessors(u);
                for (adj.Begin(); !adj.End(); adj.Next()) {
                    if (Color(adj.M_CurrentItem) == NodeColor.NC_WHITE) {
                        SetColor(adj.M_CurrentItem,NodeColor.NC_GRAY);
                        SetDistance(adj.M_CurrentItem,Distance(u)+1);
                        m_Q.Enqueue(adj.M_CurrentItem);
                    }
                }
                SetColor(u,NodeColor.NC_BLACK);
            }
#if DEBUG
            Debug();
#endif
            return 0;
        }

        public void Debug() {

            Console.WriteLine("Printing BFS Results with Source Node : {0}",m_source.M_Label);
            foreach (CGraphNode node in m_nodeVisitList) {
                Console.WriteLine("Node {0} distance: {1}",node.M_Label,Distance(node));
            }
        }

        // Algorithm accessors

        public NodeColor Color(CGraphNode node) {
            return ((BFSNodeInfo)m_BFSData.Info(node)).MColor;
        }
        public void SetColor(CGraphNode node,NodeColor color) {
            ((BFSNodeInfo)m_BFSData.Info(node)).MColor = color;
        }

        public int Distance(CGraphNode node) {
            return ((BFSNodeInfo)m_BFSData.Info(node)).MDistance;
        }
        public void SetDistance(CGraphNode node, int distance) {
            ((BFSNodeInfo)m_BFSData.Info(node)).MDistance = distance;
        }
        public List<CGraphNode> BFSNodes() {
            return ((List<CGraphNode>)m_BFSData.Info());
        }
        
    }

    public class BFSNodeInfo {
        private NodeColor m_color;
        private int m_distance;

        public NodeColor MColor {
            get => m_color;
            set => m_color = value;
        }

        public int MDistance {
            get => m_distance;
            set => m_distance = value;
        }
    }
   

}
