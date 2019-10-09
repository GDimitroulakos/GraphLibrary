using System;
using GraphLibrary.Generics;



namespace GraphLibrary {

    /// <summary>
    /// Query info from the CGraph class for a specific graph and key
    /// IN, IE, IG are the type of information stored in the nodes, edges and graph
    /// correspondingly. The class contains a key to access the information. When the
    /// information is publicly available to any object the same make the key a public
    /// constant ( i.e string ). When the information refers to a particular algorithm
    /// that concerns only specific objects use as a key the instance of the class
    /// that produces the information
    /// </summary>
    /// <seealso cref="CGraph" />
    [Serializable]
    public class CGraphQueryInfo<IN,IE,IG> : AbstractGraphQueryInfo<CGraph,CGraphNode, CGraphEdge,IN,IE,IG> {

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
        /// <param name="checkOwnership">Checks if the node belongs to the same graph</param>
        /// <returns></returns>
        public override IN Info(CGraphNode node, bool checkOwnership = true) {
            
            if (!checkOwnership) {
                return (IN)node[m_infoKey];
            }
            else {
                if (node.M_OwnerGraph == m_graph) {
                    return (IN)node[m_infoKey];
                }
                else {
                    throw new Exception("The given node does not belong to the graph");
                }
            }
        }
        public override IN TempInfo(CGraphNode node, bool checkOwnership = true) {

            if (!checkOwnership) {
                return (IN)node[node];
            }
            else {
                if (node.M_OwnerGraph == m_graph) {
                    return (IN)node[node];
                }
                else {
                    throw new Exception("The given node does not belong to the graph");
                }
            }
        }
        
        /// <summary>
        /// Returns the information of the specified edge. If the information
        /// is not there it returns null instead of dropping an exception
        /// </summary>
        /// <param name="edge">The edge.</param>
        /// <param name="key">The key that will extract the information from the specified node's dictionary. If
        /// null is given then the current Query Info object is used as a key</param>
        /// <returns></returns>
        public override IE Info(CGraphEdge edge, bool checkOwnership = true) {

            if (!checkOwnership) {
                return (IE)edge[m_infoKey];
            }
            else {
                if (edge.M_OwnerGraph == m_graph) {
                    return (IE)edge[m_infoKey];
                }
                else {
                    throw new Exception("The given edge does not belong to the graph");
                }
            }
        }
        public override IE TempInfo(CGraphEdge edge, bool checkOwnership = true) {

            if (!checkOwnership) {
                return (IE)edge[edge];
            }
            else {
                if (edge.M_OwnerGraph == m_graph) {
                    return (IE)edge[edge];
                }
                else {
                    throw new Exception("The given edge does not belong to the graph");
                }
            }
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
        public override IE Info(CGraphNode source, CGraphNode target, bool checkOwnership = true) {

            if (!checkOwnership) {
                return (IE)m_graph.Edge(source, target)[m_infoKey];
            }
            else {
                if (source.M_OwnerGraph == m_graph && target.M_OwnerGraph == m_graph) {
                    return (IE)m_graph.Edge(source, target)[m_infoKey];
                }
                else {
                    throw new Exception("The given edge does not belong to the graph");
                }
            }
        }
        public override IE TempInfo(CGraphNode source, CGraphNode target, bool checkOwnership = true) {
            if (!checkOwnership) {
                CGraphEdge edge = m_graph.Edge(source, target);
                return (IE)edge[edge];
            }
            else {
                if (source.M_OwnerGraph == m_graph && target.M_OwnerGraph == m_graph) {
                    CGraphEdge edge = m_graph.Edge(source, target);
                    return (IE)edge[edge];
                }
                else {
                    throw new Exception("The given edge does not belong to the graph");
                }
            }
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
        public override void CreateInfo(CGraphNode node, IN info, bool checkOwnership = true) {
            if (!checkOwnership) {
                node[m_infoKey] = info;
            }
            else {
                if (node.M_OwnerGraph == m_graph) {
                    node[m_infoKey] = info;
                }
                else {
                    throw new Exception("The given node does not belong to the graph");
                }
            }
        }

        public override void CreateTempInfo(CGraphNode node, IN info, bool checkOwnership = true) {
            if (!checkOwnership) {
                node[node] = info;
            }
            else {
                if (node.M_OwnerGraph == m_graph) {
                    node[node] = info;
                }
                else {
                    throw new Exception("The given node does not belong to the graph");
                }
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
        public override void CreateInfo(CGraphEdge edge, IE info, bool checkOwnership = true) {
            if (!checkOwnership) {
                edge[m_infoKey] = info;
            }
            else {
                if (edge.M_OwnerGraph == m_graph) {
                    edge[m_infoKey] = info;
                }
                else {
                    throw new Exception("The given edge does not belong to the graph");
                }
            }
        }
        public override void CreateTempInfo(CGraphEdge edge, IE info, bool checkOwnership = true) {
            if (!checkOwnership) {
                edge[edge] = info;
            }
            else {
                if (edge.M_OwnerGraph == m_graph) {
                    edge[edge] = info;
                }
                else {
                    throw new Exception("The given edge does not belong to the graph");
                }
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
        public override void CreateInfo(CGraphNode source, CGraphNode target, IE info, bool checkOwnership = true) {

            if (!checkOwnership) {
                m_graph.Edge(source, target)[m_infoKey] = info;
            }
            else {
                if (source.M_OwnerGraph == m_graph && target.M_OwnerGraph == m_graph) {
                    m_graph.Edge(source, target)[m_infoKey] = info;
                }
                else {
                    throw new Exception("The given edge does not belong to the graph");
                }
            }
        }
        public override void CreateTempInfo(CGraphNode source, CGraphNode target, IE info, bool checkOwnership = true) {

            if (!checkOwnership) {
                CGraphEdge edge = m_graph.Edge(source, target);
                edge[edge] = info;
            }
            else {
                if (source.M_OwnerGraph == m_graph && target.M_OwnerGraph == m_graph) {
                    CGraphEdge edge = m_graph.Edge(source, target);
                    edge[edge] = info;
                }
                else {
                    throw new Exception("The given edge does not belong to the graph");
                }
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
        
        public object M_InfoKey {
            get { return m_infoKey; }
        }
    }

}