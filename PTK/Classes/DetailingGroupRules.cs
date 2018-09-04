using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTK
{
    public class DetailingGroupRules
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

        public void MergeDescriptions(List<DetailingGroupRules> _descriptions)
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


    public class ElemLength : DetailingGroupRules
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

    public class ElemAmount : DetailingGroupRules
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

    public class NodeHitRegion : DetailingGroupRules
    {


        private List<Curve> polycurves;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////Properties
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public List<Curve> Polycurves { get { return polycurves; } set { } }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////Constructors
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public NodeHitRegion(List<Curve> _polyCurves)
        {
            polycurves = new List<Curve>();
            polycurves = _polyCurves;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////Methods
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public bool check(Detail _detail)
        //This method checks if the node point is in the regions or not.
        {
            double tolerance = 0.01;
            Plane Curveplane = new Plane();

            for (int i = 0; i < polycurves.Count; i++)
            {
                Curve polycurve = polycurves[i];
                if (polycurve.TryGetPlane(out Curveplane))
                {

                    PointContainment relationship = polycurve.Contains(_detail.Nodes[0].Pt3d, Curveplane, tolerance);
                    if (relationship == PointContainment.Inside || relationship == PointContainment.Coincident)
                    {
                        if (Curveplane.DistanceTo(_detail.Nodes[0].Pt3d) < tolerance)
                        {
                            return true;
                        }

                    }

                }
            }

            return false;


        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////constructors
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



    }


    public class NodeClosestPoint : DetailingGroupRules
    {


        private List<Point3d> points;
        private List<double> shortestDistance;
        private double tolerance;
        private bool OnlyOne; //Bool that tells if only one node pr point is allowed



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////Properties
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////





        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////Constructors
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        public NodeClosestPoint(List<Point3d> _points, double _tolerance, bool _OnlyOne)
        {
            points = new List<Point3d>();
            points = _points;
            tolerance = _tolerance;
            OnlyOne = _OnlyOne;

            shortestDistance = Enumerable.Repeat(1000000d, points.Count).ToList();  //Creating a list that has the same count as the point list. Used to check the closest point

        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////Methods
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public bool check(Detail _detail)
        //This method checks if the node point is in the regions or not.
        {
            Point3d nodePoint = _detail.Nodes[0].Pt3d;

            for (int i = 0; i < points.Count; i++)
            {

                Point3d samplePoint = points[i];


                if (!OnlyOne)
                {
                    if (nodePoint.DistanceTo(samplePoint) < tolerance)
                    {
                        return true;
                    }
                }

                if (OnlyOne)
                {
                    double Distance = samplePoint.DistanceTo(nodePoint);

                    if (Distance < shortestDistance[i])
                    {
                        shortestDistance[i] = Distance;
                        return true;
                    }


                }


            }

            return false;




        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////constructors
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



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
