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

        private string materialName;            //either glulam, either timber
        // for glulam according LIMTREBOKA
        
        private double fmgk;
        private double ft0gk;
        private double ft90gk;
        private double fc0gk;
        private double fc90gk;
        private double fvgk;
        private double frgk;
        private double e0gmean;
        private double e0g05;
        private double e90gmean;
        private double e90g05;
        private double ggmean;
        private double gg05;
        private double grgmean;
        private double grg05;
        // private double Qgk;
        // private double Qgmean;
        private double rhogk;       // density characteristic value
        private double rhogmean;    // density mean value
        private string txtHash = "";
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
            materialName = _materialName; 
            fmgk = _fmgk;
            ft0gk = _ft0gk;
            ft90gk = _ft90gk;
            fc0gk = _fc0gk;
            fc90gk = _fc90gk;
            fvgk = _fvgk;
            frgk = _frgk;
            e0gmean = _e0gmean;
            e0g05 = _e0g05;
            e90gmean = _e90gmean;
            e90g05 = _e90g05;
            ggmean = _ggmean;
            gg05 = _gg05;
            grgmean = _grgmean;
            grg05 = _grg05;
            // double Qgk = _Qgk;
            // double Qgmean = _Qgmean;
            rhogk = _rhogk;          // density
            rhogmean = _rhogmean;    // density

            txtHash = CreateHashFromMP(this);

        }
        #endregion

        #region properties
        public string MaterialName { get { return materialName; } set { materialName = value; } }
        public double Fmgk { get { return fmgk; } set { fmgk = value; } }
        public double Ft0gk { get { return ft0gk; } set { ft0gk = value; } }
        public double Ft90gk { get { return ft90gk; } set { ft90gk = value; } }
        public double Fc0gk { get { return fc0gk; } set { fc0gk = value; } }
        public double Fc90gk { get { return fc90gk; } set { fc90gk = value; } }
        public double Fvgk { get { return fvgk; } set { fvgk = value; } }
        public double Frgk { get { return frgk; } set { frgk = value; } }
        public double EE0gmean { get { return e0gmean; } set { e0gmean = value; } }
        public double EE0g05 { get { return e0g05; } set { e0g05 = value; } }
        public double EE90gmean { get { return e90gmean; } set { e90gmean = value; } }
        public double EE90g05 { get { return e90g05; } set { e90g05 = value; } }
        public double GGgmean { get { return ggmean; } set { ggmean = value; } }
        public double GGg05 { get { return gg05; } set { gg05 = value; } }
        public double GGrgmean { get { return grgmean; } set { grgmean = value; } }
        public double GGrg05 { get { return grg05; } set { grg05 = value; } }
        // public double QQgk { get { return Qgk; } set { Qgk = value; } }
        // public double QQgmean { get { return Qgmean; } set { Qgmean = value; } }
        public double Rhogk { get { return rhogk; } set { rhogk = value; } }
        public double Rhogmean { get { return rhogmean; } set { rhogmean = value; } }
        public string TxtHash { get { return txtHash; } }
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
