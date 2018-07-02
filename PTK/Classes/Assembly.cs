using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;

namespace PTK
{
    public class Assembly
    {
        #region fields
        public List<Element1D> Elements { get; private set; }
        public List<Node> Nodes { get; private set; }
        public List<string> Tags { get; private set; }
        public List<CrossSection> CrossSections { get; private set; }
        public List<Material> Materials { get; private set; }
        public Dictionary<Element1D,List<int>> NodeMap { get; private set; }
        public Dictionary<CrossSection, Material> CrossSectionMap { get; private set; }

        #endregion

        #region constructors
        public Assembly()
        {
            Elements = new List<Element1D>();
            Nodes = new List<Node>();
            Tags = new List<string>();
            CrossSections = new List<CrossSection>();
            Materials = new List<Material>();
            NodeMap = new Dictionary<Element1D, List<int>>();
            CrossSectionMap = new Dictionary<CrossSection, Material>();
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
                string tag = _element.Tag;
                if (!Tags.Contains(tag))
                {
                    Tags.Add(tag);
                }
                foreach(CrossSection sec in _element.Sections)
                {
                    if(!CrossSectionMap.ContainsKey(sec))
                    {
                        CrossSections.Add(sec);
                        Material mat = sec.Material;
                        CrossSectionMap.Add(sec, mat);
                        if (!Materials.Contains(mat))
                        {
                            Materials.Add(mat);
                        }
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

        public Assembly DeepCopy()
        {
            return (Assembly)base.MemberwiseClone();
        }
        public override string ToString()
        {
            string info;
            info = "<Assembly> Elements:" + Elements.Count.ToString() +
                " Nodes:" + Nodes.Count.ToString() +
                " Materials:" + Materials.Count.ToString() +
                " Sections:" + CrossSections.Count.ToString();
            return info;
        }
        public bool IsValid()
        {
            return Elements.Count != 0;
        }
        #endregion

    }

    public class StructuralAssembly
    {
        #region fields
        public Assembly Assembly { get; private set; }
        public List<StructuralElement> SElements { get; private set; }
        public List<Support> Supports { get; private set; }
        public List<Load> Loads { get; private set; }
        #endregion
        #region constructors
        public StructuralAssembly()
        {
            Assembly = new Assembly();
            SElements = new List<StructuralElement>();
            Supports = new List<Support>();
            Loads = new List<Load>();
        }
        public StructuralAssembly(Assembly _assembly)
        {
            Assembly = _assembly;
            SElements = new List<StructuralElement>();
            Supports = new List<Support>();
            Loads = new List<Load>();
        }
        #endregion
        #region properties
        #endregion
        #region methods
        public int AddSElement(StructuralElement _sElement)
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

        public StructuralAssembly DeepCopy()
        {
            return (StructuralAssembly)base.MemberwiseClone();
        }
        public override string ToString()
        {
            string info;
            info = "<StructuralAssembly> StructuralElements:" + SElements.Count.ToString() +
                " Supports:" + Supports.Count.ToString() +
                " Loads:" + Loads.Count.ToString();
            return info;
        }
        public bool IsValid()
        {
            return SElements.Count != 0;
        }
        #endregion
    }

    public class GH_Assembly : GH_Goo<Assembly>
    {
        public GH_Assembly() { }
        public GH_Assembly(GH_Assembly other) : base(other.Value) { this.Value = other.Value.DeepCopy(); }
        public GH_Assembly(Assembly ass) : base(ass) { this.Value = ass; }
        public override IGH_Goo Duplicate()
        {
            return new GH_Assembly(this);
        }
        public override bool IsValid => base.m_value.IsValid();
        public override string TypeName => "Assembly";
        public override string TypeDescription => "A model that gathers elements and has intersection points";
        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class Param_Assembly : GH_PersistentParam<GH_Assembly>
    {
        public Param_Assembly() : base(new GH_InstanceDescription("Assembly", "Assembly", "A model that gathers elements and has intersection points", CommonProps.category, CommonProps.subcate0)) { }

        protected override System.Drawing.Bitmap Icon { get { return null; } }  //クラスにアイコンを付けたい場合はここ

        public override Guid ComponentGuid => new Guid("E49369AA-4F29-498E-9808-E3197929FF51");

        protected override GH_GetterResult Prompt_Plural(ref List<GH_Assembly> values)
        {
            return GH_GetterResult.success;
        }

        protected override GH_GetterResult Prompt_Singular(ref GH_Assembly value)
        {
            return GH_GetterResult.success;
        }
    }
    public class GH_StructuralAssembly : GH_Goo<StructuralAssembly>
    {
        public GH_StructuralAssembly() { }
        public GH_StructuralAssembly(GH_StructuralAssembly other) : base(other.Value) { this.Value = other.Value.DeepCopy(); }
        public GH_StructuralAssembly(StructuralAssembly ass) : base(ass) { this.Value = ass; }
        public override IGH_Goo Duplicate()
        {
            return new GH_StructuralAssembly(this);
        }
        public override bool IsValid => base.m_value.IsValid();
        public override string TypeName => "StructuralAssembly";
        public override string TypeDescription => "A model that gathers elements and has intersection points";
        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class Param_StructuralAssembly : GH_PersistentParam<GH_StructuralAssembly>
    {
        public Param_StructuralAssembly() : base(new GH_InstanceDescription("StructuralAssembly", "StructuralAssembly", "A model that gathers elements and has intersection points", CommonProps.category, CommonProps.subcate0)) { }

        protected override System.Drawing.Bitmap Icon { get { return null; } }  //クラスにアイコンを付けたい場合はここ

        public override Guid ComponentGuid => new Guid("4B468C32-EC87-47F8-A995-0832EDADEBA0");

        protected override GH_GetterResult Prompt_Plural(ref List<GH_StructuralAssembly> values)
        {
            return GH_GetterResult.success;
        }

        protected override GH_GetterResult Prompt_Singular(ref GH_StructuralAssembly value)
        {
            return GH_GetterResult.success;
        }
    }
}
