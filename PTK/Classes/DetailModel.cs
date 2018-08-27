using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;

namespace PTK
{
    public class DetailModel
    {
        public Assembly Assembly { get; private set; }
        public List<Detail> Details { get; private set; }

        public DetailModel()
        {
            Assembly = new Assembly();
            Details = new List<Detail>();
        }
        public DetailModel(Assembly _assembly)
        {
            Assembly = _assembly;
            Details = new List<Detail>();
        }
    }

    public class PriorityModel
    {
        public Assembly Assembly { get; private set; }
        public List<Detail> Details { get; private set; }
        public List<string> Priority { get; private set; }

        public PriorityModel()
        {
            Assembly = new Assembly();
            Details = new List<Detail>();
            Priority = new List<string>();
        }
        public PriorityModel(Assembly _assembly)
        {
            Assembly = _assembly;
            Details = new List<Detail>();
            Priority = new List<string>();
        }

        public void SetPriority(string _priorityText)
        {
            Priority = _priorityText.Split(',').ToList();
        }

        public bool SearchDetails()
        {
            foreach(Node n in Assembly.Nodes)
            {
                int ind = Assembly.Nodes.IndexOf(n);
                //List of elements belonging to the specified node
                List<Element1D> detailElems = Assembly.NodeMap.Where(p => p.Value.Contains(ind)).ToList().ConvertAll(p => p.Key);
                Details.Add(new Detail(n));
                if(!Details.Last().SetElements(detailElems, Priority))
                {
                    return false;
                }
            }
            return true;
        }

        public PriorityModel DeepCopy()
        {
            return (PriorityModel)base.MemberwiseClone();
        }
        public override string ToString()
        {
            string info;
            info = "<PriorityModel> Details:" + Details.Count.ToString();
            return info;
        }
        public bool IsValid()
        {
            return Details.Count != 0;
        }
    }

    public class Detail
    {
        public Node Node { get; private set; }
        //public List<Element1D> Elements { get; private set; }
        public Dictionary<Element1D,int> ElementsPriorityMap { get; private set; }
        public DetailType Type { get; private set; }
        //private int crossElementNum = 0;

        public Detail()
        {
            Node = new Node();
            ElementsPriorityMap = new Dictionary<Element1D, int>();
        }
        public Detail(Node _node)
        {
            Node = _node;
            ElementsPriorityMap = new Dictionary<Element1D, int>();
        }

        public bool SetElements(List<Element1D> _elements, List<string> _priority)
        {
            List<Element1D> crossElements = _elements.FindAll(e => !IsNodeEndPointAtElement(e));
            List<Element1D> cornerElements = _elements.FindAll(e => IsNodeEndPointAtElement(e));

            if (crossElements.Count >= 2)
            {
                Type = DetailType.XType;
                if (!SortElementsByPriority(ref crossElements, _priority))
                {
                    return false;
                }
            }
            else if(crossElements.Count == 1)
            {
                Type = DetailType.TType;
            }
            else
            {
                Type = DetailType.LType;
            }
            if (cornerElements.Count >= 2)
            {
                if(!SortElementsByPriority(ref cornerElements, _priority))
                {
                    return false;
                }
            }

            string preTag = "";
            int priorityIndex = 0;
            foreach(Element1D e in crossElements)
            {
                if (preTag != e.Tag)
                {
                    preTag = e.Tag;
                    priorityIndex++;
                }
                ElementsPriorityMap[e] = priorityIndex;
            }
            preTag = "";    //When CrossElement and CornerElement are the same tag
            foreach (Element1D e in cornerElements)
            {
                if (preTag != e.Tag)
                {
                    preTag = e.Tag;
                    priorityIndex++;
                }
                ElementsPriorityMap[e] = priorityIndex;
            }
            return true;
        }

        private bool SortElementsByPriority(ref List<Element1D> _elements, List<string> _priority)
        {
            //When rearranging by priority is not necessary
            if (_elements.Count <= 1 || _elements.ConvertAll(e => e.Tag).Distinct().Count() <= 1)
            {
                return true;
            }
            //Whether all Elements to be prioritized are input with priority
            if (_elements.ConvertAll(e => e.Tag).Except(_priority).Count() == 0)
            {
                _elements = _elements.OrderBy(e => _priority.IndexOf(e.Tag)).ToList();
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsNodeEndPointAtElement(Element1D _element)
        {
            if ( Node.Point.DistanceTo(_element.BaseCurve.PointAtStart)<=CommonProps.tolerances ||
                Node.Point.DistanceTo(_element.BaseCurve.PointAtEnd) <= CommonProps.tolerances)
            {
                return true;
            }
            return false;
        }

        public double SearchNodeParamAtElement(Element1D _element)
        {
            double p;
            _element.BaseCurve.ClosestPoint(Node.Point, out p);
            return p;
        }
    }


    public class GH_PriorityModel : GH_Goo<PriorityModel>
    {
        public GH_PriorityModel() { }
        public GH_PriorityModel(GH_PriorityModel other) : base(other.Value) { this.Value = other.Value.DeepCopy(); }
        public GH_PriorityModel(PriorityModel ass) : base(ass) { this.Value = ass; }
        public override IGH_Goo Duplicate()
        {
            return new GH_PriorityModel(this);
        }
        public override bool IsValid => base.m_value.IsValid();
        public override string TypeName => "PriorityModel";
        public override string TypeDescription => "A model prioritized for mating members";
        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class Param_PriorityModel : GH_PersistentParam<GH_PriorityModel>
    {
        public Param_PriorityModel() : base(new GH_InstanceDescription("PriorityModel", "PriorityModel", "A model prioritized for mating members", CommonProps.category, CommonProps.subcate0)) { }

        protected override System.Drawing.Bitmap Icon { get { return null; } }  //Set icon image

        public override Guid ComponentGuid => new Guid("0A29F71C-B30D-4244-82CB-1A5ADCD38FC6");

        protected override GH_GetterResult Prompt_Plural(ref List<GH_PriorityModel> values)
        {
            return GH_GetterResult.success;
        }

        protected override GH_GetterResult Prompt_Singular(ref GH_PriorityModel value)
        {
            return GH_GetterResult.success;
        }
    }
}
