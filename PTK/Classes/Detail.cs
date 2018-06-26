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

        private List<PTK_Element> elems;
        private List<int> elemsIds;


        #endregion
        #region constructors
        public Detail(List<Node> _nodes, List<PTK_Element> _elems)
        {
            nodes = new List<Node>();
            nodeIds = new List<int>();
            elems = new List<PTK_Element>();
            elemsIds = new List<int>();


            nodes = _nodes;
            elems = _elems;
            elemsIds = new List<int>();
            nodeIds = new List<int>();
            foreach (Node node in _nodes)
            {
                nodeIds.Add(node.Id);
            }
            foreach (PTK_Element elem in _elems)
            {
                elemsIds.Add(elem.Id);
            }


        }
        public Detail()
        {

        }
        #endregion
        #region properties
        public List<Node> Nodes { get { return nodes; } }
        public List<PTK_Element> Elems { get { return elems; } }
        public List<int> NodeIds { get { return nodeIds; } }
        public List<int> ElemsIds { get { return elemsIds; } }

        #endregion
        #region methods
        #endregion
    }
}
