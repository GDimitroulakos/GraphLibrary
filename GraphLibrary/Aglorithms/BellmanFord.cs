using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary.Algorithms {
    public class BellmanFord :CGraphAlgorithm<int> {
        private CGraphQueryInfo m_graphPathInfo;
        private CGraphQueryInfo m_graphWeightInfo;
        private CGraph mGraph;
        private int m_weightsKey;
        private CGraphNode m_source;
        public const int  m_PATHINFO =0;
        private Dictionary<CGraphNode,  List<CGraphNode> > m_shortestPaths = 
            new Dictionary<CGraphNode, List<CGraphNode>>();
           
        
        public BellmanFord(CGraph graph,CGraphNode source, int graphWeightsKey) {
            mGraph = graph;
            m_source = source;
            this[m_PATHINFO] = m_graphPathInfo =  new CGraphQueryInfo(graph,this);
            m_graphWeightInfo = new CGraphQueryInfo(graph,graphWeightsKey);
        }

        public override void Init() {

            CIt_GraphNodes itn = new CIt_GraphNodes(mGraph);
            
            for (itn.Begin(); !itn.End(); itn.Next()) {
                if (itn.M_CurrentItem != m_source) {
                    m_graphPathInfo.CreateInfo(itn.M_CurrentItem,
                        new PathInfo() {MDistance = null, MPredecessor = null});
                }
                else {
                    m_graphPathInfo.CreateInfo(itn.M_CurrentItem,
                        new PathInfo() { MDistance = 0, MPredecessor = null});
                }
            }
        }

        public void Run() {
            Init();

            CIt_GraphEdges ite = new CIt_GraphEdges(mGraph);
            for (int i = 0; i < mGraph.M_NumberOfNodes - 1; i++) {
#if DEBUG
                Console.WriteLine("Iteration : {0}",i);
#endif
                for (ite.Begin(); !ite.End(); ite.Next()) {
                    Relax(ite.M_CurrentItem.M_Source,ite.M_CurrentItem.M_Target);
                }
            }

            for (ite.Begin(); !ite.End(); ite.Next()) {
                if (Distance(ite.M_CurrentItem.M_Target) > Distance(ite.M_CurrentItem.M_Source) +
                    Weight(ite.M_CurrentItem.M_Source, ite.M_CurrentItem.M_Target)) {
                    throw new Exception("The graph contains negative cycles");
                }
            }
            GeneratePaths();
#if DEBUG
            Debug();
#endif
        }

        private void Debug() {
            CIt_GraphNodes it = new CIt_GraphNodes(mGraph);

            for (it.Begin(); !it.End(); it.Next()) {
                Console.WriteLine("{0}.distance={1}", it.M_CurrentItem.M_Label, Distance(it.M_CurrentItem));
                if (Predecessor(it.M_CurrentItem) != null) {
                    Console.WriteLine("{0}.predecessor={1}", it.M_CurrentItem.M_Label,
                        Predecessor(it.M_CurrentItem).M_Label);
                }
            }

            Console.WriteLine("Shortest Paths {0}->*",m_source.M_Label);

            for (it.Begin(); !it.End(); it.Next()) {
                
                foreach (var cGraphNode in m_shortestPaths[it.M_CurrentItem]) {
                    Console.Write("{0},",cGraphNode.M_Label);
                }
                Console.WriteLine();
            }
        }

        private void GeneratePaths() {
            CIt_GraphNodes it = new CIt_GraphNodes(mGraph);
            CGraphNode m_node;

            for (it.Begin(); !it.End(); it.Next()) {
                m_shortestPaths[it.M_CurrentItem]=new List<CGraphNode>();
                m_node = it.M_CurrentItem;
                while (m_node!=null){
                    m_shortestPaths[it.M_CurrentItem].Insert(0, m_node);
                    m_node = Predecessor(m_node);
                }
            }
        }

        protected void Relax(CGraphNode source, CGraphNode target) {
            int currentDistance ;
            
            if ( Distance(source)!=null ) {
                currentDistance = (int) Distance(source) + Weight(source, target);

                if (Distance(target) != null) {
                    if (Distance(target) > currentDistance) {
                        SetDistance(target, currentDistance);
                        SetPredecessor(target, source);
                    }
                }
                else {
                    SetDistance(target, currentDistance);
                    SetPredecessor(target, source);
                }
            }
            else {
#if DEBUG
                currentDistance = Weight(source, target);
#endif
            }

#if DEBUG
            Console.WriteLine("Relax: {0} -->  {1}  CurrentDistance={2}, Distance={3}, Weight={4}", 
                source.M_Label, target.M_Label,currentDistance, Distance(target),Weight(source, target));
#endif
        }

        protected int? Distance(CGraphNode node) {
            return ((PathInfo) (m_graphPathInfo.Info(node))).MDistance;
        }

        protected void SetDistance(CGraphNode node, int distance) {
            ((PathInfo) (m_graphPathInfo.Info(node))).MDistance = distance;
        }
        protected void SetPredecessor(CGraphNode node, CGraphNode predecessor) {
            ((PathInfo)(m_graphPathInfo.Info(node))).MPredecessor = predecessor;
        }

        protected CGraphNode Predecessor(CGraphNode node) {
            return ((PathInfo) (m_graphPathInfo.Info(node))).MPredecessor;
        }
        
        protected int Weight(CGraphNode source, CGraphNode target) {
            return (int) (m_graphWeightInfo.Info(source, target));
        }



    }



    public class PathInfo {
        private int? m_distance;
        private CGraphNode m_predecessor;

        public PathInfo() {
        }

        public int? MDistance {
            get => m_distance;
            set => m_distance = value;
        }

        public CGraphNode MPredecessor {
            get => m_predecessor;
            set => m_predecessor = value;
        }
    }
    
}
