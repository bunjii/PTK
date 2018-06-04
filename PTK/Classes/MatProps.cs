// alphanumerical order for namespaces please
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography; // needed to create hash
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PTK
{
    public class MatProps
    {

        #region fields
        public string MaterialName { get; set; }
        public double Fmgk { get; set; }
        public double Ft0gk { get; set; }
        public double Ft90gk { get; set; }
        public double Fc0gk { get; set; }
        public double Fc90gk { get; set; }
        public double Fvgk { get; set; }
        public double Frgk { get; set; }
        public double EE0gmean { get; set; }
        public double EE0g05 { get; set; }
        public double EE90gmean { get; set; }
        public double EE90g05 { get; set; }
        public double GGgmean { get; set; }
        public double GGg05 { get; set; }
        public double GGrgmean { get; set; }
        public double GGrg05 { get; set; }
        // public double QQgk { get; set; }
        // public double QQgmean { get; set; }
        public double Rhogk { get; set; }
        public double Rhogmean { get; set; }
        public string TxtHash { get; } = "";
        #endregion

        #region constructors
        public MatProps(
            string _materialName,
            double _fmgk,
            double _ft0gk,
            double _ft90gk,
            double _fc0gk,
            double _fc90gk,
            double _fvgk,
            double _frgk,
            double _e0gmean,
            double _e0g05,
            double _e90gmean,
            double _e90g05,
            double _ggmean,
            double _gg05,
            double _grgmean,
            double _grg05,
            // double _Qgk,
            // double _Qgmean,
            double _rhogk,      // density
            double _rhogmean    // density
        )
        {
            MaterialName = _materialName; 
            Fmgk = _fmgk;
            Ft0gk = _ft0gk;
            Ft90gk = _ft90gk;
            Fc0gk = _fc0gk;
            Fc90gk = _fc90gk;
            Fvgk = _fvgk;
            Frgk = _frgk;
            EE0gmean = _e0gmean;
            EE0g05 = _e0g05;
            EE90gmean = _e90gmean;
            EE90g05 = _e90g05;
            GGgmean = _ggmean;
            GGg05 = _gg05;
            GGrgmean = _grgmean;
            GGrg05 = _grg05;
            // double Qgk = _Qgk;
            // double Qgmean = _Qgmean;
            Rhogk = _rhogk;          // density
            Rhogmean = _rhogmean;    // density

            TxtHash = CreateHashFromMP(this);

        }
        #endregion

        #region properties
        #endregion

        #region methods
        private static string CreateHashFromMP(MatProps _m)
        {
            string _key = "";

            _key += _m.Fmgk + _m.Ft0gk + _m.Ft90gk + _m.Fc0gk + _m.Fc90gk + _m.Fvgk + _m.Frgk
                + _m.EE0gmean + _m.EE0g05 + _m.EE90gmean + _m.EE90g05
                + _m.GGgmean + _m.GGg05 + _m.GGrgmean + _m.GGrg05 + _m.Rhogk + _m.Rhogmean;

            return Functions_DDL.CreateHash(_key);
        }
        #endregion
    }
}
