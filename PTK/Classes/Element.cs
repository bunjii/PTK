using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PTK
{
    public class Element
    {
        #region fields
        static int idCount = 0;
        public List<int> NodeIds { get; private set; } = new List<int>();
        public int MatId { get; set; }
        public int SecId { get; set; }
        public Curve Crv { get; private set; }
        public static int NumberOfStructuralLines { get; private set; }
        public string Tag { get; private set; } = "N/A";
        // below: with one of the xy- yz- zx- planes being fixed, 
        // the others can be calculated. 
        // so we can go with less field members.
        // private Plane xyPlane;
        // private Plane xzPlane;
        private Plane yzPlane;
        public int Id { get; private set; }
        public Section Section { get; private set; }
        public Material Material { get; private set; }
        public Forces Force { get; set; }
        public Align Align { get; private set; }
        public int Priority { get; set; }
        public List<Subelement> SubElem { get; private set; } = new List<Subelement>();
        public List<SubElementBTL> SubElementBTL { get; private set; } = new List<SubElementBTL>();

        public Brep ElementGeometry { get; private set; }
        public BoundingBox BoundingBox { get; private set; }
        public Point3d PointAtStart { get; private set; }
        public Point3d PointAtEnd { get; private set; }
        public int ConnectedNodes { get; private set; } = 0;
        public List<double> NodeParams { get; private set; } = new List<double>();
        // below has moved to PTK_UTIL_1_GenerateGeometry
        Interval iz; //From Centricplane
        Interval iy;
        Interval ix;
        Rectangle3d crossSectionRectangle;
        #endregion

        #region constructors
        public Element(Curve _crv, string _tag, Align _align, Section _section, Material _material)
        {
            Crv = _crv;
            Tag = _tag;
            Align = _align;
            Section = _section;
            Material = _material;
            PointAtEnd = _crv.PointAtEnd;
            PointAtStart = _crv.PointAtStart;
            MatId = -999;
            SecId = -999;
            Priority = -999;

            // initializeCentricPlanes();   // replaced by DDL on 2nd April
            InitializeCentricPlanes();
            GenerateIntervals();
            GenerateElementGeometry();

            //n0id = -999: This one is currently missing, but easy to remake in the AsignNeighbour function
            //n1id = -999; This one is currently missing, but easy to remake in the AsignNeighbour function

        }
        #endregion

        #region properties

        public double GetLength()
        {
            return Crv.GetLength();
        }

        public Plane LocalYZPlane
        {
            get { return yzPlane; }
        }

        #endregion

        #region methods
        public Element Clone()
        {
            return (Element)MemberwiseClone();
        }

        public void AddNodeId(int _nid)
        {
            this.NodeIds.Add(_nid);
        }

        public void ClrNodeData()
        {
            this.NodeIds.Clear();
            this.NodeParams.Clear();
        }

        //This class add neighbouring points. The analysis is done in the function called AsignNeighbour in functions.cs
        //public void AddNeighbour(int _ids)
        //{
        //    this.ptId.Add(_ids);
        //}
        /*
        public void AddNode(Node _node)
        {
            bool add = true;
            foreach(Node node in nodes)
            {
                if (_node.ID.Equals(node.ID))
                    add = false;
            }
            
            if (add)
            {
                nodes.Add(_node);
                connectedNodes++;
            }
        }
        */
        public void AddNodeParams(double _param)
        {
            this.NodeParams.Add(_param);
        }

        public void AssignID()
        {
            this.Id = idCount;
            idCount++;
        }



        #region obsolete
        /*
        //Making CentricPlanes using offset/rotation information from the align-component
        private void initializeCentricPlanes()
        {
            Plane tempPlane = new Plane(crv.PointAtStart, crv.TangentAtStart);
            align.rotationVectorToPoint(crv.PointAtStart);
            Vector3d alignvector = align.Rotation;
            //Getting rotation angle
            double angle = Rhino.Geometry.Vector3d.VectorAngle(tempPlane.XAxis, alignvector, tempPlane);

            tempPlane.Rotate(angle, tempPlane.Normal, tempPlane.Origin);
            tempPlane.Translate(tempPlane.XAxis * align.OffsetY);
            tempPlane.Translate(tempPlane.YAxis * align.OffsetZ);
            yzPlane = tempPlane;
            // xzPlane = new Plane(tempPlane.Origin, tempPlane.ZAxis, tempPlane.YAxis);
            // xyPlane = new Plane(tempPlane.Origin, tempPlane.ZAxis, tempPlane.XAxis);

        }
        */
        #endregion

        private void InitializeCentricPlanes()
        {
            Vector3d localX = Crv.TangentAtStart;
            Vector3d globalZ = Vector3d.ZAxis;

            // determination of local-y direction
            // case A: where local X is parallel to global Z.
            // (such as most of the columns)

            Vector3d localY = Vector3d.YAxis; // case A default

            // case B: other than case A. (such as beams or inclined columns)
            if (Vector3d.Multiply(globalZ, localX) != globalZ.Length * localX.Length)
            {
                // localY direction is obtained by the cross product of globalZ and localX.
                localY = Vector3d.CrossProduct(globalZ, localX);
            }

            Plane localXY = new Plane(Crv.PointAtStart, localX, localY);
            Plane localYZ = new Plane(localXY.Origin, localY, localXY.ZAxis);

            // rotation
            if (Align.RotationAngle != 0.0)
            {
                double rad = Align.RotationAngle * Math.PI / 180; // degree to radian
                Vector3d axis = localYZ.ZAxis;
                Point3d origin = localYZ.Origin;
                Transform transR = Transform.Rotation(rad, axis, origin);

                localXY.Transform(transR);
                localYZ.Transform(transR);

            }

            // translation
            if (Align.OffsetY != 0.0 || Align.OffsetZ != 0.0)
            {
                Point3d origin = new Point3d(Crv.PointAtStart);
                Transform transL = Transform.Translation((-1.0) * Align.OffsetY * localYZ.XAxis + Align.OffsetZ * localYZ.YAxis);

                localXY.Transform(transL);
                localYZ.Transform(transL);
            }

            yzPlane = localYZ;
        }

        // has moved to PTK_UTIL1_GenerateGeometry
        // Generating extrusion/SweepIntervals
        private void GenerateIntervals()
        {
            //double[] parameter = { 0.0, 2.2 };

            double HalfWidth = Section.Width / 2;
            double HalfHeight = Section.Height / 2;

            iz = new Interval(-HalfHeight, HalfHeight);
            iy = new Interval(-HalfWidth, HalfWidth);
            ix = new Interval(0, Crv.GetLength());
            crossSectionRectangle = new Rectangle3d(yzPlane, iy, iz);
        }

        private void GenerateElementGeometry()
        {
            Brep tempgeometry = new Brep();

            if (Crv.IsLinear())
            {
                Box boxen = new Box(yzPlane, iy, iz, ix);
                tempgeometry = Brep.CreateFromBox(boxen);
                BoundingBox = boxen.BoundingBox;

            }
            else
            {
                SweepOneRail tempsweep = new SweepOneRail();

                var sweep = tempsweep.PerformSweep(Crv, crossSectionRectangle.ToNurbsCurve());
                tempgeometry = sweep[0];
                BoundingBox = tempgeometry.GetBoundingBox(Rhino.Geometry.Plane.WorldXY);
                int test = 0;
            }
            ElementGeometry = tempgeometry;
        }

        public static void ResetIDCount()
        {
            idCount = 0;
            NumberOfStructuralLines = 0;
        }
        #endregion

        // THIS IS THE CLASS THAT STORES DATA OF ALL BEAMELEMENTS. SUB LINES. 
        // An element contains one or more sub-elements.
        public class Subelement
        {

            #region fields
            private Point3d subStartPoint;
            private Point3d subEndPoint;
            private double fx;
            private double fy;
            private double fz;
            private double mx;
            private double my;
            private double mz;

            #endregion

            #region constructors
            public Subelement(Line _subLn, int _id)
            {
                StrLn = _subLn;
                StrLnId = _id;
                subStartPoint = _subLn.From;
                subEndPoint = _subLn.To;

                SNId = -999;
                ENId = -999;
            }

            #endregion

            #region properties
            public Line StrLn { get; private set; }
            public int StrLnId { get; private set; }
            public int SNId { get; set; }// meaning start node ID
            public int ENId { get; set; }// meaning end node ID

            #endregion

            #region methods
            public static void ResetSubStrIdCnt()
            {
                NumberOfStructuralLines = 0;
            }
            #endregion
        }

        //This function send needed information to the subclass "subElement"
        public void AddSubElem(Line _strLn)
        {
            this.SubElem.Add(new Subelement(_strLn, NumberOfStructuralLines));
            NumberOfStructuralLines++;

        }

        public void ClrSubElem()
        {
            this.SubElem = new List<Subelement>();
            // this.subElem.Clear();
        }

        public static Element FindElemById(List<Element> _elems, int _eid)
        {
            Element tempElem;
            tempElem = _elems.Find(e => e.Id == _eid);

            return tempElem;
        }

    }
    
    }
