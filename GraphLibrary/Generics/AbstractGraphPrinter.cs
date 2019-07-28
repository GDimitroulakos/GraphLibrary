
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GraphLibrary;

namespace GraphLibrary.Generics {

    /// <summary>
    /// This class wraps one or more AbstractGraphPrinters to create a
    /// graph file combining the graphs from all the given graph printers 
    /// </summary>
    /// <typeparam name="TGraph"></typeparam>
    public abstract class AbstractMultiGraphPrinter<TGraph> {
        protected internal List<AbstractGraphPrinter<TGraph>> m_graphPrinters=new List<AbstractGraphPrinter<TGraph>>();

        protected string m_filePath;

        // The events are risen in different occasions of the 
        // combined graph generation. The object parameter can 
        // offer additional information to the generator in order
        // to configure the generation before, after and between 
        // the graphs emmition. In our case it is a AbstractGraphPrinter<TGraph>.
        // The function returns a string that is extracted intact to the file
        public event Func<object,string> e_prelude;
        public event Func<object,string> e_epilogue;
        public event Func<object,string> e_intermediate_after;
        public event Func<object, string> e_intermediate_before;


        protected AbstractMultiGraphPrinter(string filePath) {
            m_filePath = filePath;
        }
        
        /// <summary>
        /// Add a new graph printer. The graph printers must be properly configured
        /// to allow combined extraction of the graphs to a single file
        /// </summary>
        /// <param name="graphPrinter"></param>
        public virtual void Add(AbstractGraphPrinter<TGraph> graphPrinter) {
            if (!m_graphPrinters.Contains(graphPrinter)) {
                m_graphPrinters.Add(graphPrinter);
            }
        }

        /// <summary>
        /// This function generates the representation of the embedded graphs in a file
        /// </summary>
        public virtual void Generate() {
            StreamWriter outf = new StreamWriter(m_filePath);

            // Prologue
            outf.WriteLine(e_prelude?.Invoke(null));
            foreach (AbstractGraphPrinter<TGraph> graphPrinter in m_graphPrinters) {
                outf.WriteLine(e_intermediate_before?.Invoke(graphPrinter));
                outf.WriteLine(graphPrinter.Print());
                // Intermediate glue code
                outf.WriteLine(e_intermediate_after?.Invoke(graphPrinter));
            }
            // Epilogue
            outf.WriteLine(e_epilogue?.Invoke(null));

            outf.Close();
        }
    }

    /// <summary>
    /// This class is the parent class of all classes that print CGraph objects
    /// Hence the class know the interface of CGraph. (Its build for CGraph).
    /// This class heirarchy is based on the Builder design pattern.
    /// </summary>
    public abstract class AbstractGraphPrinter<TGraph> {
        /// <summary>
        /// This is a reference to the graph that is printed
        /// </summary>
        protected internal TGraph m_graph;

        /// <summary>
        /// The key required to access information regarding specialized 
        /// graphs
        /// </summary>
        protected object m_infoKey=null;

        protected internal AbstractGraphPrinter(TGraph graph){
            m_graph = graph;
        }

        /// <summary>
        /// Prints the graph into a StringBuilder object
        /// </summary>
        /// <param name="onlyedges">if set to <c>true</c> [onlyedges].</param>
        /// <returns></returns>
        public abstract StringBuilder Print();

        /// <summary>
        /// Generates the graph representation to a file
        /// </summary>
        public abstract void Generate(string filepath, bool executeGenerator = true);

        /// <summary>
        /// The key required to access information regarding specialized 
        /// graphs
        /// </summary>
        protected object M_InfoKey {
            set { m_infoKey = value; }
            get { return m_infoKey;  }
        }

        public TGraph M_Graph => m_graph;
    }
}