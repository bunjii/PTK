using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTK
{
    public delegate bool MethodDelegate(Detail _detail);

    public class DetailingGroup
    {
        #region fields
        private string name;
        private Descriptions trueDescriptions;
        private Descriptions falseDescriptions;
        private List<Detail> details; // A detail is a node and its elems or a elem and its nodes. A detail should then be a list of elemID and a list of NodeID
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
        public void assignDetails(List<Node> _nodes, List<PTK_Element> _elems)
        {
            Detail tempdetail = new Detail();
            details = new List<Detail>();
            //Running through node-based details first
            //Define a detail
            for (int i = 0; i < _nodes.Count; i++)
            {
                List<PTK_Element> tempElems = new List<PTK_Element>();
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
