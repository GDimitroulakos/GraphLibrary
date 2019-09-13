using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphLibrary.Generics;

namespace GraphLibrary{

    /// <summary>
    /// This class provides general graph labelling capabilities. It doesn't follow
    /// a specific strategy to label nodes/edges. It just implements the default version
    /// of SetLabel methods for the class CGraph. (This class is specific to CGraph class)
    /// </summary>
    /// <typeparam name="T">Type of element to label</typeparam>
    /// <seealso cref="GraphLibrary.Generics.AbstractGraphLabeling{T}" />
    public class CGraphLabeling<T> : AbstractGraphLabeling<T> where T : CGraphPrimitive {
        protected CGraph m_graph;


        public CGraphLabeling(CGraph graph) {
            m_graph = graph;
        }

        public override void RemoveElement(T element) {
            string label;
            //1. Check if the element exists
            if (m_LabelsIndexedByElements.ContainsKey(element)) {
                //1a. EXISTS
                label = m_LabelsIndexedByElements[element];

                m_LabelsIndexedByElements.Remove(element);
                m_ElementsIndexedByLabels.Remove(label);

            }
        }

        /// <summary>
        /// Sets the label of the given element. The element must exist in the graph
        /// and the label must not be already used by another element of the same type
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="label">The label.</param>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">Element does not exist or label already exists in the graph</exception>
        public override void SetLabel(T element, string label) {
            string oldLabel;

            //1. Check if the element exists
            if (m_LabelsIndexedByElements.ContainsKey(element)) {
                //1a. EXISTS
                //2. Check if another element has the same label
                if (m_ElementsIndexedByLabels.ContainsKey(label) && m_ElementsIndexedByLabels[label] != element) {
                    //2a. EXISTS
                    // 4.Issue an Exception. "A dublicate label exists"
                    throw new Exception("A identical label already utilized by another element");
                }
                else {
                    //2b. NOT EXIST
                    // 5. Issue a Warning. OVERWRITE the label for the given element

                    //Console.WriteLine("Warning!!!. Label ovewritted");

                    oldLabel = m_LabelsIndexedByElements[element];

                    // substitute the label in the label-key collection
                    m_ElementsIndexedByLabels.Remove(oldLabel);

                    //  Update both collections
                    m_LabelsIndexedByElements[element] = label;

                    m_ElementsIndexedByLabels[label] = element;
                }
            }
            else {
                //1b. NOT EXISTS
                //6. Check if the label is utilized by another element
                if (!m_ElementsIndexedByLabels.ContainsKey(label)) {
                    //6a. NO OTHER ELEMENT HAS THE LABEL
                    //3. Create a new entry for the given element, label
                    //  Update both collections
                    m_LabelsIndexedByElements[element] = label;

                    m_ElementsIndexedByLabels[label] = element;
                }
                else {
                    //6b. LABEL ALREADY UTILIZED
                    //7. Issue an exception "An identical label already exists"
                    throw new Exception("An identical label already exists");
                }
            }
        }

        /// <summary>
        /// Gives the initial mapping of labels to elements.
        /// Called by the constructor. Must be defined in subclasses
        /// The version in this class doesn't do anything which means that
        /// the labeller node and edge labelling lies on the methods SetLabel() of
        /// this class
        /// </summary>
        protected override void LabelElements() { }

        /// <summary>
        /// Returns the graph for which this labeller is indented
        /// </summary>
        public CGraph M_Graph {
            get { return m_graph;  }
        }
    }
}
