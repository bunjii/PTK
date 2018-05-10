using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTK
{
    public class ProjectProps
    {
        #region fields
        static public double tolerances = 0.001;
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
