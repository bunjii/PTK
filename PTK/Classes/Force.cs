using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTK
{
    public class Force
    {
        #region fields
        public int LoadCase { get; private set; } = 0;
        public double FX { get; private set; }
        public double FY { get; private set; }
        public double FZ { get; private set; }
        public double MX { get; private set; }
        public double MY { get; private set; }
        public double MZ { get; private set; }
        #endregion

        #region constructors
        public Force()
        {
            FX = 0.0;
            FY = 0.0;
            FZ = 0.0;
            MX = 0.0;
            MY = 0.0;
            MZ = 0.0;
        }
        public Force(int _loadCase, double _fx, double _fy, double _fz, double _mx, double _my, double _mz)
        {
            LoadCase = _loadCase;
            FX = _fx;
            FY = _fy;
            FZ = _fz;
            MX = _mx;
            MY = _my;
            MZ = _mz;
        }
        #endregion

        #region properties
        #endregion

        #region methods
        #endregion
    }
}
