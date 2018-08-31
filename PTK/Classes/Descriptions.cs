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
            List<PTK_Element> _elems = _detail.Elems;


            bool valid = false;
            List<int> elemIds = _nodes[0].ElemIds;
            for (int i = 0; i < elemIds.Count; i++)
            {
                int elemId = elemIds[i];
                int elemIndex = _elems.FindIndex(x => x.Id == elemId);
                if (minLength < _elems[elemIndex].Length && _elems[elemIndex].Length < maxLength == true)
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
            List<PTK_Element> _elems = _detail.Elems;

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

    public class ElementTag : Descriptions
    {
        #region fields

        private List<string> tagsAre = new List<string>();
        private int mode = 0;

        #endregion
        #region constructors

        public ElementTag(List<string> _tagsAre, int _mode = 0)
        {
            tagsAre = _tagsAre;
            mode = _mode;
        }
        
        #endregion
        #region properties

        #endregion
        #region methods

        public bool check(Detail _detail)
        {
            List<Node> _nodes = _detail.Nodes;
            List<PTK_Element> _elems = _detail.Elems;


            List<String> detailTags = new List<string>();

            List<String> tagsAreStrict = tagsAre;
            List<String> tagsAreDistinct = tagsAre.Distinct().ToList();

            tagsAreStrict.Sort();
            tagsAreDistinct.Sort();

            bool valid = false;

            List<int> elemIds = _nodes[0].ElemIds;

            if (mode >= 4) //mode verifier
                mode = 0;

            if (mode == 0)  // Mode 0 - One of - The detail must contain one of the inputted tags
            {
                for (int j = 0; j < elemIds.Count; j++)
                {
                    int elemId = elemIds[j];
                    int elemIndex = _elems.FindIndex(x => x.Id == elemId);

                    for (int i = 0; i < tagsAre.Count; i++)
                    {
                        if (_elems[elemIndex].Tag.Equals(tagsAre[i]))
                        {
                            valid = true;
                            break;
                        }
                        
                    }
                }
            }

            if (mode == 1) // Mode 1 - At least -  The detail must contain all the inputted tags, but can also contain other tags
            {
                for (int j = 0; j < elemIds.Count; j++)
                {
                    foreach (PTK_Element element in _elems)
                    {
                        detailTags.Add(element.Tag);
                    }

                    if (tagsAre.Count == 1)
                    {
                        if (detailTags.Contains(tagsAre[0]))
                            valid = true;
                    }
                    else
                    {
                     List<string> detailTagsDistinct = detailTags.Distinct().ToList();

                    if (detailTagsDistinct.Except(tagsAreDistinct).Count() == 0 && tagsAreDistinct.Except(detailTagsDistinct).Count() == 0)
                        valid = true;
                    }
                    

                }
            }


            if (mode == 2) // Mode 2 - Distinct - The detail must contain all the inputted tags and no other tags
            {
               for (int j = 0; j < elemIds.Count; j++)
                {

                    foreach (PTK_Element element in _elems)
                    {
                        detailTags.Add(element.Tag);
                    }
                    
                    List<string> detailTagsDistinct = detailTags.Distinct().ToList();
                    detailTagsDistinct.Sort();
                    if (detailTagsDistinct.SequenceEqual(tagsAreDistinct))
                    {
                        valid = true;
                    }
                }
            }

            if (mode == 3) // Mode 3 - Strict - The detai must contain all the inputted tags and the exact amount 

            {
                for (int j = 0; j < elemIds.Count; j++)
                {
                    foreach (PTK_Element element in _elems)
                    {
                        detailTags.Add(element.Tag);
                    }

                    detailTags.Sort();
                    if (detailTags.SequenceEqual(tagsAreStrict))
                    {
                        valid = true;
                    }
                }
            }

            return valid;
        }

            

        

            


        #endregion
    }
    public class Angle : Descriptions
    {
        #region fields

        private List<string> tagsAre = new List<string>();
        private int mode = 0;

        #endregion
        #region constructors

        public Angle(int _angle = 0)
        {
            int Angle = _angle;
        }

        #endregion
        #region properties

        #endregion
        #region methods

        public bool check(Detail _detail)
        {
            return true;
        }








        #endregion
    }



    #region fields
    #endregion
    #region constructors
    #endregion
    #region properties
    #endregion
    #region methods
    #endregion

}
