using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary.Aglorithms
{

    /// <summary>
    /// Graph node info to assist the algorithm. Only the algorithm has
    /// access to the type of the Info_DFS class. There could be also
    /// puclic Info class if the information is shared between diffrent 
    /// algorithms. There is an instance of the Info_DFS class in each
    /// graph node
    /// </summary>
    public class NodeInfo_DFS {
        public int m_color;

        public NodeInfo_DFS() { }
        public NodeInfo_DFS(int color) {
            this.m_color = color;
        }
    }

    /// <summary>
    /// Graph edge info to assist the algorithm. 
    /// There is an instance of the Info_DFS class in each
    /// graph node
    /// </summary>
    public class EdgeInfo_DFS  {
        public int m_color;

        public EdgeInfo_DFS() { }
        public EdgeInfo_DFS(int color) {
            this.m_color = color;
        }
    }

    
}
