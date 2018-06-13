using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTK
{
    public class Detail
    {
        #region fields
        private List<Node> nodes;
        private List<int> nodeIds;

        private List<Element> elems;
        private List<int> elemsIds;


        #endregion
        #region constructors
        public Detail(List<Node>_nodes, List<Element> _elems)
        {
            nodes = _nodes;
            elems = _elems;
            elemsIds = new List<int>();
            nodeIds = new List<int>();
            foreach (Node node in _nodes)
            {
                nodeIds.Add(node.ID);
            }
            foreach (Element elem in _elems)
            {
                elemsIds.Add(elem.ID);
            }


        }
        public Detail()
        {

        }
        #endregion
        #region properties
        public List<Node> Nodes { get { return nodes; } }
        public List<Element> Elems { get { return elems; } }
        public List<int> NodeIds { get { return nodeIds; } }
        public List<int> ElemsIds { get { return elemsIds; } }

        #endregion
        #region methods
        #endregion
    }
}
