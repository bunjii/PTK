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
        public Point3d Point { get; private set; }
        public List<Vector3d> DisplacementVectors { get; private set; } = new List<Vector3d>();
        #endregion

        #region constructors
        public Node()
        {
            Point = new Point3d();
        }
        public Node(Point3d _point)
        {
            Point = _point;
        }
        #endregion

        #region properties
        #endregion

        #region methods
        public bool Equals(Node _other)
        {
            //It is necessary to consider a minute error
            if (Point == _other.Point)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Equals(Point3d _point)
        {
            if(Point == _point)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public int AddDisplacementVector(Vector3d _vector)
        {
            DisplacementVectors.Add(_vector);
            return DisplacementVectors.Count;
        }
        #endregion
    }
}
