using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace PTK
{
    public class SubElementBTL       //THis is the BTL element. There can be more than one BTL element in a main element (Element.cl)
    {

        private PartType btlPart;
        private List<BTLprocess> btlProcesses;
        private List<Refside> refSides;
        private int uid;
        private Plane yzPlane;
        private double height;
        private double width;
        private double length;
        private Plane btlPlane;
        private List<Point3d> startPoints;
        private List<Point3d> endPoints;
        private List<Point3d> cornerPoints;




        #region constructors
        public SubElementBTL(Plane _yzPlane, CrossSection _section, double _length)
        {
            PartType tempPart = new PartType();
            CoordinateSystemType CoordinateSystem = new CoordinateSystemType();   //Initializing the coordinate system of a part
            PointType Point = new PointType();  //the point of a part
            btlProcesses = new List<BTLprocess>();
            yzPlane = _yzPlane;
            height = 777;//_section;
            width = 777;//_section.Width;
            length = _length;


            //each btl operation is using one of four refsides
            GenerateRefSides();

            CoordinateSystem.XVector.X = btlPlane.XAxis.X;
            CoordinateSystem.XVector.Y = btlPlane.XAxis.Y;
            CoordinateSystem.XVector.Z = btlPlane.XAxis.Z;
            CoordinateSystem.YVector.X = btlPlane.YAxis.X;
            CoordinateSystem.YVector.Y = btlPlane.YAxis.Y;
            CoordinateSystem.YVector.Z = btlPlane.YAxis.Z;
            CoordinateSystem.ReferencePoint.X = btlPlane.OriginX;
            CoordinateSystem.ReferencePoint.Y = btlPlane.OriginY;
            CoordinateSystem.ReferencePoint.Z = btlPlane.OriginZ;

            ReferenceType Reference = new ReferenceType();
            Reference.Position = CoordinateSystem;


            tempPart.Transformations.Transformation.Add(Reference);

            tempPart.Length = _length;
            tempPart.Width = 777; //_section.Width;
            tempPart.Height = 777;// _section.Height;

            btlPart = tempPart;


        }

        #endregion

        #region properties
        public PartType BTLPart { get { return btlPart; } }
        public List<Refside> RefSides { get { return refSides; } }
        public List<Point3d> StartPoints { get { return startPoints; } }
        public List<Point3d> EndPoints { get { return endPoints; } }
        public List<Point3d> CornerPoints { get { return cornerPoints; } }
        public List<BTLprocess> BTLProcesses { get { return btlProcesses; } set { btlProcesses = value; } }
        #endregion

        #region methods


        private void GenerateRefSides()
        {
            //Move half height, half width
            refSides = new List<Refside>();

            yzPlane.Translate(yzPlane.XAxis * width / 2 + (yzPlane.YAxis * -height / 2));

            //Making same plane as in manual
            btlPlane = new Plane(yzPlane.Origin, yzPlane.ZAxis, yzPlane.YAxis);


            Plane refSide1 = new Plane(btlPlane.Origin, btlPlane.XAxis, btlPlane.ZAxis);
            Plane refSide2 = btlPlane;
            refSide2.Translate(btlPlane.YAxis * height);
            refSide2 = new Plane(refSide2.Origin, btlPlane.XAxis, -btlPlane.YAxis);
            Plane refSide3 = refSide2;
            refSide3.Translate(btlPlane.ZAxis * width);
            refSide3 = new Plane(refSide3.Origin, btlPlane.XAxis, -btlPlane.ZAxis);
            Plane refSide4 = btlPlane;
            refSide4.Translate(btlPlane.ZAxis * width);
            refSide4 = new Plane(refSide4.Origin, btlPlane.XAxis, btlPlane.YAxis);

            refSides.Add(new Refside(1, refSide1, length));
            refSides.Add(new Refside(2, refSide2, length));
            refSides.Add(new Refside(3, refSide3, length));
            refSides.Add(new Refside(4, refSide4, length));


            startPoints = new List<Point3d>();
            endPoints = new List<Point3d>();
            cornerPoints = new List<Point3d>();

            foreach (Refside side in refSides)
            {
                startPoints.Add(side.RefPoint);
                Plane tempPlane = new Plane(side.RefPoint, yzPlane.ZAxis);

                tempPlane.Translate(tempPlane.ZAxis * length);
                endPoints.Add(tempPlane.Origin);
            }

            //Cornerpoints are used to define cuttingboxes
            cornerPoints = new List<Point3d>();
            cornerPoints.AddRange(startPoints);
            cornerPoints.AddRange(endPoints);

        }
        static public Plane AlignInputPlane(Line _refEdge, Plane _refPlane, Plane _cutPlane, out OrientationType orientationtype)
        {


            double lineparameter = 0;

            if (Rhino.Geometry.Intersect.Intersection.LinePlane(_refEdge, _cutPlane, out lineparameter))
            {

            }


            Point3d intersectPoint = _refEdge.PointAt(lineparameter);
            Line intersectionLine = new Line();
            Rhino.Geometry.Intersect.Intersection.PlanePlane(_refPlane, _cutPlane, out intersectionLine);
            _cutPlane.Origin = intersectPoint;

            Line directionLine = FlipLine(_refPlane.YAxis, intersectionLine);




            double angle = Vector3d.VectorAngle(_cutPlane.XAxis, directionLine.Direction, _cutPlane);

            _cutPlane.Rotate(angle, _cutPlane.ZAxis, _cutPlane.Origin);

            orientationtype = OrientationType.start;
            if (Vector3d.VectorAngle(_refPlane.XAxis, _cutPlane.ZAxis) < Math.PI / 2)
            {
                orientationtype = OrientationType.end;
            }




            return _cutPlane;
        }
        public static Line FlipLine(Vector3d _guide, Line _line)
        {
            List<double> angle = new List<double>();
            List<Line> intersectionlines = new List<Line>();
            Line flipline = _line;
            flipline.Flip();
            angle.Add(Vector3d.VectorAngle(_guide, _line.Direction));
            intersectionlines.Add(_line);
            angle.Add(Vector3d.VectorAngle(_guide, flipline.Direction));
            intersectionlines.Add(flipline);

            //Returning the line with the smallest angle: Aligning the line to face the same direction as the vector
            if (angle[0] < angle[1])
            {
                return intersectionlines[0];
            }
            else
            {
                return intersectionlines[1];
            }



        }
        public static List<Point3d> GetValidVoidPoints(Plane _CutPlane, List<Point3d> TestPoints)
        {


            List<Point3d> Voidpoints = new List<Point3d>();

            foreach (Point3d point in TestPoints)
            {
                Point3d localaxispoint;
                _CutPlane.RemapToPlaneSpace(point, out localaxispoint);
                if (localaxispoint.Z > -.00001)
                {
                    Voidpoints.Add(point);
                }

            }
            return Voidpoints;


        }

        public static List<Point3d> GetCutPoints(Plane _CutPlane, List<Refside> _refsides)
        {

            List<Point3d> CutPoints = new List<Point3d>();
            foreach (Refside side in _refsides)
            {
                double tempDouble = 0;
                if (Rhino.Geometry.Intersect.Intersection.LinePlane(side.RefEdge, _CutPlane, out tempDouble)) ;
                if (0 < tempDouble && tempDouble < side.RefEdge.Length)
                {
                    CutPoints.Add(side.RefEdge.PointAt(tempDouble));
                }
            }
            return CutPoints;





        }

        public static Refside GetRefSideFromPlane(List<Refside> _refSides, Plane _CutPlane, out List<Point3d> _CutPoints)
        {
            int i = 0;
            double smallestDistance = 9999999;
            int sideIndex = 0;

            List<Point3d> CutPoints = new List<Point3d>();
            foreach (Refside side in _refSides)
            {

                double tempDouble = 0;
                if (Rhino.Geometry.Intersect.Intersection.LinePlane(side.RefEdge, _CutPlane, out tempDouble)) ;
                if (0 < tempDouble && tempDouble < side.RefEdge.Length)
                {

                    Point3d cutPoint = side.RefEdge.PointAt(tempDouble);
                    double distance = side.RefPoint.DistanceTo(cutPoint);
                    CutPoints.Add(cutPoint);


                    if (distance < smallestDistance)
                    {

                        smallestDistance = distance;
                        sideIndex = i;
                    }

                }
                i++;
            }

            _CutPoints = CutPoints;
            return _refSides[sideIndex];



            //Checking each intersection, determine which is smallest, return plane, line and origo





        }





        #endregion


    }


    public class Refside
    {
        uint refSideID;
        Plane refPlane;
        Line refEdge;
        Point3d refPoint;


        public Refside(uint _refsideID, Plane _refPlane, double _length)
        {
            refSideID = _refsideID;
            refPlane = _refPlane;
            refEdge = new Line(refPlane.Origin, refPlane.XAxis, _length);
            refPoint = refPlane.Origin;
        }

        ////////////
        ///Properties
        ////////////
        public uint RefSideID { get { return refSideID; } }
        public Plane RefPlane { get { return refPlane; } }
        public Line RefEdge { get { return refEdge; } }
        public Point3d RefPoint { get { return refPoint; } }




    }


    public class BTLprocess                                     //Contains a BTL-processingtype and a voidgeometry (to boolean )
    {
        ProcessingType process;
        Brep voidgeometry;
        int elemId;
        int elementHashCode;



        public BTLprocess(ProcessingType _process, Brep _voidGeometry, int _elemId)
        {

            voidgeometry = _voidGeometry;
            process = _process;
            elemId = _elemId;
            Element1D elem = new Element1D();
            elem.GetHashCode();

        }


        public BTLprocess()
        {
        }







        public ProcessingType Process { get { return process; } }
        public Brep Voidgeometry { get { return voidgeometry; } }
        public int ElemId { get { return elemId; } }


        ////////////////////////////////
        ///METHODS TO MAKE DIFFERENT BTL  OPERATIONS!
        ///ALL METHODS INPUTS AN ELEMENT
        ///BASED ON OPERATIONS, ADDITIONAL INPUT IS ADDED
        ///ALL METHODS OUT'S A BTLPROCSESS
        ////////////////////////////////

        /*
        static public BTLprocess Cut(PTK_Element _Element, Plane _Plane)
        {
            PTK_Element element = _Element;
            Plane cutPlane = _Plane;



            if (element != null)
            {
                //In the PTK1.0 here will be a loop to go through multiple btl-elements
                List<Refside> Refsides = element.SubElementBTL[0].RefSides;

                List<Point3d> cutpoints = new List<Point3d>();


                //Calculating the refside that has the cutpoint closest to the refpoint
                Refside RefSide = SubElementBTL.GetRefSideFromPlane(Refsides, cutPlane, out cutpoints);

                uint RefSideId = RefSide.RefSideID;
                Plane RefPlane = RefSide.RefPlane;
                Line RefEdge = RefSide.RefEdge;
                Point3d RefPoint = RefSide.RefPoint;

                Point3d intersectPoint = new Point3d();

                double lineparameter = 0;

                if (Rhino.Geometry.Intersect.Intersection.LinePlane(RefEdge, cutPlane, out lineparameter))
                    intersectPoint = RefEdge.PointAt(lineparameter);




                //intersectPoint = intersectionevent.PointA;
                Line directionLine = new Line();
                if (Rhino.Geometry.Intersect.Intersection.PlanePlane(RefPlane, cutPlane, out directionLine)) ;


                OrientationType orientation;


                //AlignInputPlane aligns the x-axis of the plane to the surface of the ref-side
                cutPlane = SubElementBTL.AlignInputPlane(RefEdge, RefPlane, cutPlane, out orientation);


                Plane inclinationplane = new Plane(cutPlane.Origin, cutPlane.YAxis, cutPlane.ZAxis);





                Vector3d RefVector = RefEdge.Direction;
                Vector3d Cutvector = cutPlane.YAxis;

                List<Point3d> voidpoints = new List<Point3d>();
                voidpoints.AddRange(SubElementBTL.GetCutPoints(cutPlane, Refsides));  //adding the four points where the refEdge and the cutplane intersects
                voidpoints.AddRange(element.SubElementBTL[0].CornerPoints);


                if (orientation == OrientationType.end)
                {
                    //Adding endpoints if orientationtype is end
                    RefVector.Reverse();
                    Cutvector.Reverse(); //Correct
                }


                voidpoints = SubElementBTL.GetValidVoidPoints(cutPlane, voidpoints);




                //Calculating negative distance by remaping to planespace
                double startX = 0;
                Point3d localaxispoint;
                Plane checkplane = new Plane(RefEdge.From, RefEdge.Direction);
                checkplane.RemapToPlaneSpace(intersectPoint, out localaxispoint);




                //Creating voidbox
                Box box = new Box(cutPlane, voidpoints);


                //Creating BTL processing
                JackRafterCutType JackRafterCut = new JackRafterCutType();
                JackRafterCut.Orientation = orientation;
                JackRafterCut.ReferencePlaneID = RefSideId;
                JackRafterCut.Process = BooleanType.yes;
                JackRafterCut.StartX = localaxispoint.Z;
                JackRafterCut.StartY = 0.0;
                JackRafterCut.StartDepth = 0.0;
                JackRafterCut.Angle = Vector3d.VectorAngle(RefVector, cutPlane.XAxis);
                JackRafterCut.Angle = Convert.ToDouble(Rhino.RhinoMath.ToDegrees(JackRafterCut.Angle));
                JackRafterCut.Inclination = Vector3d.VectorAngle(RefVector, Cutvector, new Plane(directionLine.From, directionLine.Direction));
                JackRafterCut.Inclination = Convert.ToDouble(Rhino.RhinoMath.ToDegrees(JackRafterCut.Inclination));
                JackRafterCut.StartDepth = 0.0;
                JackRafterCut.Name = Convert.ToString(element.Id);    //Name is used as container for elemId identifier


                return new BTLprocess(JackRafterCut, Brep.CreateFromBox(box), element.Id);


            }
            //if it does not work
            return new BTLprocess();




        }
        */

        static public bool Drill(GH_Element1D _Element, /*SOMEDATA HERE, */ out BTLprocess Drill)
        {
            Drill = new BTLprocess();
            return false;
        }


        /*
        public class BTLOperations
        {
            private RefSides Refsides;
            private Plane RefSide;
            private Line refEdge;
            private Point3d intersectPoint;


        }
        */

    }
}











