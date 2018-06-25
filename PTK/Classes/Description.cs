using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTK
{

    public class Descriptions
    {
        #region fields
        private ElemLength elemLength;



        #endregion
        #region constructors



        #endregion
        #region properties
        public ElemLength ElemLength { get { return elemLength; } set { elemLength = value; } }




        #endregion
        #region methods

        public void MergeDescriptions(List<Descriptions> _descriptions)
        {
            for (int i = 0; i < _descriptions.Count; i++)
            {
                if (elemLength == null && _descriptions[i].elemLength != null) { _descriptions[i].elemLength = elemLength; }
            }
        }

        public bool CheckIfValid(Detail Detail)
        {

            if (elemLength.check(Detail) == false) { return false; };

            return true;

        }



        #endregion

    }


    public class ElemLength : Descriptions
    {
        #region fields
        private double minLength = 0;
        private double maxLength = 100000000000;


        #endregion

        #region constructors
        public ElemLength(double _min, double _max)
        {

            minLength = _min;
            maxLength = _max;

        }


        #endregion
        #region properties

        #endregion
        #region methods

        public bool check(Detail _detail)
        {
            List<Node> _nodes = _detail.Nodes;
            List<Element> _elems = _detail.Elems;


            bool valid = false;
            List<int> elemIds = _nodes[0].ElemIds;
            for (int i = 0; i < elemIds.Count; i++)
            {
                int elemId = elemIds[i];
                int elemIndex = _elems.FindIndex(x => x.Id == elemId);
                if (minLength < _elems[elemIndex].GetLength() && _elems[elemIndex].GetLength() < maxLength == true)
                {
                    valid = true;
                }
                else
                {
                    valid = false;
                    break;
                }

            }



            return valid;
        }
        #endregion

    }

    public class ElemAmount : Descriptions
    {
        #region fields
        private int minAmount = 0;
        private int maxAmount = 1000;


        #endregion

        #region constructors
        public ElemAmount(int _minAmount, int _maxAmount)
        {

            minAmount = _minAmount;
            maxAmount = _maxAmount;

        }


        #endregion
        #region properties

        #endregion
        #region methods

        public bool check(Detail _detail)
        {
            List<Node> _nodes = _detail.Nodes;
            List<Element> _elems = _detail.Elems;


            bool valid = false;
            List<int> elemIds = _nodes[0].ElemIds;
            for (int i = 0; i < elemIds.Count; i++)
            {
                int elemId = elemIds[i];

                if (minAmount <= _elems.Count && _elems.Count <= maxAmount == true)
                {
                    valid = true;
                }
                else
                {
                    valid = false;
                    break;
                }

            }



            return valid;
        }
        #endregion

    }

    public class Rule
    {
        private List<MethodDelegate> rules;

        public Rule(List<MethodDelegate> _rules)
        {
            rules = _rules;
        }


        public List<MethodDelegate> Rules { get { return rules; } }


    }

}
