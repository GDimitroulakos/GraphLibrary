

using System;
using System.Collections.Generic;
using System.Text;

namespace GraphLibrary.Generics {

    #region Graph Type Enumeration 
    /// <summary>
    /// An enumeration type that describes the type of the graph regarding the type of 
    /// edges in contains. When it has only directed edges is called Directed ( GT_DIRECTED)
    /// when it has non-directional edges its called Undirected ( GT_UNDIRECTED ) and when
    /// it has both directed and undirected edge its a mixed graph ( GT_MIXED ). A graph with 
    /// no edge can't have a type ( GT_NA) 
    /// </summary>
    public enum GraphType {
        GT_NA, GT_DIRECTED, GT_UNDIRECTED, GT_MIXED
    }
    #endregion

    /// <summary>
    /// This enumeration describes the type of graph element. ET_NA characterizes conditions
    /// such as "element not found in query". Can also be used as a boolean value with ET_NA
    /// representing "false" and the other values "true". For this reason look the extension
    /// method ToBoolean(this GraphElementType value) below
    /// </summary>
    public enum GraphElementType {
        ET_NA = 0, ET_NODE, ET_EDGE, ET_GRAPH
    }

    /// <summary>
    /// This interface contains primitive operations common 
    /// to any graph element ( node, edge, graph )
    /// </summary>
    [Serializable]
    public abstract class CGraphPrimitive {
        int M_SerialNumber { get; }

        /// <summary>
        /// A reference to the graph owning the element
        /// </summary>
        protected CGraph m_graph;

        /// <summary>
        /// Represents the owner graph of the node
        /// </summary>
        /// <value>
        /// Is the owner graph
        /// </value>
        public CGraph M_OwnerGraph {
            get { return m_graph; }
            set {
                if (m_graph == null || m_graph == value) {
                    m_graph = value;
                }
                else {
                    throw new Exception("elements cannot be reowned");
                }
            }
        }

        public GraphElementType M_ElementType { get; set; }

        /// <summary>
        /// Each element has a label which can be set either centrally from the CGraph
        /// <see cref="CGraph.SetNodeLabelContext"/>, or
        /// locally using the SetLabel() method <see cref="SetLabel"/> or
        /// automatically given a default label consisting of the serial number  
        /// a prefix indicating the type of element ( i.e for the case of nodes 
        /// <see cref="CGraphNode.ToString()"/>. This property is realized in the 
        /// descentant classes for nodes, edges and graphs correspondingly
        /// </summary>
        public string M_Label { get; set; }

        /// <summary>
        /// Represents permanent output that is stored by a graph processing
        /// algorithm.  key = this is reserved for temporary information
        /// </summary>
        protected Dictionary<object, object> m_algorithmOutput;

        /// <summary>
        /// Holds the type of info under a given key.
        /// </summary>
        private Dictionary<object, Type> m_keyToInfoTypeRecord = new Dictionary<object, Type>();

        /// <summary>
        /// This is a counter providing unique serial numbers to every graph element
        /// for naming purposes
        /// </summary>
        protected static int ms_UniqueSerialNumberCounter = 0;

        /// <summary>
        ///  Represents permanent information
        /// </summary>
        /// <param name="index">key accessing the information</param>
        /// <returns>Returns the information object referring to the node for the algorithm index.
        /// The returned object must be casted to the appropriate information object that is
        /// declared inside the algorithm object. The algorithm can be any object that may
        /// process the graph nodes eg. Iterators, Algorithms classes etc</returns>
        public object this[object index] {
            get {
                if (m_algorithmOutput.ContainsKey(index)) {
                    return m_algorithmOutput[index];
                }
                else {
                    return null;
                }
            }
            set {
                // The following code checks the type of assigned information if it is the same
                // as the existing. This check is performed at the object and the graph level. 
                // That is, the first check refers if the information has the same type with the 
                // information already existing in the node and the second check if the information
                // assigned is the same with the information stored in the other nodes in the graph.
                // Check if the is already placed in the graph object
                if (value != null) {
                    if (!m_keyToInfoTypeRecord.ContainsKey(index) ||
                        // If the key is there and the existing value has the same 
                        // type as the one assigned through the operator...
                        m_keyToInfoTypeRecord.ContainsKey(index) &&
                        value.GetType() == m_keyToInfoTypeRecord[index]) {

                        // Record the type of information given for the current element
                        m_keyToInfoTypeRecord[index] = value.GetType(); 

                        switch (M_ElementType) {
                            case GraphElementType.ET_NODE:
                                if (m_graph.m_nkeyToInfoTypeRecord.ContainsKey(index) &&
                                    m_graph.m_nkeyToInfoTypeRecord[index] != value.GetType()) {
                                    Console.WriteLine(
                                        "Warning!!! incompatible of information type assigned to one of the graph nodes.");
                                }
                                m_graph.m_nkeyToInfoTypeRecord[index] = m_keyToInfoTypeRecord[index];
                                break;
                            case GraphElementType.ET_EDGE:
                                if (m_graph.m_ekeyToInfoTypeRecord.ContainsKey(index) &&
                                    m_graph.m_ekeyToInfoTypeRecord[index] != value.GetType()) {
                                    Console.WriteLine(
                                        "Warning!!! incompatible type of information assigned to one of the graph edges.");
                                }
                                m_graph.m_ekeyToInfoTypeRecord[index] = m_keyToInfoTypeRecord[index];
                                break;
                            case GraphElementType.ET_GRAPH:
                                if (m_graph.m_gkeyToInfoTypeRecord.ContainsKey(index) &&
                                    m_graph.m_gkeyToInfoTypeRecord[index] != value.GetType()) {
                                    Console.WriteLine(
                                        "Warning!!! incompatible type of information assigned to the graph");
                                }
                                m_graph.m_gkeyToInfoTypeRecord[index] = m_keyToInfoTypeRecord[index];
                                break;
                        }
                    }
                    else {
                        Console.WriteLine("existing {0} != given {1}", m_keyToInfoTypeRecord[index].ToString(),
                            value.GetType().ToString());
                        throw new Exception("Incompatible info type for the given node information key!!!");
                    }
                }
                else {
                    m_keyToInfoTypeRecord[index] = null;
                    switch (M_ElementType) {
                        case GraphElementType.ET_NODE:
                            m_graph.m_nkeyToInfoTypeRecord[index] = m_keyToInfoTypeRecord[index];
                            break;
                        case GraphElementType.ET_EDGE:
                            m_graph.m_ekeyToInfoTypeRecord[index] = m_keyToInfoTypeRecord[index];
                            break;
                        case GraphElementType.ET_GRAPH:
                            m_graph.m_gkeyToInfoTypeRecord[index] = m_keyToInfoTypeRecord[index];
                            break;
                    }
                }

                m_algorithmOutput[index] = value;
            }
        }
        protected CGraphPrimitive(GraphElementType etype, CGraph owner = null) {
            m_algorithmOutput = new Dictionary<object, object>();
            M_ElementType = etype;
            m_graph = owner;
        }

        /// <summary>
        /// Local Label Setter: This method set the label of the current
        /// element independently from the other elements of the graph.
        /// </summary>
        /// <param name="label"></param>
        public abstract void SetLabel(string label);

        /// <summary>
        /// Returns a string that contains the element and its info contents
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            StringBuilder sBuilder = new StringBuilder();
            string s = "";

            switch (M_ElementType) {
                case GraphElementType.ET_NODE:
                    s = "Node ";
                    break;
                case GraphElementType.ET_EDGE:
                    s = "Edge ";
                    break;
                case GraphElementType.ET_GRAPH:
                    s = "Graph ";
                    break;
            }
            sBuilder.Append(s + M_Label + ":");
            sBuilder.AppendLine();

            foreach (KeyValuePair<object, object> key in m_algorithmOutput) {
                sBuilder.Append("key :" + key.Key.ToString());
                sBuilder.Append("info :" + key.Value.ToString());
            }

            if (M_ElementType == GraphElementType.ET_GRAPH) {
                CIt_GraphNodes itn = new CIt_GraphNodes(this as CGraph);
                for (itn.Begin(); !itn.End(); itn.Next()) {
                    sBuilder.Append(itn.M_CurrentItem.ToString());
                    sBuilder.AppendLine();
                }
                CIt_GraphEdges itg = new CIt_GraphEdges(this as CGraph);
                for (itg.Begin(); !itg.End(); itg.Next()) {
                    sBuilder.Append(itg.M_CurrentItem.ToString());
                    sBuilder.AppendLine();
                }
            }
            return sBuilder.ToString();
        }

        public abstract string ToInfoString(object infokey);
    }


    // This class hosts extension methods
    static class GraphUtilsExtensions {
        public static bool ToBoolean(this GraphElementType value) {
            return value != GraphElementType.ET_NA;
        }
    }
}