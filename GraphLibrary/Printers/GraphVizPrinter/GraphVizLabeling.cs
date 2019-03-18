using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphLibrary.Generics;

namespace GraphLibrary.Printers.GraphVizPrinter {


    public class CCondensedGraphVizLabelling : CGraphLabeling<CGraphNode> {
       
        private string m_prefix;

        /// <summary>
        /// Labels 
        /// </summary>
        /// <param name="prefix">The label prefix</param>
        /// <param name="graph">The condensed graph</param>
        public CCondensedGraphVizLabelling(string prefix, CGraph graph) : base(graph) {
            m_prefix = prefix;
            LabelElements();
        }

        protected override void LabelElements() {
            string label;
            int serialNumber = 0;
            // Create iterator
            CIt_GraphNodes it = new CIt_GraphNodes(m_graph);
            for (it.Begin(); !it.End(); it.Next()){
                label = m_prefix + serialNumber++;
                SetLabel(it.M_CurrentItem, label);
            }
        }
    }

    /// <summary>
    /// Assign labels to nodes for graphviz printing
    /// </summary>
    /// <seealso cref="CGraphNode" />
    public class GraphVizNodeLabeling : CGraphLabeling<CGraphNode> {
        
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphVizNodeLabeling"/> class.
        /// </summary>
        /// <param name="graph">The graph.</param>
        public GraphVizNodeLabeling(CGraph graph) : base(graph) {
            LabelElements();
        }

        /// <summary>
        /// Gives the initial mapping of labels to elements.
        /// Called by the constructor. Must be defined in subclasses
        /// </summary>
        protected override void LabelElements() {
            string label;
            // Create iterator
            CIt_GraphNodes it = new CIt_GraphNodes(m_graph);
            for (it.Begin(); !it.End(); it.Next()) {
               label = it.M_CurrentItem.ToString();
               SetLabel(it.M_CurrentItem, label);
            }
        }
    }
}