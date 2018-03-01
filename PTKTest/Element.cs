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
        private string tag;
        private List<int> ptid;
        private Point3d startpoint;
        private Point3d endpoint; 
        private Curve elemLine;
        private Section rectSec;
        private Material mtl;
        private Forces force;
        private Align align; 
        private List<SubElementStructural> subStructural;
        private int numberOfStructuralLines = 0;

        #endregion

        #region constructors
        public Element(Curve _crv, string _tag)
        {
            elemLine = _crv;
            tag = _tag;
            id = -999;
            endpoint = _crv.PointAtEnd;
            startpoint = _crv.PointAtStart;
            //n0id = -999: This one is currently missing, but easy to remake in the AsignNeighbour function
            //n1id = -999; This one is currently missing, but easy to remake in the AsignNeighbour function
            ptid = new List<int>();
            subStructural = new List<SubElementStructural>();
            


        }

        #endregion

        #region properties
        public Curve Crv{ get{ return elemLine;}}
        public int NumberOfStructuralLines { get { return numberOfStructuralLines; } }
        public string Tag{get { return tag; }set { tag = value; } }
        //public int N0id { get { return n0id; } set { n0id = value; } } Not working atm. See line 33
        //public int N1id { get { return n1id; } set { n1id = value; } } Not working atm. See line 34
        public int ID { get { return id; } set { id = value; } }
        public Section RectSec { get { return rectSec; } set { rectSec = value; } }
        public Material Mtl { get { return mtl; } set { mtl = value; } }
        public Forces Force { get { return force; } set { force = value; } }
        public Align Align { get { return align; } set { align = value; } }
        public List<SubElementStructural> SubStructural { get { return subStructural; } }

        #endregion

        #region methods

        //This class add neighbouring  points. The the analysis is done in the function called AsignNeighbour in functions.cs
        public void AddNeighbour(int _ids)
        {
            ptid.Add(_ids);
        }


        //This function send needed information to the subclass "subStructural"
        public void AddStrctline(Line _structuralline)
        { 

            subStructural.Add(new SubElementStructural(_structuralline, numberOfStructuralLines));
            numberOfStructuralLines++;
    
        }
        

        //THIS FUNCTION IS TEMPORARY. The function generates a box or a sweep. This determined by the linearity of the curve. 
        public Brep MakeBrep()
        {

            Brep Geometri = new Brep();
            double[] parameter = { 0.0, 2.2 };

            Plane[] Planet = new Plane[1];
            Planet = elemLine.GetPerpendicularFrames(parameter);
            Plane tempPlane = Planet[0];
            Interval iz = new Interval();
            Interval iy = new Interval();
            Interval ix = new Interval();
            
            double HalfWidth = rectSec.Width / 2;
            double HalfHeight = rectSec.Height / 2; 

            iz = new Interval(-HalfHeight, HalfHeight);
            iy = new Interval(-HalfWidth, HalfWidth);
            ix = new Interval(0,  elemLine.GetLength());



            if (elemLine.IsLinear())
            {



                Box boxen = new Box(Planet[0], iy, iz, ix);
                Geometri = boxen.ToBrep();
                

            }
            else
            {
                SweepOneRail tempsweep = new SweepOneRail();
                Rectangle3d rect = new Rectangle3d(tempPlane, iy, iz);
                rect.ToPolyline();
                
                var sweep = tempsweep.PerformSweep(elemLine, rect.ToNurbsCurve());
                

                Geometri = sweep[0];
            }



            return Geometri;
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