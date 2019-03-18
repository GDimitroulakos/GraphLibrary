using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Text;
using GraphLibrary.Aglorithms;
using GraphLibrary.Generics;
using GraphLibrary.Printers.GraphVizPrinter;

namespace GraphLibrary {

   /* public class CDFSSpanningTreeGraphvizPrinter : CGraphPrinter {
        public CDFSSpanningTreeGraphvizPrinter(CGraph graph, object key=null,
            AbstractGraphLabeling<CGraphNode> nodeLabeller = null, 
            AbstractGraphLabeling<CGraphEdge> edgeLabeller = null) : base(graph, nodeLabeller, edgeLabeller) {
            M_InfoKey = key;
        }

        public override StringBuilder Print() {
             // Allocate a stringbuiler object and allocate space for 1000 characters
            StringBuilder graphvizStringBuilder = new StringBuilder(1000);
            string graphedge_operator = " ";
            string header = "";
            string headerProperties = "";
            object infoKey;
            EdgeTypeDFS etype;
            CIt_GraphNodes itn = new CIt_GraphNodes(m_graph);
            CIt_GraphEdges itg = new CIt_GraphEdges(m_graph);

            switch (m_graph.M_GraphType) {
                case GraphType.GT_UNDIRECTED:
                    graphedge_operator = "--";
                    header = "graph G" + m_graph.M_SerialNumber + "{\r\n";
                    header += headerProperties;
                    break;
                case GraphType.GT_DIRECTED:
                    graphedge_operator = "->";
                        header = "digraph G" + m_graph.M_SerialNumber + "{\r\n";
                        header += headerProperties;
                    break;
                case GraphType.GT_MIXED:
                    break;
            }

            // Print header if necessary
            graphvizStringBuilder.Append(header);


            // Print all  nodes
            for (itn.Begin(); !itn.End(); itn.Next()) {
                graphvizStringBuilder.Append("\"" +itn.M_CurrentItem.M_Label+"\";");
            }

            // Acquire graph information key
            if (M_InfoKey == null) {
                infoKey = m_graph;
            }
            else {
                infoKey = M_InfoKey;
            }

            // Print all edges of the graph
            for (itg.Begin(); !itg.End(); itg.Next()) {
                CGraphEdge g = itg.M_CurrentItem;
                etype = ((EdgeInfo_DFSSpanningTree) g[infoKey]).M_Type;

                string source, target;
                source = g.M_Source.M_Label;
                target = g.M_Target.M_Label; 

                graphvizStringBuilder.AppendFormat("\"{0}\"" + graphedge_operator + "\"{1}\"",
                   source, target);

                switch (etype) {
                        case EdgeTypeDFS.forward:
                        graphvizStringBuilder.AppendFormat(" [style=dotted, label=\"f\"]");
                        break;
                        case EdgeTypeDFS.back:
                        graphvizStringBuilder.AppendFormat(" [style=dotted, label=\"b\"]");
                        break;
                        case EdgeTypeDFS.cross:
                        graphvizStringBuilder.AppendFormat(" [style=dotted, label=\"c\"]");
                        break;
                }
                graphvizStringBuilder.Append(";\r\n");
            }
            // Print footer if necessary
            graphvizStringBuilder.Append("}\r\n");
            return graphvizStringBuilder;
        }

        public override void Generate(string filepath, bool executeGenerator = true) {
            // Open a streamwriter
            using (StreamWriter fstream = new StreamWriter(filepath)){
                fstream.WriteLine(Print());
                fstream.Close();
            }
            // Prepare the process dot to run
            ProcessStartInfo start = new ProcessStartInfo();
            // Enter in the command line arguments, everything you would enter after the executable name itself
            start.Arguments = "-Tgif " +
                                "\"" +Path.GetFileName(filepath) + "\"" + " -o " +"\"" +
                                Path.GetFileNameWithoutExtension(filepath) + ".gif\"";
            // Enter the executable to run, including the complete path
            start.FileName = "dot";
            // Do you want to show a console window?
            start.WindowStyle = ProcessWindowStyle.Hidden;
            start.CreateNoWindow = true;
            int exitCode;

            // Run the external process & wait for it to finish
            using (Process proc = Process.Start(start)){
                proc.WaitForExit();

                // Retrieve the app's exit code
                exitCode = proc.ExitCode;
            }
        }
    }*/


   /* /// <summary>
    /// This class prints a condensed (clustered) graph to graphviz
    /// </summary>
    public class CCondensedGraphVizPrinter : CGraphPrinter {
        private CCondensedGraph m_condensedGraph;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="condensedGraph">Condensed graph</param>
        /// <param name="nodeLabeller">Node labelling contructor</param>
        /// <param name="edgeLabeller">Edge labelling contrucotr</param>
        public CCondensedGraphVizPrinter(CCondensedGraph condensedGraph, AbstractGraphLabeling<CGraphNode> nodeLabeller = null,
            AbstractGraphLabeling<CGraphEdge> edgeLabeller = null) : base(condensedGraph, nodeLabeller, edgeLabeller) {
            m_condensedGraph = condensedGraph;
        }

        public override StringBuilder Print() {
            StringBuilder output = new StringBuilder(1000);
            CIt_GraphNodes it1 = new CIt_GraphNodes(m_graph);
            CIt_GraphEdges it3 = new CIt_GraphEdges(m_graph);
            int clusterNumber;
            string clusterHeader="";

            output.Append("digraph X{\n");
            output.Append("\tcompound = true;\n");

            // Output Condensed nodes
            clusterNumber = 0;
            for (it1.Begin(); !it1.End(); it1.Next()) {
                // Iterate over the nodes of the condensed graph
                clusterHeader += "\n\tsubgraph " + it1.M_CurrentItem.M_Label +"{\n";
                clusterHeader += "\t\tlabel=" + it1.M_CurrentItem.M_Label+";\n";
                clusterHeader += "\t\t";
                clusterNumber++;
                CIt_CondensedGraphNodes it2 = new CIt_CondensedGraphNodes(m_graph as CCondensedGraph, it1.M_CurrentItem);
                for (it2.Begin(); !it2.End(); it2.Next()) {
                    // Iterate over the subnodes of a condensed node
                    clusterHeader += it2.M_CurrentItem.M_Label + ';';
                }
                clusterHeader += "\n\t}";
            }
            output.Append(clusterHeader);

            // Output original edges
            clusterHeader = "";
            CIt_GraphEdges it4 = new CIt_GraphEdges(m_condensedGraph.M_OriginalGraph);
            for (it4.Begin(); !it4.End(); it4.Next()) {
                clusterHeader += "\n\t\"" + it4.M_CurrentItem.M_Source.M_Label + "\" -> \"" +
                                 it4.M_CurrentItem.M_Target.M_Label + "\";";
            }

            // Output Condesned Edges
            CIt_GraphEdges it5 = new CIt_GraphEdges(m_condensedGraph);

            if (false) {
                CGraphEdge oedge, cedge;
                for (it5.Begin(); !it5.End(); it5.Next()) {
                    cedge = it5.M_CurrentItem;
                    oedge = m_condensedGraph.GetOriginalMappedEdge(cedge);
                    clusterHeader += "\n\t\"" + oedge.M_Source.M_Label + "\" -> \"" +
                                     oedge.M_Target.M_Label + "\"" +
                                     "[ltail=" + cedge.M_Source + ", lhead=" + cedge.M_Target + " ,style=dotted];";
                }
            }
            else {
                CGraphEdge oedge, cedge;
                for (it5.Begin(); !it5.End(); it5.Next()) {
                    cedge = it5.M_CurrentItem;
                    oedge = m_condensedGraph.GetOriginalMappedEdge(cedge);
                    clusterHeader += "\n\t\"" + cedge.M_Source.M_Label + "\" -> \"" +
                                     cedge.M_Target.M_Label + "\"" +
                                     "[style=dotted];";
                }
            }

            output.Append(clusterHeader);

            output.Append("\n}");

            return output;
        }

        public override void Generate(string filepath, bool executeGenerator = true) {
            // Open a streamwriter
            using (StreamWriter fstream = new StreamWriter(filepath)){
                fstream.WriteLine(Print());
                fstream.Close();
            }
            // Prepare the process dot to run
            ProcessStartInfo start = new ProcessStartInfo();
            // Enter in the command line arguments, everything you would enter after the executable name itself
            start.Arguments = "-Tgif " +
                                "\"" +Path.GetFileName(filepath) + "\"" + " -o " +"\"" +
                                Path.GetFileNameWithoutExtension(filepath) + ".gif\"";
            // Enter the executable to run, including the complete path
            start.FileName = "dot";
            // Do you want to show a console window?
            start.WindowStyle = ProcessWindowStyle.Hidden;
            start.CreateNoWindow = true;
            int exitCode;

            // Run the external process & wait for it to finish
            using (Process proc = Process.Start(start)){
                proc.WaitForExit();

                // Retrieve the app's exit code
                exitCode = proc.ExitCode;
            }
        }
    }*/
    
    /// <summary>
    /// This class exports the graph in graphviz form. It accepts the graph
    /// and optionally a class that defined the labelling of nodes. If the
    /// client doen't provide a labelling class a default class (GraphVizNodeLabeling)
    /// takes over  the labelling of nodes
    /// </summary>
    /// <seealso cref="GraphLibrary.CGraphPrinter" />
    public class CGraphVizPrinter : CGraphPrinter {

        public CGraphVizPrinter(CGraph graph, CGraphLabeling<CGraphNode> nodeLabeling = null,
            CGraphLabeling<CGraphEdge> edgeLabeling=null ) : base(graph,nodeLabeling,edgeLabeling) {
        }

        /// <summary>
        /// Prints the graph into a StringBuilder object.
        /// Optionally the header and footer of the .dot file can be ommited for use of the
        /// graph edges in the multi layer graph printer.
        /// </summary>
        /// <param name="onlyedges">if set to <c>true</c> [onlyedges] the graph is printed as a standalone graph.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override StringBuilder Print(){
            // Allocate a stringbuiler object and allocate space for 1000 characters
            StringBuilder graphvizStringBuilder = new StringBuilder(1000);
            string graphedge_operator = " ";
            string header = "";
            string headerProperties = "";
            CIt_GraphNodes itn = new CIt_GraphNodes(m_graph);
            CIt_GraphEdges itg = new CIt_GraphEdges(m_graph);

            switch (m_graph.M_GraphType) {
                case GraphType.GT_UNDIRECTED:
                    graphedge_operator = "--";
                    header = "graph G" + m_graph.M_SerialNumber + "{\r\n";
                    header += headerProperties;
                    break;
                case GraphType.GT_DIRECTED:
                    graphedge_operator = "->";
                        header = "digraph G" + m_graph.M_SerialNumber + "{\r\n";
                        header += headerProperties;
                    break;
                case GraphType.GT_MIXED:
                    break;
            }

            // Print header if necessary
            graphvizStringBuilder.Append(header);


            // Print all  nodes
            for (itn.Begin(); !itn.End(); itn.Next()) {
                graphvizStringBuilder.Append("\"" +itn.M_CurrentItem.M_Label+"\";");
            }
            // Print all edges of the graph
            for (itg.Begin(); !itg.End(); itg.Next()) {
                CGraphEdge g = itg.M_CurrentItem;

                string source, target;
                source = g.M_Source.M_Label;
                target = g.M_Target.M_Label; 

                graphvizStringBuilder.AppendFormat("\"{0}\"" + graphedge_operator + "\"{1}\"",
                   source, target);
                graphvizStringBuilder.AppendFormat(" [style = bold, label = \""+g.M_Label +"\"]");

                graphvizStringBuilder.Append(";\r\n");
            }
            // Print footer if necessary
            graphvizStringBuilder.Append("}\r\n");
            return graphvizStringBuilder;
        }

        /// <summary>
        /// Generates the GraphViz .dot file and optionally calls the graphviz to
        /// generate the picture of the graph.
        /// </summary>
        /// <param name="filepath">The full path to which the file is generated</param>
        /// <param name="executeGenerator">If true calls the dot tool to produce the graph in a picture</param>
        public override void Generate(string filepath, bool executeGenerator = true){
            // Open a streamwriter
            using (StreamWriter fstream = new StreamWriter(filepath)){
                fstream.WriteLine(Print());
                fstream.Close();
            }
            // Prepare the process dot to run
            ProcessStartInfo start = new ProcessStartInfo();
            // Enter in the command line arguments, everything you would enter after the executable name itself
            start.Arguments = "-Tgif " +
                                Path.GetFileName(filepath) + " -o " +
                                Path.GetFileNameWithoutExtension(filepath) + ".gif";
            // Enter the executable to run, including the complete path
            start.FileName = "dot";
            // Do you want to show a console window?
            start.WindowStyle = ProcessWindowStyle.Hidden;
            start.CreateNoWindow = true;
            int exitCode;

            // Run the external process & wait for it to finish
            using (Process proc = Process.Start(start)){
                proc.WaitForExit();

                // Retrieve the app's exit code
                exitCode = proc.ExitCode;
            }
        }
    }
}