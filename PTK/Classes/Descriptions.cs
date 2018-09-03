using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using feb;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

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



    public class ElementAngle : Descriptions
    {
        #region fields

        private int minimumAngle = 0;
        private int maximumAngle = 360;
        

        #endregion
        #region constructors

        public ElementAngle(int _minimumAngle = 0, int _maximumAngle = 360)
        {
            minimumAngle = _minimumAngle;
            maximumAngle = _maximumAngle;

        }

        #endregion
        #region properties

        #endregion
        #region methods



        public bool check(Detail _detail)
        {
            List<Node> _nodes = _detail.Nodes;
            List<PTK_Element> _elems = _detail.Elems;
            List<Line> elementsFromNode = new List<Line>();
            List<Vector3d> elementVectors = new List<Vector3d>();

            for (int i = 0; i < _elems.Count; i++) //Creates a unitized vector or each element, and ensures that the vectors start in the node
            {
            Line elementLine = new Line(_nodes[0].Pt3d,_elems[i].Crv.PointAt(0.5));
            Vector3d elementVector = elementLine.UnitTangent;
            Line curveFlipped = new Line(_nodes[0].Pt3d,elementVector,_elems[i].Length);
            
            elementVectors.Add(elementVector);
            elementsFromNode.Add(curveFlipped);
            
            }
            
            List<Double> elementAngles = new List<double>(); //Creates a plane in the node that has normal like the average of all elementvectors

            double x = 0f;
            double y = 0f;
            double z = 0f;
            
            for (int i = 0; i < elementVectors.Count; i++)
            {
                x += elementVectors[i].X;
                y += elementVectors[i].Y;
                z += elementVectors[i].Z;
            }
            Vector3d nodeVector = new Vector3d(x/elementVectors.Count,y/elementVectors.Count,z/elementVectors.Count);
            Plane nodePlane = new Plane(_nodes[0].Pt3d, nodeVector);

            //har konstruert eit plan basert på snittvektoren til alle vektorene. Neste steg er å sortere basert på vinkel.

            Vector3d nX = nodePlane.XAxis;
            Vector3d nY = nodePlane.YAxis;
            Vector3d nZ = nodePlane.ZAxis;

            List<double> angles = new List<double>();

            double angle = 0;
            bool valid = false;

            Vector3d nodePlaneVector = (Vector3d.Add(nX, nZ));

            for (int i = 0; i < elementVectors.Count; i++)
            {
                angle = Vector3d.VectorAngle(nodePlaneVector, elementVectors[i]);
                angles.Add(angle * 180 / Math.PI);
            }

            //List<KeyValuePair<> >
            //angles.OrderByDescending().ToList();
            //angle = (int) _angle;

            //if (minimumAngle <= angle && angle <= maximumAngle)
            //    valid = true;


            return valid;

            //Circle circle = new Circle(nodePlane,1);
            //List<double> circleInt = new List<double>();

            //for (int i = 0; i < elementsFromNode.Count; i++)
            //{
            //    double t1, t2;
            //    Point3d p1, p2;
            //    LineCircleIntersection(elementsFromNode[i], circle, out t1, out p1, out t2, out p2);
            //    circleInt.Add(t1);
            // }

            //circleInt.Sort(x => x);

           
           
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
