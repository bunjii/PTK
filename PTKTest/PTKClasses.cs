using System;
using System.Collections.Generic;
using Rhino.Geometry;

using System.Windows.Forms;

namespace PTK
{
#region class nodes
    public class Node : IEquatable<Node>
    {
        #region fields
        private int id;
        private List<int> elemIds;
        private Point3d pt3d;
        private double x;
        private double y;
        private double z;
        #endregion

        #region constructors
        public Node(Point3d pt)
        {
            pt3d = pt;
            x = pt.X;
            y = pt.Y;
            z = pt.Z;
            id = -999;
            elemIds = new List<int>();
        }
        #endregion

        #region properties
        public Point3d Pt3d { get { return pt3d; } }
        public double X { get { return x; } }
        public double Y { get { return y; } }
        public double Z { get { return z; } }
        public int ID { get { return id; } set { id = value; } }
        public List<int> ElemIds { get { return elemIds; } set { elemIds = value; } }
        #endregion

        #region methods

        public bool Equals(Node other)
        {
            if (x == other.X && y == other.Y && z == other.Z)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static List<Node> AddElemIds(List<Node> _nodes, Element _elem, Node _nd)
        {
            if (!_nodes.Contains(_nd))
            {
                _nd.ElemIds.Add(_elem.ID);
                _nodes.Add(_nd);
            }
            else
            {
                _nodes.Find(n => n.Pt3d == _nd.Pt3d).elemIds.Add(_elem.ID);
            }

            return _nodes;
        }

        public static int FindNodeId(List<Node> _nodes, Point3d _pt)
        {
            int tempId = -999;
            tempId = _nodes.Find(n => n.Pt3d == _pt).ID;

            return tempId;
        }

        public static Node FindNodeById(List<Node> _nodes, int _nid)
        {
            Node tempNode;
            tempNode = _nodes.Find(n => n.ID == _nid);

            return tempNode; 
        }
        #endregion
    }
    #endregion

#region class elements
    public class Element 
    {
        #region fields
        private int id;
        private string tag;
        private int n0id;
        private int n1id;
        private Line elemLine;
        private Section rectSec;

        #endregion

        #region constructors
        public Element(Line _line, string _tag)
        {
            elemLine = _line;
            tag = _tag;
            id = -999;
            n0id = -999;
            n1id = -999;
        }

        #endregion

        #region properties
        public Line Ln { get { return elemLine; } }
        public string Tag
        {
            get { return tag; }
            set { tag = value; }
        }
        public int N0id { get { return n0id; } set { n0id = value; } }
        public int N1id { get { return n1id; } set { n1id = value; } }
        public int ID { get { return id; } set { id = value; } }
        public Section RectSec { get { return rectSec; } set { rectSec = value; } }
        #endregion

        #region methods
        public static List<Element> AddNodeIds(List<Element> _elems, List<Node> _nodes)
        {

            return _elems;
        }

        public static Element FindElementById(List<Element> _elems, int _eid)
        {
            Element tempElement;
            tempElement = _elems.Find(e => e.ID == _eid);

            return tempElement;
        }
        #endregion
    }
    #endregion

#region class section
    public class Section
    {
        #region fields
        private string tag;
        private int id;
        private Vector3d offset;
        private double width;
        private double height;
        #endregion

        #region constructors
        public Section(string _tag, double _width, double _height, Vector3d _offset)
        {
            tag = _tag; // inheriting Section Class
            id = -999; // inheriting Section Class
            offset = _offset; // inheriting Section Class
            width = _width;
            height = _height;
        }
        #endregion

        #region properties
        public double Width { get { return width; }  }
        public double Height { get { return height; }  }
        public string Tag { get { return tag; } set { tag = value; } }
        public int ID { get { return id; } set { id = value; } }
        public Vector3d Offset { get { return offset; } set { offset = value; } }
        #endregion

        #region methods

        #endregion
    }
    #endregion

#region class Load
    public class Load 
    {
        #region fields
        private string tag;
        private int loadid;
        private Vector3d loadvector;
        private Point3d loadpoint;

        #endregion

        #region constructors
        public Load(string _tag, double _width, double _height, Vector3d _loadvector, Point3d _loadpoint)
        {
            tag = _tag; // inheriting Section Class
            loadid = -999; // inheriting Section Class
            
            
        }
        #endregion

        #region properties

        public string Tag { get { return tag; } set { tag = value; } }
        public int Loadid { get { return loadid; } set { loadid = value; } }
        public Vector3d Loadvector { get { return loadvector; } set { loadvector = value; } }
        public Point3d Loadpoint { get { return loadpoint; } set { loadpoint = value; } }
        #endregion
    }

    #endregion
    /*
    public class SomeNewClass
    {
        #region fields
        #endregion
        #region constructors
        #endregion
        #region properties
        #endregion
        #region methods
        #endregion
    }

        private double width;
        private double height;
        private Vector2d offsetVec;
    */
}
