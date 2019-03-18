using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using GraphLibrary.Generics;

namespace GraphLibrary {

    /// <summary>
    /// This class is the parent class of all classes that print CGraph objects
    /// Hence the class know the interface of CGraph. (Its build for CGraph).
    /// This class heirarchy is based on the Builder design pattern.
    /// </summary>
    public abstract class CGraphPrinter : AbstractGraphPrinter<CGraph> {
        protected AbstractGraphLabeling<CGraphNode> m_nodeLabelling;
        protected AbstractGraphLabeling<CGraphEdge> m_edgeLabelling;

        /// <summary>
        /// Initializes a new instance of the <see cref="CGraphPrinter"/> class.
        /// </summary>
        /// <param name="graph">The graph.</param>
        protected internal CGraphPrinter(CGraph graph, AbstractGraphLabeling<CGraphNode> nodeLabeller=null,
            AbstractGraphLabeling<CGraphEdge> edgeLabeller=null) : base(graph) {
            if (nodeLabeller != null) {
                graph.SetNodeLabelContext(nodeLabeller);
            }
            else {
                graph.SetNodeLabelContext();
            }
            

            if (edgeLabeller != null) {
                m_edgeLabelling = edgeLabeller;
            }
            else {
                m_edgeLabelling = m_graph.GetNativeEdgeLabelling();
            }
        }

        /// <summary>
        /// This indexer facilitates the access to the graph nodes labels. The indexer's
        /// type is CGraphNode and returns the label as a string
        /// Gets or sets the <see cref="System.String"/> at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="System.String"/>.
        /// </value>
        /// <param name="index">The index as CGraphNode object</param>
        /// <returns>The label of the node</returns>
        public string this[CGraphNode index]{
            get { return m_graph.GetNodeLabel(this, index); }
        }
    }
}