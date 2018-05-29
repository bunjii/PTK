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
        public List<Node> Nodes { get; private set; }
        public List<Element> Elems { get; private set; }
        public List<Material> Mats { get; private set; }
        public List<Section> Secs { get; private set; }
        public List<Support> Sups { get; private set; }
        #endregion

        #region constructors
        public Assembly(List<Node> _nodes, List<Element> _elems, List<Material> _mats,
            List<Section> _secs, List<Support> _sups)
        {
            Nodes = _nodes;
            Elems = _elems;
            Mats = _mats;
            Secs = _secs;
            Sups = _sups;
        }
        #endregion

        #region properties
        #endregion

        #region methods
        #endregion

    }
}
