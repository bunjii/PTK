using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private List<Material> mats;
        private List<Section> secs;
        private List<Support> sups;
        #endregion

        #region constructors
        public Assembly(List<Node> _nodes, List<Element> _elems, List<Material> _mats,
            List<Section> _secs, List<Support> _sups)
        {
            nodes = _nodes;
            elems = _elems;
            mats = _mats;
            secs = _secs;
            sups = _sups;
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
        public List<Material> Mats
        {
            get { return mats; }
            set { mats = value; }
        }
        public List<Section> Secs
        {
            get { return secs; }
            set { secs = value; }
        }
        public List<Support> Sups
        {
            get { return sups; }
            set { sups = value; }
        }
        #endregion

        #region methods
        #endregion

    }
}
