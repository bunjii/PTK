using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTK
{
    public class Assembly
    {
        #region fields
        private List<Node> nodes;
        private List<Element> elems;
        private List<DetailingGroup> detailingGroups;
        #endregion

        #region constructors
        public Assembly(List<Node> _nodes, List<Element> _elems, List<DetailingGroup> _detailingGroups)
        {
            nodes = _nodes;
            elems = _elems;
            detailingGroups = _detailingGroups;
        }
        public Assembly(List<Node> _nodes, List<Element> _elems)
        {
            nodes = _nodes;
            elems = _elems;
            
        }


        #endregion

        #region properties
        public List<Node> Nodes
        {
            get { return nodes; }
            set { nodes = value; }
        }
        public List<Element> Elems
        {
            get { return elems; }
            set { elems = value; }
        }

        public List<DetailingGroup> DetailingGroups
        {
            get { return detailingGroups; }
            set { detailingGroups = value; }
        }
        #endregion

        #region methods
        #endregion

    }
}
