using System;
using System.Collections.Generic;
using Rhino.Geometry;

namespace PTK
{
    public class Loads
    {

        #region fields
        static int idCount = 0;

        public string Load_Tag { get; private set; }
        public int Load_ID { get; private set; }
        public Vector3d Load_vecotr { get; private set; }
        public Point3d Load_point { get; private set; }

        private Karamba.Loads.GH_Load krmb_GH_load;
        #endregion

        #region constructors
        public Loads(string _load_tag, Point3d _load_point, Vector3d _load_vector)
        {
            Load_Tag = _load_tag;       // inheriting Load Class
            Load_ID = -999;             // inheriting Load Class
            Load_vecotr = _load_vector; // inheriting Load Class
            Load_point = _load_point;   // inheriting Load Class
        }

        public Loads(Karamba.Loads.GH_Load _krmb_GH_load)
        {
            krmb_GH_load = _krmb_GH_load;               // inheriting Load Class
        }
        #endregion

        #region properties
        public Karamba.Loads.GH_Load Krmb_GH_load { get { return krmb_GH_load; } set { krmb_GH_load = value; } }

        #endregion

        #region methods
        public Loads Clone()
        {
            return (Loads)MemberwiseClone();
        }
        public void AssignID()
        {
            Load_ID = idCount;
            idCount++;
        }

        public static void ResetIDCount()
        {
            idCount = 0;
        }

        public override string ToString()
        {
            if (krmb_GH_load != null)
            {
                return base.ToString() + "\n" + krmb_GH_load.ToString();
            }

            return base.ToString();

        }

        #endregion
    }
}

