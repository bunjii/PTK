using System;
using System.Collections.Generic;
using Rhino.Geometry;

namespace PTK
{
    public class Loads
    {
        #region fields
        private string load_tag;
        private int load_id;
        private Vector3d load_vector;
        private Point3d load_point;

        #endregion

        #region constructors
        public Loads(string _load_tag, Point3d _load_point, Vector3d _load_vector)
        {
            load_tag = _load_tag; // inheriting Load Class
            load_id = -999; // inheriting Load Class
            load_vector = _load_vector; // inheriting Load Class
            load_point = _load_point; // inheriting Load Class
        }
        #endregion

        #region properties
        public string Load_Tag { get { return load_tag; } set { load_tag = value; } }
        public int Load_ID { get { return load_id; } set { load_id = value; } }
        public Vector3d Load_vecotr { get { return load_vector; } set { load_vector = value; } }
        public Point3d Load_point { get { return load_point; } set { load_point = value; } }
        #endregion

        #region methods

        #endregion
    }
}

