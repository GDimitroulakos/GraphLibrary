using System;
using GraphLibrary.Generics;



namespace GraphLibrary {

    /// <summary>
    /// Query info from the CGraph class for a specific graph and key 
    /// </summary>
    /// <seealso cref="CGraph" />
    [Serializable]
    public class CGraphQueryInfo<IN,IE,IG> : AbstractGraphQueryInfo<CGraphNode, CGraphEdge,IN,IE,IG> {

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
        public override IN Info(CGraphNode node) {
            if (node.M_OwnerGraph == m_graph) {
                return (IN) node[m_infoKey];
            }
            throw new Exception("The given node does not belong to the graph");
        }
        public override IN TempInfo(CGraphNode node){
            if (node.M_OwnerGraph == m_graph) {
                return (IN) node[node];
            }
            throw new Exception("The given node does not belong to the graph");
        }
        
        /// <summary>
        /// Returns the information of the specified edge. If the information
        /// is not there it returns null instead of dropping an exception
        /// </summary>
        /// <param name="edge">The edge.</param>
        /// <param name="key">The key that will extract the information from the specified node's dictionary. If
        /// null is given then the current Query Info object is used as a key</param>
        /// <returns></returns>
        public override IE Info(CGraphEdge edge){
            if (edge.M_OwnerGraph == m_graph) {
                return (IE) edge[m_infoKey];
            }
            throw new Exception("The given edge does not belong to the graph");
        }
        public override IE TempInfo(CGraphEdge edge){
            if (edge.M_OwnerGraph == m_graph) {
                return (IE) edge[edge];
            }
            throw new Exception("The given edge does not belong to the graph");
        }

        /// <summary>
        /// Returns the information of the edge between the given source
        /// and target nodes of the source graph. If the information is
        /// not there it returns null instead of dropping an exception
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="key">The key that will extract the information from the specified node's dictionary.If
        /// null is given then the current Query Info object is used as a key</param>
        /// <returns></returns>
        public override IE Info(CGraphNode source, CGraphNode target) {
            if (source.M_OwnerGraph == m_graph && target.M_OwnerGraph == m_graph) {
                return (IE) m_graph.Edge(source, target)[m_infoKey];
            }
            throw new Exception("The given edge does not belong to the graph");
        }
        public override IE TempInfo(CGraphNode source, CGraphNode target) {
            if (source.M_OwnerGraph == m_graph && target.M_OwnerGraph == m_graph) {
                CGraphEdge edge = m_graph.Edge(source, target);
                return (IE) edge[edge];
            }
            throw new Exception("The given edge does not belong to the graph");
        }
        /// <summary>
        /// Returns information from the source graph under the specified key.
        /// If the information is not there it returns null instead of dropping
        /// an exception
        /// </summary>
        /// <param name="key">The key object</param>
        /// <returns>The information object</returns>
        public override IG Info() {
            return (IG)m_graph[m_infoKey];
        }
        public override IG TempInfo() {
            return (IG)m_graph[m_graph];
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
        public override void CreateInfo(CGraphNode node, IN info ){
            if (node.M_OwnerGraph == m_graph) {
                node[m_infoKey] = info;
            }
            else {
                throw new Exception("The given node does not belong to the graph");
            }
        }

        public override void CreateTempInfo(CGraphNode node, IN info ){
            if (node.M_OwnerGraph == m_graph) {
                node[node] = info;
            }
            else {
                throw new Exception("The given node does not belong to the graph");
            }
        }

        /// <summary>
        /// Stores the information at the specified edge. If information already
        /// exists, it is overwritten
        /// </summary>
        /// <param name="edge">The edge.</param>
        /// <param name="info">The information object.</param>
        /// <param name="key">The key that will extract the information from the specified node's dictionary. If
        /// null is given then the current Query Info object is used as a key</param>
        public override void CreateInfo(CGraphEdge edge, IE info ){
            if (edge.M_OwnerGraph == m_graph) {
                edge[m_infoKey] = info;
            }
            else {
                throw new Exception("The given edge does not belong to the graph");
            }

        }
        public override void CreateTempInfo(CGraphEdge edge, IE info ){
            if (edge.M_OwnerGraph == m_graph) {
                edge[edge] = info;
            }
            else {
                throw new Exception("The given edge does not belong to the graph");
            }
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
        public override void CreateInfo(CGraphNode source, CGraphNode target, IE info ){
            if (source.M_OwnerGraph == m_graph && target.M_OwnerGraph == m_graph) {
                m_graph.Edge(source, target)[m_infoKey] = info;
            }
            else {
                throw new Exception("One or two of the given nodes does not belong to the graph");
            }
        }
        public override void CreateTempInfo(CGraphNode source, CGraphNode target, IE info ){
            if (source.M_OwnerGraph == m_graph && target.M_OwnerGraph == m_graph) {
                CGraphEdge edge = m_graph.Edge(source, target);
                edge[edge] = info;
            }
            else {
                throw new Exception("One or two of the given nodes does not belong to the graph");
            }
        }

        /// <summary>
        /// Stores information in the source graph under the specified key
        /// </summary>
        /// <param name="info">The information object</param>
        /// <param name="key">The key object</param>
        public override void CreateInfo(IG info) {
            m_graph[m_infoKey] = info;
        }
        public override void CreateTempInfo(IG info) {
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