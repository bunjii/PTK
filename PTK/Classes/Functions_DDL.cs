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
    class Functions_DDL
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

        public static string CreateHash(string _str)
        {

            byte[] _byteVal = Encoding.UTF8.GetBytes(_str);

            // create SHA256 value
            SHA256 _sha256val = new SHA256CryptoServiceProvider();
            byte[] _hashVal = _sha256val.ComputeHash(_byteVal);

            // byte -> string
            StringBuilder _hashedTxt = new StringBuilder();
            for (int i = 0; i < _hashVal.Length; i++)
            {
                _hashedTxt.Append(_hashVal[i].ToString("X2"));
            }

            return _hashedTxt.ToString();
        }

        

        public static void CloneKarambaModel(ref Karamba.Models.Model _model)
        {
            // clone model to avoid side effects
            _model = (Karamba.Models.Model)_model.Clone();

            // clone its elements to avoid side effects
            _model.cloneElements();

            // clone the feb-model to avoid side effects
            _model.deepCloneFEModel();
        }

        // ### private functions ###

        
        private static Line CurveToLine(Curve _crv)
        {
            Point3d pt0 = _crv.PointAtStart;
            Point3d pt1 = _crv.PointAtEnd;
            Line result = new Line(pt0, pt1);
            return result;
        }
    }
}