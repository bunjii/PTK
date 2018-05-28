using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTK
{
    public class Forces
    {
        #region fields
        private List<double> fx;
        private List<double> fy;
        private List<double> fz;
        private List<double> mx;
        private List<double> my;
        private List<double> mz;


        #endregion

        #region constructors
        #endregion

        #region properties
        public List<double> FX
        {
            get
            {
                return fx;
            }
            set
            {
                fx = value;
            }
        }
        public List<double> FY
        {
            get
            {
                return fy;
            }
            set
            {
                fy = value;
            }
        }
        public List<double> FZ
        {
            get
            {
                return fz;
            }
            set
            {
                fz = value;
            }
        }
        public List<double> MX
        {
            get
            {
                return mx;
            }
            set
            {
                mx = value;
            }
        }
        public List<double> MY
        {
            get
            {
                return my;
            }
            set
            {
                my = value;
            }
        }
        public List<double> MZ
        {
            get
            {
                return mz;
            }
            set
            {
                mz = value;
            }
        }


        #endregion

        #region methods

        #endregion
    }
}
