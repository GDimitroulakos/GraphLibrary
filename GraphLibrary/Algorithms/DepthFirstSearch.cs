using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary.Algorithms {


    public class DepthFirstSearchQueryInfo : CGraphQueryInfo {
        public DepthFirstSearchQueryInfo(CGraph graph, object key) : base(graph, key) {
        }
        
        
        
        public NodeColor Color(CGraphNode node) {
            return CastNodeInfo<DepthFirstSearchNodeInfo>(node).MColor;
        }
        public void SetColor(CGraphNode node,NodeColor color) {
            CastNodeInfo<DepthFirstSearchNodeInfo>(node).MColor = color ;
        }
        public int Arrival(CGraphNode node) {
            return CastNodeInfo<DepthFirstSearchNodeInfo>(node).MArrival;
        }
        public void SetArrival(CGraphNode node, int arrival) {
            CastNodeInfo<DepthFirstSearchNodeInfo>(node).MArrival = arrival;
        }
        public int Departure(CGraphNode node) {
            return CastNodeInfo<DepthFirstSearchNodeInfo>(node).MDeparture;
        }
        public void SetDeparture(CGraphNode node, int departure) {
            CastNodeInfo<DepthFirstSearchNodeInfo>(node).MDeparture = departure;
        }
    }
    
    public class DepthFirstSearch :CGraphAlgorithm<int> {
        private CGraph m_graph;
        private DepthFirstSearchQueryInfo m_outputDepthFirstSearch;
        private int m_time;

        public DepthFirstSearch(CGraph mGraph) {
            m_graph = mGraph;
            m_outputDepthFirstSearch = new DepthFirstSearchQueryInfo(mGraph,this);
        }

        public override void Init() {
            CIt_GraphNodes it = new CIt_GraphNodes(m_graph);
            for (it.Begin(); !it.End(); it.Next()) {
                m_outputDepthFirstSearch.CreateInfo(it.M_CurrentItem,new DepthFirstSearchNodeInfo() {
                    MColor = NodeColor.NC_WHITE,
                    MDeparture = -1,
                    MArrival = -1
                }); 
            }
            m_time = 0;
            for (it.Begin(); !it.End(); it.Next()) {
                if (Color(it.M_CurrentItem) == NodeColor.NC_WHITE) {
                    Visit(it.M_CurrentItem);
                }
            }
        }

        public override int Run() {
            Init();
#if DEBUG
            Debug();
#endif
            return 0;
        }

        public void Debug() {
            DepthFirstSearchQueryInfo info = new DepthFirstSearchQueryInfo(m_graph,this);
            CIt_GraphNodes it  = new CIt_GraphNodes(m_graph);
            for (it.Begin(); !it.End(); it.Next()) {
                Console.WriteLine("Node {0}: arrival ({1}) - departure ({2})",
                    it.M_CurrentItem.M_Label,info.Arrival(it.M_CurrentItem),info.Departure(it.M_CurrentItem));
            }
        }

        public override int Visit(CGraphNode node) {
            m_time++;
            SetArrival(node,m_time);
            SetColor(node,NodeColor.NC_GRAY);
            CIt_Successors si = new CIt_Successors(node);
            for (si.Begin(); !si.End(); si.Next()) {
                if (Color(si.M_CurrentItem) == NodeColor.NC_WHITE) {
                    Visit(si.M_CurrentItem);
                }
            }
            SetColor(node,NodeColor.NC_BLACK);
            m_time++;
            SetDeparture(node,m_time);
            return 0;
        }

        NodeColor Color(CGraphNode node) {
            return m_outputDepthFirstSearch.Color(node);
        }
        void SetColor(CGraphNode node, NodeColor color) {
            m_outputDepthFirstSearch.SetColor(node,color);
        }
        int Arrival(CGraphNode node) {
            return m_outputDepthFirstSearch.Arrival(node);
        }
        void SetArrival(CGraphNode node, int arrival) {
            m_outputDepthFirstSearch.SetArrival(node, arrival);
        }
        int Departure(CGraphNode node) {
            return m_outputDepthFirstSearch.Departure(node);
        }
        void SetDeparture(CGraphNode node, int departure) {
            m_outputDepthFirstSearch.SetDeparture(node, departure);
        }

    }

    public class DepthFirstSearchNodeInfo {
        private NodeColor m_Color;
        private int m_arrival;
        private int m_departure;

        public NodeColor MColor {
            get => m_Color;
            set => m_Color = value;
        }

        public int MArrival {
            get => m_arrival;
            set => m_arrival = value;
        }

        public int MDeparture {
            get => m_departure;
            set => m_departure = value;
        }
    }

}
