using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace PTK
{
    public class Forces
    {

        #region fields

        public double Max_Fx_compression { get; set; }
        public double Max_Fx_tension { get; set; }
        public double Max_Fy_shear { get; set; }
        public double Max_Fz_shear { get; set; }
        public double Max_Mx_torsion { get; set; }
        public double Max_My_bending { get; set; }
        public double Max_Mz_bending { get; set; }

        public Point3d Position_Max_Fx_compression { get;  set; }
        public Point3d Position_Max_Fx_tension { get;  set; }
        public Point3d Position_Max_Fy_shear { get;  set; }
        public Point3d Position_Max_Fz_shear { get;  set; }
        public Point3d Position_Max_Mx_torsion { get;  set; }
        public Point3d Position_Max_My_bending { get;  set; }
        public Point3d Position_Max_Mz_bending { get;  set; }

        public int Loadcase_Max_Fx_compression { get;  set; }
        public int Loadcase_Max_Fx_tension { get;  set; }
        public int Loadcase_Max_Fy_shear { get;  set; }
        public int Loadcase_Max_Fz_shear { get;  set; }
        public int Loadcase_Max_Mx_torsion { get;  set; }
        public int Loadcase_Max_My_bending { get;  set; }
        public int Loadcase_Max_Mz_bending { get;  set; }


        public List<double> FXc { get; set; }
        public List<double> FXt { get; set; }
        public List<double> FY { get; set; }
        public List<double> FZ { get; set; }
        public List<double> MX { get; set; }
        public List<double> MY { get; set; }
        public List<double> MZ { get; set; }
        #endregion

        #region constructors
        public Forces()
        {
            // empty class

        }
        public Forces(
            List<double> _fxc,
            List<double> _fxt,
            List<double> _fy,
            List<double> _fz,
            List<double> _mx,
            List<double> _my,
            List<double> _mz
            )
        {
            // only list of forces
            FXc = _fxc;
            FXt = _fxc;
            FY = _fy;
            FZ = _fz;
            MX = _mx;
            MY = _my;
            MZ = _mz;
        }
        public Forces(
            List<double> _fxc,
            List<double> _fxt,
            List<double> _fy,
            List<double> _fz,
            List<double> _mx,
            List<double> _my,
            List<double> _mz,
            int _loadcase_max_Fx_compression,
            int _loadcase_max_Fx_tension,
            int _loadcase_max_Fy_shear,
            int _loadcase_max_Fz_shear,
            int _loadcase_max_Mx_torsion,
            int _loadcase_max_My_bending,
            int _loadcase_max_Mz_bending
            )
        {
            // only list of forces
            FXc = _fxc;
            FXt = _fxc;
            FY = _fy;
            FZ = _fz;
            MX = _mx;
            MY = _my;
            MZ = _mz;
            Loadcase_Max_Fx_compression = _loadcase_max_Fx_compression;
            Loadcase_Max_Fx_tension = _loadcase_max_Fx_tension;
            Loadcase_Max_Fy_shear = _loadcase_max_Fy_shear;
            Loadcase_Max_Fz_shear = _loadcase_max_Fz_shear;
            Loadcase_Max_Mx_torsion = _loadcase_max_Mx_torsion;
            Loadcase_Max_My_bending = _loadcase_max_My_bending;
            Loadcase_Max_Mz_bending = _loadcase_max_Mz_bending;
        }

        #endregion

        #region properties
        #endregion

        #region methods
        #endregion
    }
}
