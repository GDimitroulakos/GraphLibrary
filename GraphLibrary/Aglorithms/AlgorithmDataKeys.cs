using System;
using System.Collections.Generic;


namespace GraphLibrary.Algorithms {

    
    /// <summary>
    /// Describes the input/output arguments of an algorithm instance. For each argument
    /// it records:
    /// 1) Its type
    /// 2) Description
    /// 3) The key that index the argument into the node, edge, graph etc
    /// 4) Its local key that characterizes in respect to the algorithm that belongs
    /// 5) The graph from which it comes from
    /// </summary>
    public class AlgorithmDataRecord {
        
        //INPUT ARGUMENTS
        /// <summary>
        /// string: Description (The standard description of the output argument)
        /// int : global key ( Should be dynamically assigned ) and it is the key that
        ///       index the argument into the node, edge, graph etc
        /// CGraph : The graph from which the argument originates
        /// </summary>
        private List<Tuple<string,int,CGraph>> m_inputArgumentsRecords;

        //OUTPUT
        /// <summary>
        /// string: Description (The standard description of the output argument)
        /// int : global key ( Should be dynamically assigned ) and it is the key that
        ///       index the argument into the node, edge, graph etc
        /// /// CGraph : The graph to which the argument has its destination
        /// </summary>
        private List<Tuple<string, int, CGraph>> m_outputArgumentsRecords;


       public string GetIArgumentDescription(int index = 0) {
            return m_inputArgumentsRecords[index].Item1;
        }
        public int GetIArgumentGlobalKey(int index = 0) {
            return m_inputArgumentsRecords[index].Item2;
        }
        public CGraph GetIArgumentOriginGraph(int index = 0) {
            return m_inputArgumentsRecords[index].Item3;
        }
        

       
        public string GetOArgumentDescription(int index = 0) {
            return m_outputArgumentsRecords[index].Item1;
        }
        public int GetOArgumentGlobalKey(int index = 0) {
            return m_outputArgumentsRecords[index].Item2;
        }
        public CGraph GetOArgumentOriginGraph(int index = 0) {
            return m_outputArgumentsRecords[index].Item3;
        }
        

        public int GetNumberOfInputArguments() {
            return m_inputArgumentsRecords.Count;
        }
        public int GetNumberOfOutputArguments() {
            return m_outputArgumentsRecords.Count;
}
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="algorithmType"></param>
        public AlgorithmDataRecord() {
            m_inputArgumentsRecords = new List<Tuple<string, int,CGraph>>();
            m_outputArgumentsRecords = new List<Tuple<string, int,CGraph>>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="argumentType"></param>
        /// <param name="description"></param>
        /// <param name="localKey"></param>
        public Tuple<string, int, CGraph> AddInputArgument( string description, CGraph inputGraph, int globalkey=-1) {
            Tuple<string, int, CGraph> newarg = Tuple.Create(description,globalkey, inputGraph );
            m_inputArgumentsRecords.Add(newarg);
            return newarg;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="argumentType"></param>
        /// <param name="description"></param>
        /// <param name="localKey"></param>
        public Tuple<string, int, CGraph> AddOutputArgument(string description, CGraph outputGraph, int globalkey=-1) {
            Tuple<string, int, CGraph> newarg = Tuple.Create( description,
                outputGraph.GetStorageReservationKey(),  outputGraph);
            m_outputArgumentsRecords.Add(newarg);
            return newarg;
        }
    }
}