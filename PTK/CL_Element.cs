using System;
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
        private List<int> ptid;
        private List<Node> nodes;
        private List<double> parameterConnectedNodes;
        private int connectedNodes = 0;
        private Point3d pointAtStart;
        private Point3d pointAtEnd;
        private Curve crv;
        private Section section;
        private Material material;
        private Forces force;
        private Align align;
        private List<SubElementStructural> subStructural;
        private int numberOfStructuralLines = 0;
        private Plane xyPlane;
        private Plane xzPlane;
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
            nodes = new List<Node>();
            crv = _crv;
            tag = _tag;
            align = _align;
            section = _section;
            material = _material;
            pointAtEnd = _crv.PointAtEnd;
            pointAtStart = _crv.PointAtStart;
            ptid = new List<int>();
            subStructural = new List<SubElementStructural>();
            initializeCentricPlanes();
            generateIntervals();
            generateElementGeometry();
            parameterConnectedNodes = new List<double>();

            //n0id = -999: This one is currently missing, but easy to remake in the AsignNeighbour function
            //n1id = -999; This one is currently missing, but easy to remake in the AsignNeighbour function

        }
        #endregion

        #region properties

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

        public int ID
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
        public List<double> ParameterConnectedNodes
        {
            get { return parameterConnectedNodes; }
        }
        public List<Node> Nodes
        {
            get { return nodes; }
        }

        #endregion

        #region methods


        //This class add neighbouring  points. The analysis is done in the function called AsignNeighbour in functions.cs
        public void AddNeighbour(int _ids)
        {
            ptid.Add(_ids);
        }

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


        public void AsignID()
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
            xzPlane = new Plane(tempPlane.Origin, tempPlane.ZAxis, tempPlane.YAxis);
            xyPlane = new Plane(tempPlane.Origin, tempPlane.ZAxis, tempPlane.XAxis);


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

        //THIS IS THE CLASS THAT STORES DATA OF ALL BEAMELEMENTS. SUB LINES. An element contains one or more sub-elements
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
            #endregion


        }
        

            
    }
    
    }
#endregion