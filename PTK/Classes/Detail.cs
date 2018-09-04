using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTK
{
    public class Detail
    {
        public Node Node { get; private set; }
        //public List<Element1D> Elements { get; private set; }
        public Dictionary<Element1D, int> ElementsPriorityMap { get; private set; }
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
            // test 

            if (crossElements.Count >= 2)
            {
                Type = DetailType.XType;
                if (!SortElementsByPriority(ref crossElements, _priority))
                {
                    return false;
                }
            }
            else if (crossElements.Count == 1)
            {
                Type = DetailType.TType;
            }
            else
            {
                Type = DetailType.LType;
            }
            if (cornerElements.Count >= 2)
            {
                if (!SortElementsByPriority(ref cornerElements, _priority))
                {
                    return false;
                }
            }

            string preTag = "";
            int priorityIndex = 0;
            foreach (Element1D e in crossElements)
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
            if (Node.Point.DistanceTo(_element.BaseCurve.PointAtStart) <= CommonProps.tolerances ||
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

}
