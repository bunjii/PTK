using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;


namespace PTK
{
    public class Node : IEquatable<Node>
    {

        #region fields
        private static int idCount = 0;
        public Point3d Pt3d { get; private set; }
        public BoundingBox BoundingBox { get; private set; }
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Z { get; private set; }
        public int Id { get; private set; }  //removed the possibility to set an ID
        public int ConnectedElements { get; private set; }  //removed the possibility to set an ID
        public List<int> ElemIds { get; private set; } = new List<int>();
        public List<Element> Elems { get; private set; } = new List<Element>();
        public List<double> ElemParams { get; private set; } = new List<double>();
        public Plane NodePlane { get; private set; }
        //int temp;
        #endregion

        #region constructors
        //The points are given from the PTK4_assemble. ID is unique by iterating each time the class is instanced. idCount is static
        public Node(Point3d pt)
        {
            Pt3d = pt;
            X = pt.X;
            Y = pt.Y;
            Z = pt.Z;
            Id = idCount;
            NodePlane = new Plane(pt, new Vector3d(0, 0, 1));
            idCount++;
            BoundingBox = new BoundingBox(pt, pt);
        }
        #endregion

        #region properties
        #endregion

        #region methods

        public void AddElemId(int _id)
        {
            ElemIds.Add(_id);
        }

        //Adding neighbours. see the function called AssignNeighbours in function.cs. 
        public void AddNeighbour(int ids)
        { 
            if (ElemIds==null)
            {
                ElemIds = new List<int>();
            }
            //temp = ids;
            ElemIds.Add(ids);
        }

        public void AddElemParams(double _param)
        {
            this.ElemParams.Add(_param);
        }


        #region obsolete
        public void AddElements(Element _element)
        {
            bool add = true;
            foreach (Element elem in Elems)
            {
                if (elem.Id.Equals(_element.Id))
                    add = false;
            }

            if (add)
            {
                Elems.Add(_element);
            }
            
            Elems.Add(_element);
        }
        #endregion

        // Are the next functions in use? -> yes, i intend to use them.
        // Probably useful later when extracting the geometry. 
        public bool Equals(Node other)
        {
            if (X == other.X && Y == other.Y && Z == other.Z)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static int FindNodeId(List<Node> _nodes, Point3d _pt)
        {
            int tempId = -999;
            tempId = _nodes.Find(n => n.Pt3d == _pt).Id;

            return tempId;
        }

        public static Node FindNodeById(List<Node> _nodes, int _nid)
        {
            Node _tempNode;
            _tempNode = _nodes.Find(n => n.Id == _nid);

            return _tempNode;
        }

        public static void ResetIDCount()
        {
            idCount = 0;
        }

        #endregion
    }
}
