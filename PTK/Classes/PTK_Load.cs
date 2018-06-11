using System;
using System.Collections.Generic;
using Rhino.Geometry;

namespace PTK
{
    public class PTK_Load
    {
        #region fields
        static int idCount = 0;

        private int id;

        private string load_tag;
        private int load_id;
        private Vector3d load_vector;
        private Point3d load_point;
        private Karamba.Loads.GH_Load krmb_load;
        #endregion

        #region constructors
        public PTK_Load(string _load_tag, Point3d _load_point, Vector3d _load_vector)
        {
            load_tag = _load_tag;               // inheriting Load Class
            load_id = -999;                     // inheriting Load Class
            load_vector = _load_vector;         // inheriting Load Class
            load_point = _load_point;           // inheriting Load Class

        }

        public PTK_Load(Karamba.Loads.GH_Load _krmb_load)
        {
            krmb_load = _krmb_load ;               // inheriting Load Class

        }
        #endregion

        #region properties
        public string Load_Tag { get { return load_tag; } set { load_tag = value; } }
        public int Load_ID { get { return load_id; } set { load_id = value; } }
        public Vector3d Load_vecotr { get { return load_vector; } set { load_vector = value; } }
        public Point3d Load_point { get { return load_point; } set { load_point = value; } }

        public Karamba.Loads.GH_Load Krmb_load { get { return krmb_load; } set { krmb_load = value; } }
        #endregion

        #region methods

        public void AssignID()
        {
            this.id = idCount;
            idCount++;
        }

        public static void ResetIDCount()
        {
            idCount = 0;
        }
        #endregion
    }
}

