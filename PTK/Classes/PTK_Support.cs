using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Rhino.Geometry;
using Karamba;


namespace PTK
{
    public class PTK_Support
    {

        #region fields
        static int idCount = 0;

        private int id;
        private Point3d sup_point;
        private Plane sup_orient;
        private Plane sup_plane;
        private List<bool> sup_bc;
        
        private Karamba.Supports.GH_Support krmb_GH_support;
        #endregion

        #region constructors
        public PTK_Support( Karamba.Supports.GH_Support _krmb_GH_support) 
        {
            id = -999;
            krmb_GH_support = _krmb_GH_support;

        }

        public PTK_Support(Point3d _sup_point, Plane _sup_orient)
        {
            id = -999;
            sup_point = _sup_point;
            sup_orient = _sup_orient;
        }
        #endregion

        #region properties
        public int Id { get { return id; } set { id = value; } }

        public Karamba.Supports.GH_Support Krmb_GH_support { get { return krmb_GH_support; } set { krmb_GH_support = value; } }
        public Point3d Sup_point { get { return sup_point; } set { sup_point = value; } }
        public Plane Sup_orient { get { return sup_orient; } set { sup_orient = value; } }

        #endregion

        #region methods
        public PTK_Support Clone()
        {
            return (PTK_Support)MemberwiseClone();
        }
        public void AssignID()
        {
            this.id = idCount;
            idCount++;
        }

        public static void ResetIDCount()
        {
            idCount = 0;
        }

        public override string ToString()
        {
            if (krmb_GH_support != null) {
                return base.ToString() + "\n" + krmb_GH_support.ToString();
            }

            return base.ToString();

        }

        #endregion
    }
}
