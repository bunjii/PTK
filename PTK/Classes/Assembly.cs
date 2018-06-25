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

        private List<Loads> loads;
        private List<Karamba.Models.GH_Model> krmb_GH_model;
        private List<DetailingGroup> detailingGroups;
        #endregion

        #region constructors
        public Assembly() { }
        public Assembly(List<Node> _nodes, List<Element> _elems, List<Material> _mats,
            List<Section> _secs, List<Support> _sups, List<Loads> _loads, List<DetailingGroup> _detailingGroup)
        {
            Nodes = _nodes;
            Elems = _elems;
            Mats = _mats;
            Secs = _secs;
            Sups = _sups;
            loads = _loads;
            detailingGroups = _detailingGroup;
        }
        public Assembly(List<Node> _nodes, List<Element> _elems, List<Material> _mats,
            List<Section> _secs, List<Support> _sups, List<Loads> _loads, List<Karamba.Models.GH_Model> _krmb_GH_model, List<DetailingGroup> _detailingGroup)
        {
            Nodes = _nodes;
            Elems = _elems;
            Mats = _mats;
            Secs = _secs;
            Sups = _sups;
            loads = _loads;
            krmb_GH_model = _krmb_GH_model;
            detailingGroups = _detailingGroup;
        }
        #endregion

        #region properties
        public List<Loads> Loads
        {
            get { return loads; }
            set { loads = value; }
        }
        public List<Karamba.Models.GH_Model> Krmb_GH_model
        {
            get { return krmb_GH_model; }
            set { krmb_GH_model = value; }
        }

        public List<DetailingGroup> DetailingGroups { get { return detailingGroups; } }


        #endregion

        #region methods
        #endregion

    }
}
