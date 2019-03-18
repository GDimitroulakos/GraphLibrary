

namespace GraphLibrary.Generics{
    /// <summary>
    /// Every iterator should implement the following interface. Look for more details
    /// in the <seealso cref="AbstractGraphIterator"/> class
    /// </summary>
    /// <typeparam name="T">The type of elements that this iterator iterates on</typeparam>
    public interface IGraphIterator<T> {

        T Begin();
        bool End();
        T Next();
    }

    /// <summary>
    /// This class is the base abstract class of GraphIterators. It has no implementation
    /// except from the m_currentItem member variable. This variable always point to the
    /// current element (which has type T) that the iterator points.
    /// The class's methods are executed in a specific sequence that is dictated by the loop
    /// statement semantics that is
    /// Begin(), End(), LOOP_BODY, Next(), End(), LOOP_BODY, Next(), End(), LOOP_BODY,
    /// Next(), End()... etc
    /// </summary>
    /// <typeparam name="T">The type of elements that this iterator iterates on</typeparam>
    /// <seealso cref="GraphLibrary.Generics.IGraphIterator{T}" />
    /// <seealso cref="CGraphNode" />
    public abstract class AbstractGraphIterator<T> : IGraphIterator<T> {

        /// <summary>
        /// Current node pointed to by the iterator
        /// </summary>
        protected T m_currentItem = default(T);

        protected int m_iterations;

        /// <summary>
        /// Returns the current item pointed by the iterator
        /// </summary>
        public T M_CurrentItem {
            get { return m_currentItem; }
        }

        /// <summary>
        /// Returns the number of times the iterator has been increased
        /// </summary>
        public int M_Iterations {
            get { return m_iterations; }
        }

        /// <summary>
        /// Executes once at the start of the iteration and before the End() method
        /// Initializes the iterator variable ( realized in the subclasses ) and the
        /// m_currentItem to point to the first element of the item set. If the set is 
        /// empty the m_currentItem points to null. After the Begin() method the 
        /// End() method executes before going into the loop body. Thus the loop 
        /// terminates if the item set is empty before ever running the loop body
        /// The transition logic for finding the first item in the items set is 
        /// implemented in the subclasses
        /// </summary>
        /// <returns></returns>
        public abstract T Begin();

        /// <summary>
        /// Executes after the Begin() or Next() methods and before the Loop Body
        /// Returns true if the m_currentItem is null. This case arises in the following
        /// scenarios:
        /// 1) The item set is empty thus, after the Begin() method call the End() identifies
        /// that m_currentItem points to null
        /// 2) The item set is not empty and the iterator went outside the boundaries of the 
        /// item set after the call to Next() method (Next() method set m_currentItem to null)   
        /// </summary>
        /// <returns></returns>
        public abstract bool End();

        /// <summary>
        /// Executes after the execution of the loop body and before the End() method
        /// Increases the iteration variable and set the m_currentItem to point to the current
        /// item of the item set. In case where the end of the item set is reached the m_currentItem
        /// points to null which is identified by the End() methods that is executed next.
        /// The transition logic from item to item is implemented from the subclasses.
        /// </summary>
        /// <returns></returns>
        public abstract T Next();
    }

}