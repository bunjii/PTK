using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void SearchDetails()
        {
            foreach(Node n in Assembly.Nodes)
            {
                int ind = Assembly.Nodes.IndexOf(n);
                //指定したノードに属するエレメントの一覧
                List<Element1D> detailElems = Assembly.NodeMap.Where(p => p.Value.Contains(ind)).ToList().ConvertAll(p => p.Key);
                Details.Add(new Detail(n));
                Details.Last().SetElements(detailElems, Priority);
            }
        }
    }

    //納まりのクラス
    public class Detail
    {
        public Node Node { get; private set; }
        //public List<Element1D> Elements { get; private set; }
        public Dictionary<Element1D,int> ElementsPriorityMap { get; private set; }
        //private int crossElementNum = 0;
        private DetailType type;

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
                type = DetailType.XType;
                if (!SortElementsByPriority(ref crossElements, _priority))
                {
                    return false;
                }
            }
            else if(crossElements.Count == 1)
            {
                type = DetailType.TType;
            }
            else
            {
                type = DetailType.LType;
            }
            if (cornerElements.Count >= 2)
            {
                if(!SortElementsByPriority(ref cornerElements, _priority))
                {
                    return false;
                }
            }
            //List<Element1D> elements = crossElements.
            return true;
        }

        private bool SortElementsByPriority(ref List<Element1D> _elements, List<string> _priority)
        {
            //優先度による並び替えが必要ない場合
            if (_elements.Count <= 1 || _elements.ConvertAll(e => e.Tag).Distinct().Count() <= 1)
            {
                return true;
            }
            //優先づけすべきElementが、全てpriority入力されているか
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

        //private int CountPriorityHigherElemenrt(Element1D _element, )

        //private void InsertElementPriorityMap(Element1D _element,int )

        private bool IsNodeEndPointAtElement(Element1D _element)
        {
            if (_element.BaseCurve.PointAtStart == Node.Point || _element.BaseCurve.PointAtEnd == Node.Point)
            {
                return true;
            }
            return false;
        }
    }
}
