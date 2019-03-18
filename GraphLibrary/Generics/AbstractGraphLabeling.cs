using System.Collections.Generic;
using System.Data;
using GraphLibrary;

namespace GraphLibrary.Generics{
    
    /// <summary>
    /// Provides a generic interface for retrieving the label of a
    /// graph element of type T ( edge, node, whatever... ). Thus
    /// this class applies to all types of graph elements.
    /// </summary>
    /// <typeparam name="T">The type of graph element</typeparam>
    public interface IGraphLabeling<T> where T : CGraphPrimitive {
        /// <summary>
        /// Returns the Label of the specified element.
        /// </summary>
        /// <param name="element">The labeled element</param>
        /// <returns>The label</returns>
        string Label(T element);

        /// <summary>
        /// Returns the element that is labelled with the given label
        /// </summary>
        /// <param name="label">The label</param>
        /// <returns>The element</returns>
        T Element(string label);

        /// <summary>
        /// Sets the label of the given element. The element must exist in the graph
        /// and the label must not be already used by another element of the same type (node, edge)
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="label">The label.</param>
        void SetLabel(T element, string label);
    }

    /// <summary>
    /// This class holds the labels for a specific type of elements (edge , nodes, graphs )
    /// of a graph. I use two dictionaries for both mappings to gain in query speed. This
    /// class gives the opportunity that diffrent graph clients can assign diffrent labels
    /// to the graph nodes, edges etc. The graph client ( contructor ) creates a subclass of 
    /// AbstractGraphLabeling<T> and assigns labels to nodes. The instance of this class exists
    /// inside the CGraph class and the client can add a new labeling of graph elements through
    /// the AddGraphNodeLabelling() method.
    /// </summary>
    /// <typeparam name="TElement">Type of element to label</typeparam>
    /// <seealso cref="IGraphLabeling{T}" />
    public abstract class AbstractGraphLabeling<TElement> : IGraphLabeling<TElement> where TElement:CGraphPrimitive {
        
        /// <summary>
        /// The mapping of elements to labels is represented by the dictionary variable
        /// m_ElementLabels
        /// </summary>
        protected Dictionary<TElement, string> m_LabelsIndexedByElements;

        /// <summary>
        /// The mapping of labels to elements is represented by the dictionary variable
        /// </summary>
        protected Dictionary<string, TElement> m_ElementsIndexedByLabels;

        public AbstractGraphLabeling() {
            m_LabelsIndexedByElements = new Dictionary<TElement, string>();
            m_ElementsIndexedByLabels = new Dictionary<string, TElement>();
            
        }

        /// <summary>
        /// Returns the label of the given element.
        /// </summary>
        /// <param name="element">The labeled element</param>
        /// <returns>
        /// The label
        /// </returns>
        public virtual string Label(TElement element) {
            return m_LabelsIndexedByElements[element];
        }

        
        /// <summary>
        /// Returns the element that is labelled with the given label
        /// </summary>
        /// <param name="label">The label</param>
        /// <returns>
        /// The element
        /// </returns>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException"></exception>
        public virtual TElement Element(string label) {
            return m_ElementsIndexedByLabels[label];
        }
        

        /// <summary>
        /// Sets the label of the given element. The element must exist in the graph
        /// and the label must not be already used by another element of the same type (node, edge)
        /// The method is implemented in the concrete subclasses ( thats why is abstract because 
        /// it depends on the graph class implementation )
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="label">The label.</param>
        public abstract void SetLabel(TElement element, string label);

        /// <summary>
        /// Removes the element and its label.
        /// </summary>
        /// <param name="element">The element.</param>
        public abstract void RemoveElement(TElement element);

        /// <summary>
        /// Gives the initial mapping of labels to elements.
        /// Called by the constructor. Must be defined in subclasses
        /// </summary>
        protected abstract void LabelElements();

        /// <summary>
        /// Relabels the elements according to some algorithm.
        /// This version does nothing
        /// </summary>
        protected virtual void RelabelElements(){}
    }
}