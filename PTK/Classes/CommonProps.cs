using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTK
{
    public static class CommonProps
    {
        #region fields
        public static double tolerances = 0.001;
        public static string category = "PTK";
        public static string subcat1 = "Assemble";
        public static string subcat2 = "Materialize";
        public static string subcat3 = "Detail";
        public static string subcat4 = "Structure";
        public static string subcat5 = "Utility";
        public static string initialMessage = "PTK Ver.0.5";
        #endregion

        #region constructors
        #endregion

        #region properties
        #endregion

        #region methods
        // simple decimal separator checker
        public static DecimalSeparator FindDecimalSeparator()
        {
            string txtFindLocale = string.Format("{0}", 1.1);

            if (txtFindLocale == "1.1") return DecimalSeparator.period;
            else if (txtFindLocale == "1,1") return DecimalSeparator.comma;
            else return DecimalSeparator.error;
        }

        
        public static double ConversionUnit(Rhino.UnitSystem _toUnitSystem)
        {
            Rhino.RhinoDoc doc = Rhino.RhinoDoc.ActiveDoc;
            Rhino.UnitSystem fromUnitSystem = doc.ModelUnitSystem;
            return Rhino.RhinoMath.UnitScale(fromUnitSystem, _toUnitSystem);
        }
        #endregion
        
    }


    public enum DecimalSeparator
    {
        error,period,comma
    }


}
