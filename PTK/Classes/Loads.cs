using System;
using System.Collections.Generic;
using Rhino.Geometry;

namespace PTK
{
    public class Loads
    {

        #region fields
        public string Load_Tag { get; private set; }
        public int Load_ID { get; private set; }
        public Vector3d Load_vecotr { get; private set; }
        public Point3d Load_point { get; private set; }
        #endregion

        #region constructors
        public Loads(string _load_tag, Point3d _load_point, Vector3d _load_vector)
        {
            Load_Tag = _load_tag;       // inheriting Load Class
            Load_ID = -999;             // inheriting Load Class
            Load_vecotr = _load_vector; // inheriting Load Class
            Load_point = _load_point;   // inheriting Load Class
        }
        #endregion

        #region properties
        #endregion

        #region methods
        #endregion
    }
}

