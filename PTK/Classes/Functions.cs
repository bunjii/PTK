using feb;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Karamba.Elements;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PTK
{
    class Functions
    {

        // ### public functions ###

        public static string ConvertCommaToPeriodDecimal(string _txt, bool _reverse = false)
        {
            if (!_reverse)
            {
                return _txt.Replace(',', '.');  //Comma to Period
            }
            else
            {
                return _txt.Replace('.', ',');  //Period to Comma
            }
        }

    }
}