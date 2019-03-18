

namespace GraphLibrary.Generics {

    /// <summary>
    /// This class is for programmers convinience. It instanciates iterators
    /// that iterate over a graph's elements required for the algorithms and
    /// other graph processing facilities
    /// </summary>
    /// <typeparam name="TNode">The type of the node.</typeparam>
    /// <typeparam name="TEdge">The type of the edge.</typeparam>
    /// <typeparam name="TGraph">The type of the graph.</typeparam>
    public abstract class AbstractGraphIteratorFactory <TNode,TEdge,TGraph> {

        /// <summary>
        /// A reference to the graph 
        /// </summary>
        protected TGraph m_Graph;

        public AbstractGraphIteratorFactory(TGraph graph) {
            m_Graph = graph;
        }

        public abstract CIt_Successors CreateSuccessorsIterator(TNode node);
        public abstract CIt_Predecessors CreatePredecessorsIterator(TNode node);  
        public abstract CIt_GraphEdges CreateGraphEdgesIterator();
        public abstract CIt_GraphNodes CreateGraphNodesIterator(); 
        public abstract CIt_GraphRootNodes CreateGraphRootNodesIterator(); 
        public abstract CIt_GraphLeafNodes CreateGraphLeafNodesIterator(); 
        //public abstract CIt_GraphDFS CreateGraphDfsIterator();
        //public abstract CIt_GraphBFS CreateGraphBfsIterator(TNode startNode);
    }

}