using System;
using System.Collections.Generic;
using Rhino.Geometry;

namespace PTK
{
    public class Load
    {
        #region fields
        public string Tag { get; private set; }
        public int LoadCase { get; private set; } = 0;
        public Point3d LoadPoint { get; private set; }
        public Vector3d LoadVector { get; private set; }
        #endregion

        #region constructors
        public Load()
        {
            Tag = "N/A";
            LoadPoint = new Point3d();
            LoadVector = new Vector3d();
        }
        public Load(string _tag, int _loadCase, Point3d _loadPoint, Vector3d _loadVector)
        {
            Tag = _tag;
            LoadCase = _loadCase;
            LoadPoint = _loadPoint;
            LoadVector = _loadVector;
        }
        #endregion

        #region properties
        #endregion

        #region methods
        public Load DeepCopy()
        {
            return (Load)base.MemberwiseClone();
        }
        public override string ToString()
        {
            string info;
            info = "<Load> Tag:" + Tag +
                " LoadCase:" + LoadCase.ToString() +
                " LoadPoint:" + LoadPoint.ToString();
            return info;
        }
        public bool IsValid()
        {
            return true;
        }
        #endregion
    }
}

