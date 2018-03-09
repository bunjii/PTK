using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTK
{
    public class Material_properties
    {

        #region fields

        private string materialName;
        // for glulam according LIMTREBOKA
        private double fmgk;
        private double ft0gk;
        private double ft90gk;
        private double fvgk;
        private double frgk;
        private double E0gmean;
        private double E0g05;
        private double E90gmean;
        private double E90g05;
        private double Ggmean;
        private double Gg05;
        private double Gtgmean;
        private double Grg05;
        private double Qgk;
        private double Qgmean;
        #endregion

        #region constructors
        public Material_properties(
         string _materialName,
         double _fmgk, 
         double _ft0gk,
         double _ft90gk,
         double _fvgk,
         double _frgk,
         double _E0gmean,
         double _E0g05,
         double _E90gmean,
         double _E90g05,
         double _Ggmean,
         double _Gg05,
         double _Gtgmean,
         double _Grg05,
         double _Qgk,
         double _Qgmean)
        {

            string materialName = _materialName;// inheriting  Class
            double fmgk=_fmgk;
            double ft0gk = _ft0gk;
            double ft90gk = _ft90gk;
            double fvgk = _fvgk;
            double frgk = _frgk;
            double E0gmean = _E0gmean;
            double E0g05 = _E0g05;
            double E90gmean = _E90gmean;
            double E90g05 = _E90g05;
            double Ggmean = _Ggmean;
            double Gg05 = _Gg05;
            double Gtgmean = _Gtgmean;
            double Grg05 = _Grg05;
            double Qgk = _Qgk;
            double Qgmean = _Qgmean;
        }
        #endregion

        #region properties
        public string MaterialName { get { return materialName; } set { materialName = value; } }
        public double Fmgk { get { return fmgk; } set { fmgk = value; } }
        public double Ft0gk { get { return ft0gk; } set { ft0gk = value; } }
        public double F90gk { get { return ft90gk; } set { ft90gk = value; } }
        public double Fvgk { get { return fvgk; } set { fvgk = value; } }
        public double Frgk { get { return frgk; } set { frgk = value; } }
        public double EE0gmean { get { return E0gmean; } set { E0gmean = value; } }
        public double EE0g05 { get { return E0g05; } set { E0g05 = value; } }
        public double EE90gmean { get { return E90gmean; } set { E90gmean = value; } }
        public double EE90g05 { get { return E90g05; } set { E90g05 = value; } }
        public double GGgmean { get { return Ggmean; } set { Ggmean = value; } }
        public double GGg05 { get { return Gg05; } set { Gg05 = value; } }
        public double GGtgmean { get { return Gtgmean; } set { Gtgmean = value; } }
        public double GGrg05 { get { return Grg05; } set { Grg05 = value; } }
        public double QQgk { get { return Qgk; } set { Qgk = value; } }
        public double QQgmean { get { return Qgmean; } set { Qgmean = value; } }



        #endregion

        #region methods

        #endregion
    }
}
