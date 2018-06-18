using Rhino.Geometry;
using Rhino.Geometry.Intersect;
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
        public List<Element1D> Elements { get; private set; }
        public List<Node> Nodes { get; private set; }
        public List<Section> Sections { get; private set; }
        public List<Material> Materials { get; private set; }
        public Dictionary<Element1D,List<int>> NodeMap { get; private set; }
        #endregion

        #region constructors
        public Assembly()
        {
            Elements = new List<Element1D>();
            Nodes = new List<Node>();
            Materials = new List<Material>();
            Sections = new List<Section>();
            NodeMap = new Dictionary<Element1D, List<int>>();
        }
        #endregion

        #region properties
        #endregion

        #region methods
        public int AddElement(Element1D _element)
        {
            if (!Elements.Contains(_element))
            {
                SearchNodes(_element);

                Elements.Add(_element);
                Section sec = _element.Section;
                if (!Sections.Contains(sec))
                {
                    Sections.Add(sec);
                    Material mat = sec.Material;
                    if (!Materials.Contains(mat))
                    {
                        Materials.Add(mat);
                    }
                }
            }
            return Elements.Count;
        }

        private void SearchNodes(Element1D _element)
        {
            if (!NodeMap.ContainsKey(_element))
            {
                NodeMap.Add(_element, new List<int>());
            }

            //エレメントの両端をノードとして登録
            AddPointToNodeMap(_element, _element.PointAtStart);
            AddPointToNodeMap(_element, _element.PointAtEnd);

            //他エレメントとの交点をノードとして登録
            foreach(Element1D otherElem in Elements.FindAll(e => e.IsIntersectWithOther == true))
            {
                var events = Intersection.CurveCurve(otherElem.BaseCurve, _element.BaseCurve, Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance, 0.0);
                if(events != null)
                {
                    foreach(IntersectionEvent e in events)
                    {
                        if (!_element.IsIntersectWithOther) //他部材と交差しないときは端点交差のみ検出
                        {
                            if (e.PointA == _element.BaseCurve.PointAtStart || e.PointA == _element.BaseCurve.PointAtEnd)
                            {
                                AddPointToNodeMap(otherElem, e.PointA);
                            }
                        }
                        else
                        {
                            AddPointToNodeMap(_element, e.PointA);
                            AddPointToNodeMap(otherElem, e.PointA);
                            if (e.IsOverlap)    //重複が区間の場合
                            {
                                AddPointToNodeMap(_element, e.PointA2);
                                AddPointToNodeMap(otherElem, e.PointA2);
                            }
                        }
                    }
                }
                else
                {
                    continue;
                }
            }

        }

        private void AddPointToNodeMap(Element1D _element, Point3d _point)
        {
            if (!Nodes.Exists(n => n.Equals(_point)))   //その位置にノードが無い場合
            {
                Nodes.Add(new Node(_point));
                NodeMap[_element].Add(Nodes.Count-1);
            }
            else
            {
                int ind = Nodes.FindIndex(n => n.Equals(_point));   //すでにノードがある場合、そのインデックス
                if (!NodeMap[_element].Contains(ind))
                {
                    NodeMap[_element].Add(ind);
                }
            }
        }
        
        public List<double> SearchNodeParamAtElement(Element1D _element)
        {
            List<double> param = new List<double>();
            if (NodeMap.ContainsKey(_element))
            {
                foreach(int i in NodeMap[_element])
                {
                    double p;
                    _element.BaseCurve.ClosestPoint(Nodes[i].Point, out p);
                    param.Add(p);
                }
                param.Sort();
            }

            return param;
        }
        #endregion

    }

    public class AssemblyForStructural
    {
        #region fields
        public Assembly Assembly { get; private set; }
        public List<ElementForStructural> SElements { get; private set; }
        public List<Support> Supports { get; private set; }
        public List<Load> Loads { get; private set; }
        #endregion
        #region constructors
        public AssemblyForStructural()
        {
            Assembly = new Assembly();
            SElements = new List<ElementForStructural>();
            Supports = new List<Support>();
            Loads = new List<Load>();
        }
        public AssemblyForStructural(Assembly _assembly)
        {
            Assembly = _assembly;
            SElements = new List<ElementForStructural>();
            Supports = new List<Support>();
            Loads = new List<Load>();
        }
        #endregion
        #region properties
        #endregion
        #region methods
        public int AddSElement(ElementForStructural _sElement)
        {
            if (!SElements.Contains(_sElement))
            {
                SElements.Add(_sElement);
            }
            return SElements.Count;
        }

        public int AddSupport(Support _support)
        {
            if (!Supports.Contains(_support))
            {
                Supports.Add(_support);
            }
            return Supports.Count;
        }

        public int AddLoad(Load _load)
        {
            if (!Loads.Contains(_load))
            {
                Loads.Add(_load);
            }
            return Loads.Count;
        }
        #endregion
    }
}
