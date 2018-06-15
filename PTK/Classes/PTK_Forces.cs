using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rhino.Geometry;

namespace PTK
{
    public class PTK_Forces
    {
        #region fields
        private double max_Fx_compression;
        private double max_Fx_tension;
        private double max_Fy_shear;
        private double max_Fz_shear;

        private double max_Mx_torsion;
        private double max_My_bending;
        private double max_Mz_bending;

        private Point3d position_max_Fx_compression;
        private Point3d position_max_Fx_tension;
        private Point3d position_max_Fy_shear;
        private Point3d position_max_Fz_shear;
        private Point3d position_max_Mx_torsion;
        private Point3d position_max_My_bending;
        private Point3d position_max_Mz_bending;

        private int loadcase_max_Fx_compression;
        private int loadcase_max_Fx_tension;
        private int loadcase_max_Fy_shear;
        private int loadcase_max_Fz_shear;
        private int loadcase_max_Mx_torsion;
        private int loadcase_max_My_bending;
        private int loadcase_max_Mz_bending;

        private List<double> fxc;
        private List<double> fxt;
        private List<double> fy;
        private List<double> fz;
        private List<double> mx;
        private List<double> my;
        private List<double> mz;

        #endregion

        #region constructors
        // constructor #1 empty
        public PTK_Forces()
        {
            // empty class
            
        }
        // constructor #2 only list of forces
        public PTK_Forces(
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
            fxc = _fxc;
            fxt = _fxc;
            fy = _fy;
            fz = _fz;
            mx = _mx;
            my = _my;
            mz = _mz;
        }
        // constructor #3 only list of forces
        public PTK_Forces(
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
            fxc = _fxc;
            fxt = _fxc;
            fy = _fy;
            fz = _fz;
            mx = _mx;
            my = _my;
            mz = _mz;
            loadcase_max_Fx_compression =_loadcase_max_Fx_compression;
            loadcase_max_Fx_tension =_loadcase_max_Fx_tension;
            loadcase_max_Fy_shear =_loadcase_max_Fy_shear;
            loadcase_max_Fz_shear =_loadcase_max_Fz_shear;
            loadcase_max_Mx_torsion= _loadcase_max_Mx_torsion;
            loadcase_max_My_bending= _loadcase_max_My_bending;
            loadcase_max_Mz_bending= _loadcase_max_Mz_bending;
        }

        #endregion

        #region properties
        public List<double> FXc
        {
            get { return fxc; }
            set { fxc = value; }
        }
        public List<double> FXt
        {
            get { return fxt; }
            set { fxt = value; }
        }
        public List<double> FY
        {
            get { return fy; }
            set { fy = value; }
        }
        public List<double> FZ
        {
            get { return fz; }
            set { fz = value; }
        }
        public List<double> MX
        {
            get { return mx; }
            set { mx = value; }
        }
        public List<double> MY
        {
            get { return my; }
            set { my = value; }
        }
        public List<double> MZ
        {
            get { return mz; }
            set { mz = value; }
        }

        // max values
        public double maxCompression
        {
            get { return max_Fx_compression; }
            set { max_Fx_compression = value; }
        }
        public double maxTension
        {
            get { return max_Fx_tension; }
            set { max_Fx_tension = value; }
        }
        public double maxShearY
        {
            get { return max_Fy_shear; }
            set { max_Fy_shear = value; }
        }
        public double maxShearZ
        {
            get { return max_Fz_shear; }
            set { max_Fz_shear = value; }
        }
        public double maxTorsion
        {
            get { return max_Mx_torsion;}
            set { max_Mx_torsion = value; }
        }
        public double maxBendingY
        {
            get { return max_My_bending; }
            set { max_My_bending = value; }
        }
        public double maxBendingZ
        {
            get { return max_Mz_bending; }
            set { max_Mz_bending = value; }
        }
        
        // positions
        public Point3d position_Fx_maxCompression
        {
            get { return position_max_Fx_compression; }
            set { position_max_Fx_compression = value; }
        }
        public Point3d position_Fx_position_maxTension
        {
            get { return position_max_Fx_tension; }
            set { position_max_Fx_tension = value; }
        }
        public Point3d position_Fy_maxShearY
        {
            get { return position_max_Fy_shear; }
            set { position_max_Fy_shear = value; }
        }
        public Point3d position_Fz_maxShearZ
        {
            get { return position_max_Fz_shear; }
            set { position_max_Fz_shear = value; }
        }
        public Point3d position_Mx_maxTorsion
        {
            get { return position_max_Mx_torsion; }
            set { position_max_Mx_torsion = value; }
        }
        public Point3d position_My_maxBendingY
        {
            get { return position_max_My_bending; }
            set { position_max_My_bending = value; }
        }
        public Point3d position_Mz_maxBendingZ
        {
            get { return position_max_Mz_bending; }
            set { position_max_Mz_bending = value; }
        }
        
        // load case, in which the max has appear
        public int Loadcase_max_Fx_compression
        {
            get { return loadcase_max_Fx_compression; }
            set { loadcase_max_Fx_compression = value; }
        }
        public int loadcase_Fx_maxTension
        {
            get { return loadcase_max_Fx_tension; }
            set { loadcase_max_Fx_tension = value; }
        }
        public int loadcase_Fy_maxShearY
        {
            get { return loadcase_max_Fy_shear; }
            set { loadcase_max_Fy_shear = value; }
        }
        public int loadcase_Fz_maxShearZ
        {
            get { return loadcase_max_Fz_shear; }
            set { loadcase_max_Fz_shear = value; }
        }
        public int loadcase_Mx_maxTorsion
        {
            get { return loadcase_max_Mx_torsion; }
            set { loadcase_max_Mx_torsion = value; }
        }
        public int loadcase_My_maxBendingY
        {
            get { return loadcase_max_My_bending; }
            set { loadcase_max_My_bending = value; }
        }
        public int loadcase_Mz_maxBendingZ
        {
            get { return loadcase_max_Mz_bending; }
            set { loadcase_max_Mz_bending = value; }
        }
        #endregion

        #region methods

        #endregion
    }
}
