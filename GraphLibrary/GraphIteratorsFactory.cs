
using GraphLibrary.Generics;

namespace GraphLibrary {

    public class CGraphIteratorsFactory : AbstractGraphIteratorFactory<CGraphNode, CGraphEdge, CGraph> {

        public CGraphIteratorsFactory(CGraph graph) : base(graph) {
        }

        public override CIt_Successors CreateSuccessorsIterator(CGraphNode node){
            return new CIt_Successors(node);
        }

        public override CIt_Predecessors CreatePredecessorsIterator(CGraphNode node) {
            return new CIt_Predecessors(node);
        }

        public override CIt_GraphEdges CreateGraphEdgesIterator(){
            return new CIt_GraphEdges(m_Graph);
        }

        public override CIt_GraphNodes CreateGraphNodesIterator(){
            return  new CIt_GraphNodes(m_Graph);
        }

        public override CIt_GraphRootNodes CreateGraphRootNodesIterator(){
            return new CIt_GraphRootNodes(m_Graph);
        }

        public override CIt_GraphLeafNodes CreateGraphLeafNodesIterator(){
            return new CIt_GraphLeafNodes(m_Graph);
        }
    }

}