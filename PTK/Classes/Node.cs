﻿using System;
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
        private int id;
        private List<int> elemIds;
        private List<Element> elems;
        private List<double> elemParams;
        private int connectedElems;
        private Point3d pt3d;
        private Plane nodePlane;
        private double x;
        private double y;
        private double z;
        private static int idCount = 0;
        int temp;
        BoundingBox boundingbox; 
        #endregion

        #region constructors
        //The points are given from the PTK4_assemble. ID is unique by iterating each time the class is instanced. idCount is static
        public Node(Point3d pt)
        {
            pt3d = pt;
            x = pt.X;
            y = pt.Y;
            z = pt.Z;
            id = idCount;
            nodePlane = new Plane(pt, new Vector3d(0, 0, 1));
            idCount++;
            elemIds = new List<int>();

            elemParams = new List<double>();
            boundingbox = new BoundingBox(pt, pt);

            #region obsolete
            elems = new List<Element>();
            #endregion

        }
        #endregion

        #region properties
        public Point3d Pt3d
        {
            get { return pt3d; }
        }
        public BoundingBox BoundingBox
        {
            get { return boundingbox; }
        }

        public ReadOnlyCollection<int> ElemIds
        {
            get { return elemIds.AsReadOnly(); }

        }

        /*
        public List<double> ElemParams // ParameterOfConnectedElements
        {
            get { return elemParams; }
        }
        */        
         
        public ReadOnlyCollection<double> ElemParams
        {
            get { return elemParams.AsReadOnly(); }
        }

        public double X { get { return x; } }
        public double Y { get { return y; } }
        public double Z { get { return z; } }
        public int Id { get { return id; } }  //removed the possibility to set an ID
        public int ConnectedElements { get { return connectedElems; } }  //removed the possibility to set an ID

        #endregion

        #region methods

        public void AddElemId(int _id)
        {
            elemIds.Add(_id);
        }

        //Adding neighbours. see the function called AssignNeighbours in function.cs. 
        public void AddNeighbour(int ids)
        { 
            if (elemIds==null)
            {
                elemIds = new List<int>();
            }
            temp = ids;
            elemIds.Add(ids);
        }

        public void AddElemParams(double _param)
        {
            this.elemParams.Add(_param);
        }


        #region obsolete
        public void AddElements(Element _element)
        {
            bool add = true;
            foreach (Element elem in elems)
            {
                if (elem.Id.Equals(_element.Id))
                    add = false;
            }

            if (add)
            {
                elems.Add(_element);
            }
            
            elems.Add(_element);
        }
        #endregion

        // Are the next functions in use? -> yes, i intend to use them.
        // Probably useful later when extracting the geometry. 
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
