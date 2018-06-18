using System;
using System.Collections.Generic;
using Rhino.Geometry;

namespace PTK
{
    public class Load
    {
        #region fields
        public string LoadTag { get; private set; }
        public int LoadCase { get; private set; } = 0;
        public Point3d LoadPoint { get; private set; }
        public Vector3d LoadVector { get; private set; }
        #endregion

        #region constructors
        public Load()
        {
            LoadTag = "N/A";
            LoadPoint = new Point3d();
            LoadVector = new Vector3d();
        }
        public Load(string _loadTag, int _loadCase, Point3d _loadPoint, Vector3d _loadVector)
        {
            LoadTag = _loadTag;
            LoadCase = _loadCase;
            LoadPoint = _loadPoint;
            LoadVector = _loadVector;
        }
        #endregion

        #region properties
        #endregion

        #region methods
        #endregion
    }
}

