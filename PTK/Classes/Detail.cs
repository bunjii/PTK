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

        private List<Element> elems;
        private List<int> elemsIds;


        #endregion
        #region constructors
        public Detail(List<Node> _nodes, List<Element> _elems)
        {
            nodes = new List<Node>();
            nodeIds = new List<int>();
            elems = new List<Element>();
            elemsIds = new List<int>();


            nodes = _nodes;
            elems = _elems;
            elemsIds = new List<int>();
            nodeIds = new List<int>();
            foreach (Node node in _nodes)
            {
                nodeIds.Add(node.Id);
            }
            foreach (Element elem in _elems)
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
        public List<Element> Elems { get { return elems; } }
        public List<int> NodeIds { get { return nodeIds; } }
        public List<int> ElemsIds { get { return elemsIds; } }

        #endregion
        #region methods
        #endregion
    }

    public delegate bool MethodDelegate(Detail _detail);

    public class DetailingGroup
    {
        #region fields
        private string name;
        private Descriptions trueDescriptions;
        private Descriptions falseDescriptions;
        private List<Detail> details;
        private List<MethodDelegate> mergeVerifierTrue;  //Methods that verify if a detail belong to this detailing group. If all methods return true, then detail belongs to group


        #endregion
        #region constructors
        public DetailingGroup(string _name, List<MethodDelegate> _mergeVerifierTrue)
        {
            name = _name;
            mergeVerifierTrue = _mergeVerifierTrue;
            details = new List<Detail>();

        }
        #endregion
        #region properties
        public List<Detail> Details { get { return details; } }
        public string Name { get { return name; } }

        #endregion
        #region methods
        public void assignDetails(List<Node> _nodes, List<Element> _elems)
        {
            Detail tempdetail = new Detail();
            details = new List<Detail>();
            //Running through node-based details first
            //Define a detail
            for (int i = 0; i < _nodes.Count; i++)
            {
                List<Element> tempElems = new List<Element>();
                List<Node> tempNode = new List<Node>();
                tempNode.Add(_nodes[i]);
                for (int j = 0; j < _nodes[i].ElemIds.Count; j++)
                {
                    tempElems.Add(_elems.Find(t => t.Id == _nodes[i].ElemIds[j]));
                }
                tempdetail = new Detail(tempNode, tempElems);

                bool valid = true;
                if (mergeVerifierTrue.Count == 0) { valid = false; }
                foreach (MethodDelegate merge in mergeVerifierTrue)  //runs through all the verifier rules. If false
                {
                    if (!merge(tempdetail))
                    {
                        valid = false;
                        break;
                    }
                }
                if (valid)
                {
                    details.Add(tempdetail);
                }

            }







            //run through true descriptions
            //run through false descriptions



        }



        #endregion

    }
}
