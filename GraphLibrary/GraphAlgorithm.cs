using System;
using System.Collections.Generic;
using GraphLibrary.Algorithms;
using GraphLibrary.Generics;

namespace GraphLibrary {
    // An algorithm consists of three basic ingredients:
    // 1) A graph where the algorithm acts upon
    // 2) The algorithm in the form of a class
    // 3) Information derived during the execution of the algorithm


    /// <summary>
    /// This is an abstract class that represents algorithms that run on graphs with
    /// the following types of elements
    /// 1) CGraphNode for nodes
    /// 2) CGraphEdge for edges
    /// 3) CGraph for graphs
    /// </summary>
    /// <typeparam name="T">The type of element returned by the visit function</typeparam>
    /// <seealso cref="CGraph" />
    public abstract class CGraphAlgorithm<T> : AbstractGraphAlgorithm<T, CGraphNode, CGraphEdge, CGraph> {

        /// <summary>
        /// Describes the arguments of the algorithm
        /// </summary>
        protected static AlgorithmDataRecord m_algorithmInterface=null;

        protected CGraphAlgorithm() {
        }

        public override object VisitChildren(CGraphNode node){
            CIt_Successors it = new CIt_Successors(node);

            for (it.Begin(); !it.End(); it.Next()){
                Visit(it.M_CurrentItem);
            }
            return null;
        }
        
        /// <summary>
        /// Returns the specified edge from the graph to which the algorithm acts
        /// </summary>
        /// <param name="source">The source node</param>
        /// <param name="target">The target node</param>
        /// <returns></returns>
        //protected CGraphEdge Edge(object keyinfo,CGraphNode source, CGraphNode target) {
        //   return m_sourceGraphs[keyinfo].M_Graph.Edge(source, target);
        //}

    }
    
}