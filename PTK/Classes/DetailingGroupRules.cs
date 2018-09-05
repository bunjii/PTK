using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;

namespace PTK.Rules

{
    


    public class Rule
    {
        public CheckGroupDelegate checkdelegate;

        public Rule(CheckGroupDelegate _checkdelegate)
        {
            checkdelegate = _checkdelegate;
        }


        
    }

   

    public class ElementLength
    {
        #region fields
        private double minLength = 0;
        private double maxLength = 100000000000;


        #endregion

        #region constructors
        public ElementLength(double _min, double _max)
        {

            minLength = _min;
            maxLength = _max;

        }


        #endregion
        #region properties

        #endregion
        #region methods

        public bool check(Detail _detail)  //Checking element length
        {
            Detail detail = _detail;
            Node node = detail.Node;
            List<Element1D> elements = detail.Elements;// ElementsPriorityMap.Keys.ToList();


            bool valid = false;
            foreach (Element1D element in elements)
            {
                double curvelength = element.BaseCurve.GetLength();
                if (minLength < curvelength && curvelength < maxLength == true)
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
    public class ElementAmount 
    {
        #region fields
        private int minAmount = 0;
        private int maxAmount = 1000;


        #endregion

        #region constructors
        public ElementAmount(int _minAmount, int _maxAmount)
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
            Detail detail = _detail;
            Node node = detail.Node;
            List<Element1D> elements = detail.Elements;// ElementsPriorityMap.Keys.ToList();

            


            bool valid = false;
            foreach (Element1D element in elements)

            {
                if (minAmount <= elements.Count && elements.Count <= maxAmount == true)
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

    public class NodeHitRegion 
    {


        private List<Curve> polycurves;
        public List<Curve> Polycurves { get { return polycurves; } set { } }


        public NodeHitRegion(List<Curve> _polyCurves)
        {
            polycurves = new List<Curve>();
            polycurves = _polyCurves;
        }

    
        public bool check(Detail _detail)
        //This method checks if the node point is in the regions or not.
        {
            Detail detail = _detail;
            Node node = detail.Node;
            List<Element1D> elements = detail.Elements;// ElementsPriorityMap.Keys.ToList();



            double tolerance = CommonProps.tolerances; ;
            Plane Curveplane = new Plane();

            for (int i = 0; i < polycurves.Count; i++)
            {
                Curve polycurve = polycurves[i];
                if (polycurve.TryGetPlane(out Curveplane))
                {

                    PointContainment relationship = polycurve.Contains(node.Point, Curveplane, tolerance);
                    if (relationship == PointContainment.Inside || relationship == PointContainment.Coincident)
                    {
                        if (Curveplane.DistanceTo(node.Point) < tolerance)
                        {
                            return true;
                        }

                    }

                }
            }

            return false;


        }



    }

    /*
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
    */






    #region fields
    #endregion
    #region constructors
    #endregion
    #region properties
    #endregion
    #region methods
    #endregion

}
