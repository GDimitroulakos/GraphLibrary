using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphLibrary.Aglorithms;
using GraphLibrary.Algorithms;

namespace GraphLibrary.Aglorithms.BasicBlocks
{
    class GAlg_CondensedGraphBuilder : CGraphAlgorithm<int> {
        // SOURCE
        private CGraph m_sourceGraph;

        /// <summary>
        /// The original graph nodes that are contained per condensed graph node
        /// </summary>
        private ICollection<List<CGraphNode>> m_condensedNodeContents;

        
        // OUTPUT
        private CCondensedGraph m_condensedGraph;
        
        // CONTEXT

        public GAlg_CondensedGraphBuilder(AlgorithmDataRecord input) : base(input) {
            m_sourceGraph = input.GetIArgumentOriginGraph();
            m_condensedNodeContents = m_sourceGraph[input.GetIArgumentGlobalKey()];
            
        }

        public override void Init() {
            // Create output graph
            AddOutputGraph(m_condensedGraph);

            // TODO!!!  Check the existence of none unique nodes per condensed node 
            // TODO!!!!!
            CIt_GraphEdges it = new CIt_GraphEdges(m_sourceGraph);
            CGraphEdge edge, newEdge;
            CGraphNode newNode;

            CCondensedGraph newgraph = new CCondensedGraph(originalGraph);

            // Add condensed nodes
            foreach (List<CGraphNode> condensedNodeContent in condensedNodeContents){
                newNode = newgraph.CreateGraphNode(condensedNodeContent);
            }
            // Add condensed edges
            for (edge = it.Begin(); !it.End(); edge = it.Next()){
                // Map the original graph edge to a condensed graph edge
                newEdge = newgraph.AddCondensedEdge(edge);
                if (newEdge != null){

                }
            }
            return newgraph;

        }

        public CGraphNode CreateGraphNode(List<CGraphNode> nodeSet) {

            // Every original graph node must participate in only one
            // condensed graph node
            foreach (CGraphNode node in nodeSet) {
                if (m_Mappings.IsOriginalNodeContained(node)) {
                    throw new Exception("Every original graph node must participate in only one condensed graph node");
                }
            }
            CGraphNode newnode = CreateGraphNode();
            m_Mappings.SetDerivedToOriginalNodeMappings(newnode,nodeSet);
            //m_condensedNodeMappings[newnode] = nodeSet;
            foreach (CGraphNode node in nodeSet) {
                if (m_Mappings.IsOriginalNodeContained(node)) {
                    m_Mappings.SetOriginalToDerivedNode(node,newnode);
                }
                else {
                    throw new Exception("Node participates in more than 1 condensed nodes");
                }
            }
            return newnode;
        }

        /// <summary>
        /// Adds a node in the specified existing condensed node if it doesn't already 
        /// exists in the condensed node 
        /// </summary>
        /// <param name="condensedNode"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public CGraphNode AddNodeToCondensedNode(CGraphNode condensedNode, CGraphNode node) {
            if (Contains(condensedNode)!= GraphElementType.ET_NA &&
                !CondensedNodeContains(condensedNode, node)) {
                m_condensedNodeMappings[condensedNode].Add(node);
                m_nodeMappings[node] = condensedNode;
                return node;
            }
            return null;
        }

        /// <summary>
        /// Checks if there is an existing condensed map edge for the given 
        /// original Graph edge
        /// </summary>
        /// <param name="originalGraphEdge"></param>
        /// <returns></returns>
        public CGraphEdge AddCondensedEdge(CGraphEdge originalGraphEdge) {
            CGraphEdge newedge = null;
            // Add the edge if :
            // 1. The original graph edges has not already being mapped to a condensed graph edge
            // 2. Source and Target nodes belong to different condensed node ( covers the case of
            // one or both being null )
            if (m_edgeOriginalToCondensedMappings[originalGraphEdge]==null &&
                m_nodeMappings[originalGraphEdge.M_Source] != m_nodeMappings[originalGraphEdge.M_Target]) {
                newedge = AddGraphEdge(m_nodeMappings[originalGraphEdge.M_Source],
                    m_nodeMappings[originalGraphEdge.M_Target],originalGraphEdge.M_EdgeType);
                m_edgeOriginalToCondensedMappings[originalGraphEdge] = newedge;
                m_edgeCondensedToOriginalMappings[newedge] = originalGraphEdge;
            }
            return newedge;
        }
        
        public override void SetAlgorithmicInterface(AlgorithmDataRecord input) {
            
            
        }

        
    }


}
