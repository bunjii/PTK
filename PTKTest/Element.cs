using System;
using System.Collections.Generic;
using Rhino.Geometry;

namespace PTK
{
    public class Element
    {
        #region fields
        private int id;
        private string tag;
        private List<int> ptid;
        private int n0id;
        private int n1id;
        private Curve elemLine;
        private Section rectSec;
        private Material mtl;
        private Forces force;
        private List<Line> strctrlLine;
        private List<int> strctrlLineID;
        private int numberOfStructuralLines = 1;
        //private SubElementStructural SubEl

        #endregion

        #region constructors
        public Element(Curve _crv, string _tag)
        {
            elemLine = _crv;
            tag = _tag;
            id = -999;
            //n0id = -999;
            //n1id = -999;
            ptid = new List<int>();
            strctrlLine = new List<Line>();
            strctrlLineID = new List<int>();
        }

        #endregion

        #region properties
        public Curve Crv
        {
            get
            {
                return elemLine;
            }
        }
        public string Tag
        {
            get { return tag; }
            set { tag = value; }
        }
        public List<Line> StrctrlLine { get { return strctrlLine; }  }
        public int N0id { get { return n0id; } set { n0id = value; } }
        public int N1id { get { return n1id; } set { n1id = value; } }
        public int ID { get { return id; } set { id = value; } }
        public Section RectSec { get { return rectSec; } set { rectSec = value; } }
        public Material Mtl { get { return mtl; } set { mtl = value; } }
        public Forces Force { get { return force; } set { force = value; } }
        #endregion

        #region methods
        public static List<Element> AddNodeIds(List<Element> _elems, List<Node> _nodes)
        {

            return _elems;
        }

        public void AddNeighbour(int _ids)
        {
            ptid.Add(_ids);
        }

        public void AddStrctline(Line _structuralline)
        {
            strctrlLine.Add(_structuralline);
            strctrlLineID.Add(numberOfStructuralLines);
            numberOfStructuralLines++;

            
        }


        public static Element FindElementById(List<Element> _elems, int _eid)
        {
            Element tempElement;
            tempElement = _elems.Find(e => e.ID == _eid);

            return tempElement;
        }


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
            /*
            else
            {

                SweepOneRail SweepedGeomtry;
                
                Rectangle3d CrossSection;
                SweepedGeomtry = new SweepOneRail();
                SweepedGeomtry.SweepTolerance = 0;
                

                

                Rectangle3d TempRect = new Rectangle3d(tempPlane, iy, iz);
                Brep[] temp;
                temp = SweepedGeomtry.PerformSweep(elemLine, TempRect.ToNurbsCurve());
                
                Geometri = temp[0];


            }
            */
            return Geometri;

        }


        /*
        private class SubElementStructural
        {

            #region fields
            private int subIdStructural;
            private Line subLine;
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
            
            public SubElementStructural(Curve MainCurve, List<double> parameter)
            {
                

                MainCurve.ClosestPoint()


            }
            


            #endregion
            #region properties




            public List<int> SubIdStructural


            {
                get {
                    return elemIds;
                } set { elemIds = value; } }


            #endregion
            #region methods
            #endregion
    
    
        }
        */




            #endregion
            
    }
    
    }
