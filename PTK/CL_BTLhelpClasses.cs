using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using Rhino.Geometry;


namespace PTK
{
    public class BTLref
    {

        #region fields
        private Plane btlplane;
        private Plane refSide1;
        private Plane refSide2;
        private Plane refSide3;
        private Plane refSide4;

        private Line refEdge1;
        private Line refEdge2;
        private Line refEdge3;
        private Line refEdge4;
        private List<Point3d> startPoints;
        private List<Point3d> endPoints;
        



        #endregion
        #region constructors

        public BTLref(Plane yzPlane, double _height, double _width, double _length )
        {
            //Move half height, half width

            
            yzPlane.Translate(yzPlane.XAxis * _width / 2 + (yzPlane.YAxis * -_height/2));

            //Making same plane as in manual
            btlplane = new Plane(yzPlane.Origin, yzPlane.ZAxis, yzPlane.YAxis);

            refSide1 = new Plane(btlplane.Origin, btlplane.XAxis, btlplane.ZAxis);
            refSide2 = btlplane;
            refSide2.Translate(btlplane.YAxis * _height);
            refSide2 = new Plane(refSide2.Origin, btlplane.XAxis, -btlplane.YAxis);
            refSide3 = refSide2;
            refSide3.Translate(btlplane.ZAxis * _width);
            refSide3 = new Plane(refSide3.Origin, btlplane.XAxis, -btlplane.ZAxis);
            refSide4 = btlplane;
            refSide4.Translate(btlplane.ZAxis * _width);
            refSide4 = new Plane(refSide4.Origin, btlplane.XAxis, btlplane.YAxis);

            refEdge1 = new Line(refSide1.Origin, refSide1.XAxis, 5000);
            refEdge2 = new Line(refSide2.Origin, refSide2.XAxis, 5000);
            refEdge3 = new Line(refSide3.Origin, refSide3.XAxis, 5000);
            refEdge4 = new Line(refSide4.Origin, refSide4.XAxis, 5000);


            Rectangle3d rec = new Rectangle3d(yzPlane, -_width, _height);
            Polyline poly = rec.ToPolyline();
            startPoints = poly.ToList();

            Plane endplane = yzPlane;
            endplane.Translate(endplane.ZAxis * _length);
            Rectangle3d rec2 = new Rectangle3d(endplane, -_width, _height);
            poly = rec2.ToPolyline();
            endPoints = poly.ToList();
            


            






        }

        #endregion
        #region properties
        public Plane BTLplane { get { return btlplane; } }
        public Plane RefSide1 { get { return refSide1; } }
        public Plane RefSide2 { get { return refSide2; } }
        public Plane RefSide3 { get { return refSide3; } }
        public Plane RefSide4 { get { return refSide4; } }

        public Line RefEdge1 { get { return refEdge1; } }
        public Line RefEdge2 { get { return refEdge2; } }
        public Line RefEdge3 { get { return refEdge3; } }
        public Line RefEdge4 { get { return refEdge4; } }
        public List<Point3d> StartPoints { get { return startPoints; } }
        public List<Point3d> Endpoints { get { return endPoints; } }
        #endregion
        #region methods




        static public Plane AlignInputPlane(Line _refEdge, Plane _refPlane, Plane _cutPlane, out OrientationType orientationtype)
        {


            Point3d intersectPoint = Rhino.Geometry.Intersect.Intersection.CurvePlane(_refEdge.ToNurbsCurve(), _cutPlane, 0.01)[0].PointA;
            Line intersectionLine = new Line();
            Rhino.Geometry.Intersect.Intersection.PlanePlane(_refPlane, _cutPlane, out intersectionLine);
            _cutPlane.Origin = intersectPoint;

            Line directionLine = FlipLine(_refPlane.YAxis, intersectionLine);

            


            double angle = Vector3d.VectorAngle(_cutPlane.XAxis, directionLine.Direction,_cutPlane);

            _cutPlane.Rotate(angle, _cutPlane.ZAxis, _cutPlane.Origin);

            orientationtype = OrientationType.start;
            if(Vector3d.VectorAngle(_refPlane.XAxis,_cutPlane.ZAxis)< Math.PI/2)
            {
                orientationtype = OrientationType.end;
            }
            


            
            return _cutPlane;
            




        }

        public static Line FlipLine (Vector3d _guide, Line _line)
        {
            List<double> angle = new List<double>();
            List<Line> intersectionlines = new List<Line>() ;
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



        #endregion



    }

    public class BTLprocess
    {
        ProcessingType process;
        Brep voidgeometry;

        public BTLprocess(ProcessingType _process, Brep _voidGeometry)
        {
            voidgeometry = _voidGeometry;
            process = _process;
        }


        public ProcessingType Process { get { return process; } }
        public Brep Voidgeometry { get { return voidgeometry; } }

    }

    

}


