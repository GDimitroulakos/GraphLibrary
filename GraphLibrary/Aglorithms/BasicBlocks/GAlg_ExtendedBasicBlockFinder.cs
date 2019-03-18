using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphLibrary.Algorithms;
using GraphLibrary.Printers.GraphVizPrinter;

namespace GraphLibrary.Aglorithms.BasicBlocks
{

    // This class identifies extended basic blocks in rooted, directed graphs.
    // The outcome is a index array of tuples carrying the extended basic block root as 
    // the index and the set with the basic block contents (m_allEbbs).
    // Each extended basic block carries nodes which have a single predecessor other than
    // the first node (which may have more) and multiple successors.
    public class GAlg_ExtendedBasicBlockFinder : CGraphAlgorithm<int> {
        // SOURCE 
        private CGraph m_sourceGraph;

        // OUTPUT
        /// <summary>
        /// The result extended basic block graph
        /// </summary>
        private CCondensedGraph m_extendedBasicBlockGraph = null;

        // CONTEXT
        /// <summary>
        /// The algorithm deposits the roots of extended basic blocks, 
        /// as they are identified, for further processing    
        /// </summary>
        private List<CGraphNode> m_ebbRoots;

        /// <summary>
        /// Each extended basic block is identified by its root. After the root of block
        /// is identified the algorithm gathers the nodes to the block until another root
        /// is identified 
        /// </summary>
        private Dictionary<CGraphNode, List<CGraphNode>> m_allEbbs;

        /// <summary>
        /// Represents the current extended basic block 
        /// </summary>
        private List<CGraphNode> m_currentEbb;

        /// <summary>
        /// Parameter Constructor
        /// </summary>
        /// <param name="inputGraph">The graph for which the extended blocks are to be found</param>
        public GAlg_ExtendedBasicBlockFinder(AlgorithmDataRecord io_arg) :base(io_arg)  {
            m_ebbRoots = new List<CGraphNode>();
            m_allEbbs = new Dictionary<CGraphNode, List<CGraphNode>>();
           // AddSourceGraph(inputGraph);
           // m_sourceGraph = inputGraph;
        }

        public override void SetAlgorithmicInterface(AlgorithmDataRecord info) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// THis method iterates over the remaining extended basic block roots
        /// and calls visit to assemble the block for each root
        /// </summary>
        public override void Init() {
            int nRoots = m_sourceGraph.GetRootNodes(m_ebbRoots);
            if (nRoots != 1){
                throw new Exception("Wrong type of graph!!! Only single rooted graphs accepted");
            }

            // For all extended basic block leaders
            while (m_ebbRoots.Count != 0) {
                // Take the first leader...
                CGraphNode x = m_ebbRoots.First();
                //... and remove it from the list
                m_ebbRoots.RemoveAt(0);

                // If leader hasn't already been considered
                if (!m_allEbbs.ContainsKey(x)) {
                    // Add new extended basic block entry in EbbRoots
                    m_currentEbb = m_allEbbs[x] = new List<CGraphNode>();
                    // Insert leader to the extended basic block
                    Visit(x);
                }
            }

            // Create exdended basic block graph
            m_extendedBasicBlockGraph = CCondensedGraph.CreateGraph(m_sourceGraph,m_allEbbs.Values);
            AddOutputGraph(m_extendedBasicBlockGraph);
         
            
            // Print graph
            m_extendedBasicBlockGraph.RegisterGraphPrinter(new CCondensedGraphVizPrinter(m_extendedBasicBlockGraph,
                new CCondensedGraphVizLabelling("cluster", m_extendedBasicBlockGraph)));
            m_extendedBasicBlockGraph.Generate("extended basic block graph.dot", true);
        }

        /// <summary>
        /// Assembles the nodes of a specific extended basic block. The graph
        /// is traversed from a node to its successors and the algorithm selects
        /// the ones who have one predesessor
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public override int Visit(CGraphNode node) {

            // Insert node to the extended basic block
            m_currentEbb.Add(node);

            foreach (CGraphNode successor in node.M_Successors) {
                if (successor.M_NumberOfPredecessors == 1 && !m_currentEbb.Contains(successor)) {
                    // Insert successor to the extended basic block
                    Visit(successor);
                }
                else if (!m_ebbRoots.Contains(successor)) {
                    m_ebbRoots.Add(successor);
                }
            }

            return base.Visit(node);
        }

        public Dictionary<CGraphNode, List<CGraphNode>> M_ExtendedBasicBlocks {
            get { return m_allEbbs; }
        }
    }
}
