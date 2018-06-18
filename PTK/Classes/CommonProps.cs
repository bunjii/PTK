using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTK
{
    public class CommonProps
    {
        #region fields
        public const double tolerances = 0.001;
        public const string category = "PTK";
        public const string subcat1 = "Assemble";
        public const string subcat2 = "Materialize";
        public const string subcat3 = "Detail";
        public const string subcat4 = "Structure";
        public const string subcat5 = "Utility";
        public const string subcat6 = "DetailDescriptions";
        public const string initialMessage = "PTK Ver.0.5";
        // public const string 
        #endregion

        #region constructors
        #endregion

        #region properties
        #endregion

        #region methods
        // simple decimal separator checker
        public static string FindDecimalSeparator()
        {
            string txtFindLocale = string.Format("{0}", 1.1);
            
            string message = "";

            if (txtFindLocale == "1.1") message += "period";
            else if (txtFindLocale == "1,1") message += "comma";
            else message += txtFindLocale;

            return message;
        }
        #endregion
        
    }


}
