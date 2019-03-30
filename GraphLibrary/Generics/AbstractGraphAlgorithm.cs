

using System;
using System.Collections;
using System.Collections.Generic;
using GraphLibrary.Algorithms;

namespace GraphLibrary.Generics{

    /// <summary>
    /// The interface represents the mandatory interface of the algorithm class.
    /// </summary>
    /// <typeparam name="T">The parameter refers to the type of the return result</typeparam>
    /// <typeparam name="Node">The type of the node.</typeparam>
    public interface IGraphAlgorithm<T, TNode> {
        /// <summary>
        /// The Visit method refers to the action the algorithm performs to a specific node
        /// Visit function can be called recursively or iteratively.
        /// </summary>
        /// <param name="node">The node to which the algorithm acts.</param>
        /// <returns>The parameter refers to the type of the return result</returns>
        T Visit(TNode node);

        /// <summary>
        /// Visits the children of the specified node.
        /// </summary>
        /// <param name="node">The node to which the children is visited</param>
        /// <returns>an object of type object</returns>
        object VisitChildren(TNode node);
    }

    /// <summary>
    /// The abstract base class of algorithms applied on graphs. An algorithm is
    /// assumed to accept one or more graphs and produce the same graph or a new
    /// graph or multiple new graphs with additional information which can reside
    /// on the nodes, the edges or the graph itself
    /// </summary>
    /// <typeparam name="T">Visit function retured type</typeparam>
    /// <typeparam name="Node">The type of the node.</typeparam>
    /// <typeparam name="Edge">The type of the edge.</typeparam>
    /// <typeparam name="Graph">The type of the Graph.</typeparam>
    /// <seealso cref="GraphLibrary.Generics.IGraphAlgorithm{T,Node}" />
    /// <seealso cref="IGraphAlgorithm{T,Node}" />
    /// <seealso cref="IGraphAlgorithm{T,Node}" />
    public abstract class AbstractGraphAlgorithm<T,TNode,TEdge,TGraph> : IGraphAlgorithm<T,TNode> {

        /// <summary>
        /// A dictionary of graph info indexed by the data provider object
        /// </summary>
        protected Dictionary<int, object> m_algorithmData;



        public object this[int index] {
            get { return m_algorithmData[index]; }
            set{ m_algorithmData[index] = value; }
        }


        /// <summary>
        /// Initializes an instance of the AbstractGraphAlgorithm class
        /// </summary>
        /// <param name="iteratorFactory"></param>
        protected AbstractGraphAlgorithm() {
            m_algorithmData = new Dictionary<int,object>();
            
        }
        
        /// <summary>
        /// Initializes the algorithm. The client writes code in this function in the
        /// subclasses to initialize the algorithm and create if necessary temporary info
        /// </summary>
        public abstract void Init();

        /// <summary>
        /// The function the executes the algorithm
        /// </summary>
        /// <returns></returns>
        public abstract T Run();

        
        /// <summary>
        /// The Visit method refers to the action the algorithm performs to a specific node
        /// This function can be called either recursively or iteratively
        /// </summary>
        /// <param name="node">The node to which the algorithm acts.</param>
        /// <returns>
        /// The parameter refers to the type of the return result
        /// </returns>
        public virtual T Visit(TNode node) {
            return default(T);
        }

        /// <summary>
        /// Visits the specified edge and performs the algorithmic action on it.
        /// </summary>
        /// <param name="edge">The edge to which the algorithm acts</param>
        /// <returns></returns>
        public virtual T Visit(TEdge edge) {
            return default(T);
        }

        /// <summary>
        /// Visits the children of the specified node. Call this function if
        /// nothing is necessesary to be done in the current node apart from 
        /// visiting its children
        /// </summary>
        /// <param name="node">The node to which the children is visited</param>
        /// <returns>
        /// The default version returns null
        /// </returns>
        /// 
        public abstract object VisitChildren(TNode node);
        
    }
}