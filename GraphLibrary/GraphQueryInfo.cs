using GraphLibrary.Generics;



namespace GraphLibrary {

    /// <summary>
    /// Query info from the CGraph class for a specific graph and key 
    /// </summary>
    /// <seealso cref="CGraph" />
    public class CGraphQueryInfo : AbstractGraphQueryInfo<CGraphNode,CGraphEdge> {

        /// <summary>
        /// The original graph where the algorithm is applied
        /// </summary>
        protected CGraph m_graph;

        /// <summary>
        /// Key to access the information
        /// </summary>
        private object m_infoKey;
        

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractGraphQueryInfo"/> class.
        /// </summary>
        /// <param name="graph">The graph.</param>
        public CGraphQueryInfo(CGraph graph, object key) {
            m_graph = graph;
            m_infoKey = key;

            

        }

        /// <summary>
        /// Returns information concerning a node of the source graph
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="key">The key that will extract the information from the specified node's dictionary. If
        /// null is given then the current Query Info object is used as a key</param>
        /// <returns></returns>
        public override object Info(CGraphNode node) {
            return node[m_infoKey];
        }
        public override object TempInfo(CGraphNode node){
            return node[node];
        }


        /// <summary>
        /// Returns the information of the specified edge
        /// </summary>
        /// <param name="edge">The edge.</param>
        /// <param name="key">The key that will extract the information from the specified node's dictionary. If
        /// null is given then the current Query Info object is used as a key</param>
        /// <returns></returns>
        public override object Info(CGraphEdge edge){
            return edge[m_infoKey];
        }
        public override object TempInfo(CGraphEdge edge){
            return edge[edge];
        }

        /// <summary>
        /// Returns the information of the edge between the given source
        /// and target nodes of the source graph.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="key">The key that will extract the information from the specified node's dictionary.If
        /// null is given then the current Query Info object is used as a key</param>
        /// <returns></returns>
        public override object Info(CGraphNode source, CGraphNode target){
            return m_graph.Edge(source, target)[m_infoKey];
        }
        public override object TempInfo(CGraphNode source, CGraphNode target) {
            CGraphEdge edge = m_graph.Edge(source, target);
            return edge[edge];
        }
        /// <summary>
        /// Returns information from the source graph under the specified key
        /// </summary>
        /// <param name="key">The key object</param>
        /// <returns>The information object</returns>
        public override object Info() {
            return m_graph[m_infoKey];
        }
        public override object TempInfo() {
            return m_graph[m_graph];
        }
        /// <summary>
        
        /// <summary>
        /// Stores the information at the specified node or graph. If information already
        /// exists, it is overwritten
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="info">The information.</param>
        /// <param name="key">The key that will extract the information from the specified node's dictionary. If
        /// null is given then the current Query Info object is used as a key. Thus the QueryInfo object can
        /// be used as an information creator/exploitator</param>
        public override void CreateInfo(CGraphNode node, object info ){
            node[m_infoKey] = info;
        }
        public override void CreateTempInfo(CGraphNode node, object info ){
            node[node] = info;
        }

        /// <summary>
        /// Stores the information at the specified edge. If information already
        /// exists, it is overwritten
        /// </summary>
        /// <param name="edge">The edge.</param>
        /// <param name="info">The information object.</param>
        /// <param name="key">The key that will extract the information from the specified node's dictionary. If
        /// null is given then the current Query Info object is used as a key</param>
        public override void CreateInfo(CGraphEdge edge, object info ){
            edge[m_infoKey] = info;
        }
        public override void CreateTempInfo(CGraphEdge edge, object info ){
            edge[edge] = info;
        }

        /// <summary>
        /// Stores the information at the specified edge of the source graph. If information already
        /// exists, it is overwritten
        /// </summary>
        /// <param name="source">The source node.</param>
        /// <param name="target">The target node.</param>
        /// <param name="info">The information.</param>
        /// <param name="key">The key that will extract the information from the specified node's dictionary. If
        /// null is given then the current Query Info object is used as a key</param>
        public override void CreateInfo(CGraphNode source, CGraphNode target, object info ){
           m_graph.Edge(source, target)[m_infoKey] = info;
        }
        public override void CreateTempInfo(CGraphNode source, CGraphNode target, object info ){
            CGraphEdge edge = m_graph.Edge(source, target);
            edge[edge] = info;
        }

        /// <summary>
        /// Stores information in the source graph under the specified key
        /// </summary>
        /// <param name="info">The information object</param>
        /// <param name="key">The key object</param>
        public override void CreateInfo(object info) {
            m_graph[m_infoKey] = info;
        }
        public override void CreateTempInfo(object info) {
            m_graph[m_graph] = info;
        }

        /// <summary>
        /// Sets the source graph reference. It is mandatory
        /// before source graph queries begin
        /// </summary>
        public CGraph M_Graph {
            get { return m_graph; }
        }

        public object M_InfoKey {
            get { return m_infoKey; }
        }
    }

}