using System;

using System.Collections.ObjectModel;
using System.Collections.Generic;
using Rhino.Geometry;
using System.Linq;

namespace PTK
{
    public class Element
    {
        #region fields
        private int id;
        static int idCount = 0;
        private string tag = "N/A";
        private List<int> ptId;
        // private List<Node> nodes;
        private List<int> nodeIds;
        private List<double> nodeParams;
        private int connectedNodes = 0;
        private Point3d pointAtStart;
        private Point3d pointAtEnd;
        private Curve crv;
        private Section section;
        private int secId;
        private Material material;
        private int matId;
        private Forces force;
        private Align align;
        private List<SubElementStructural> subStructural;
        static int numberOfStructuralLines = 0;

        // below: with one of the xy- yz- zx- planes being fixed, the others can be calculated. so we can go with less field members.
        // private Plane xyPlane;
        // private Plane xzPlane;

        private Plane yzPlane;
        Interval iz; //From Centricplane
        Interval iy;
        Interval ix;
        Rectangle3d crossSectionRectangle;
        Brep elementGeometry;
        BoundingBox boundingbox;


        #endregion

        #region constructors
        public Element(Curve _crv, string _tag, Align _align, Section _section, Material _material)
        {
            // nodes = new List<Node>();
            nodeIds = new List<int>();
            crv = _crv;
            tag = _tag;
            align = _align;
            section = _section;
            material = _material;
            pointAtEnd = _crv.PointAtEnd;
            pointAtStart = _crv.PointAtStart;
            ptId = new List<int>();
            matId = -999;
            secId = -999;
            subStructural = new List<SubElementStructural>();

            // initializeCentricPlanes(); // replaced by DDL on 2nd April
            initializeCentricPlanes2();
            generateIntervals();
            generateElementGeometry();
            nodeParams = new List<double>();

            //n0id = -999: This one is currently missing, but easy to remake in the AsignNeighbour function
            //n1id = -999; This one is currently missing, but easy to remake in the AsignNeighbour function

        }
        #endregion

        #region properties
        public ReadOnlyCollection<int> NodeIds
        {
            get { return nodeIds.AsReadOnly(); }
        }

        public int MatId
        {
            get { return matId; }
            set { matId = value; }
        }

        public int SecId
        {
            get { return secId; }
            set { secId = value; }
        }

        public Curve Crv
        {
            get { return crv; }
        }
        public int NumberOfStructuralLines
        {
            get { return numberOfStructuralLines; }
        }
        public string Tag
        {
            get { return tag; }
            set { tag = value; }
        }
        public Plane localYZPlane
        {
            get { return yzPlane; }
        }

        //public int N0id { get { return n0id; } set { n0id = value; } } Not working atm. See line 33
        //public int N1id { get { return n1id; } set { n1id = value; } } Not working atm. See line 34

        public int Id
        {
            get { return id; }
        }
        public Section Section
        {
            get { return section; }
            set { section = value; }
        }
        public Material Material
        {
            get { return material; }
            set { material = value; }
        }
        public Forces Force
        {
            get { return force; }
            set { force = value; }
        }
        public Align Align
        {
            get { return align; }
            set { align = value; }
        }
        public List<SubElementStructural> SubStructural
        {
            get { return subStructural; }
        }
        public Brep ElementGeometry
        {
            get { return elementGeometry; }
        }
        public BoundingBox BoundingBox
        {
            get { return boundingbox; }
        }
        public Point3d PointAtStart
        {
            get { return pointAtStart; }
        }
        public Point3d PointAtEnd
        {
            get { return pointAtEnd; }
        }
        public int ConnectedNodes
        {
            get { return connectedNodes; }
        }

        // this should be ReadOnlyCollection
        // public List<double> NodeParams
        // {
        //     get { return nodeParams; }
        // }

        public ReadOnlyCollection<double> NodeParams
        {
            get { return nodeParams.AsReadOnly(); }
        }
        
        /*
        public List<Node> Nodes
        {
            get { return nodes; }
        }
        */
        #endregion

        #region methods
        public void AddNodeId(int _nid)
        {
            nodeIds.Add(_nid);
        }

        //This class add neighbouring points. The analysis is done in the function called AsignNeighbour in functions.cs
        public void AddNeighbour(int _ids)
        {
            ptId.Add(_ids);
        }
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
            nodeParams.Add(_param);
        }


        public void AssignID()
        {
            id = idCount;
            idCount++;
        }


        //This function send needed information to the subclass "subStructural"
        public void AddStrctline(Line _structuralline)
        { 

            subStructural.Add(new SubElementStructural(_structuralline, numberOfStructuralLines));
            numberOfStructuralLines++;
    
        }

        public static Element FindElemById(List<Element> _elems, int _eid)
        {
            Element tempElem;
            tempElem = _elems.Find(e => e.Id == _eid);

            return tempElem; 
        }

        #region obsolete
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
        #endregion

        private void initializeCentricPlanes2()
        {
            Vector3d localX = crv.TangentAtStart;
            Vector3d globalZ = Vector3d.ZAxis;

            // determination of local-y direction
                // case A: where local X is parallel to global Z. (such as most of the columns)

            Vector3d localY = Vector3d.YAxis; // case A default

                // case B: other than case A. (such as beams or inclined columns)
            if (Vector3d.Multiply(globalZ, localX) != globalZ.Length * localX.Length)
            {
                // localY direction is obtained by the cross product of globalZ and localX.
                localY = Vector3d.CrossProduct(globalZ, localX);
            }
            
            Plane localXY = new Plane(crv.PointAtStart, localX, localY);
            Plane localYZ = new Plane(localXY.Origin, localY, localXY.ZAxis);

            // rotation
            if (align.RotationAngle != 0.0)
            {
                double rad = align.RotationAngle * Math.PI / 180; // degree to radian
                Vector3d axis = localYZ.ZAxis;
                Point3d origin = localYZ.Origin;
                Transform transR = Transform.Rotation(rad, axis, origin);

                localXY.Transform(transR);
                localYZ.Transform(transR);

            }

            // translation
            if (align.OffsetY != 0.0 || align.OffsetZ != 0.0)
            {
                Point3d origin = new Point3d (crv.PointAtStart);
                Transform transL = Transform.Translation((-1.0) * align.OffsetY * localYZ.XAxis + align.OffsetZ * localYZ.YAxis);

                localXY.Transform(transL);
                localYZ.Transform(transL);
            }
                        
            yzPlane = localYZ;
        }

        //Generating extrusion/SweepIntervals
        private void generateIntervals()
        {
            double[] parameter = { 0.0, 2.2 };

            

            double HalfWidth = section.Width / 2;
            double HalfHeight = section.Height / 2;

            iz = new Interval(-HalfHeight, HalfHeight);
            iy = new Interval(-HalfWidth, HalfWidth);
            ix = new Interval(0, crv.GetLength());
            crossSectionRectangle = new Rectangle3d(yzPlane, iy, iz);
            
        }

        private void generateElementGeometry()
        {
            Brep tempgeometry = new Brep();

            if (crv.IsLinear())
            {
                Box boxen = new Box(yzPlane, iy, iz, ix);
                tempgeometry = Brep.CreateFromBox(boxen);
                boundingbox = boxen.BoundingBox;

            }
            else
            {
                SweepOneRail tempsweep = new SweepOneRail();
                
                var sweep = tempsweep.PerformSweep(crv, crossSectionRectangle.ToNurbsCurve());
                tempgeometry = sweep[0];
                boundingbox = tempgeometry.GetBoundingBox(Rhino.Geometry.Plane.WorldXY);
                int test = 0;
            }


            elementGeometry = tempgeometry;
        }

        public static void ResetIDCount()
        {
            idCount = 0;
            numberOfStructuralLines = 0;
        }
        #endregion

        // THIS IS THE CLASS THAT STORES DATA OF ALL BEAMELEMENTS. SUB LINES. 
        // An element contains one or more sub-elements.
        public class SubElementStructural
        {

            #region fields

            private Line strctrlLine;
            private int strctrlLineID;
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
            public SubElementStructural(Line _subLine, int _id)
            {
                strctrlLine = _subLine;
                strctrlLineID= _id;
                subStartPoint = _subLine.From;
                subEndPoint = _subLine.To;
            }

            #endregion

            #region properties
            public Line StrctrLine { get { return strctrlLine; } }
            public int StrctrlLineID { get { return strctrlLineID; } }
            
            #endregion
            
            #region methods
            public static void ResetSubStrIdCnt()
            {
                numberOfStructuralLines = 0;
            }
            #endregion
            
        }
        
    }
    
}
