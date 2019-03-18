
using System.Text;
using GraphLibrary;

namespace GraphLibrary.Generics {

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
    }
}