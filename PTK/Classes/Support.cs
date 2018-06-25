using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Rhino.Geometry;

namespace PTK
{
    public class Support
    {

        #region fields
        private static int idCount = 0;
        public int Id { get; private set; }
        public int LCase { get; private set; }
        public Plane Pln { get; private set; }
        public List<bool> Conditions { get; private set; } = new List<bool>();
        private Karamba.Supports.GH_Support krmb_GH_support;

        #endregion

        #region constructors
        public Support(int _loadCase, Plane _supPln, List<bool> _supConditions) 
        {
            Id = -999;
            LCase = _loadCase;
            Conditions = _supConditions; 
            Pln = _supPln;
        }
        public Support(Karamba.Supports.GH_Support _krmb_GH_support)
        {
            Id = -999;
            krmb_GH_support = _krmb_GH_support;

        }
        #endregion

        #region properties
        public Karamba.Supports.GH_Support Krmb_GH_support { get { return krmb_GH_support; } set { krmb_GH_support = value; } }

        #endregion

        #region methods
        public Support Clone()
        {
            return (Support)MemberwiseClone();
        }
        public void UpdateConditions(List<bool> _conditions)
        {
            this.Conditions = _conditions;
        }

        public static bool[] StringToArray(string _boolStr)
        {
            List<bool> _returnArray = new List<bool>();
            char[] _tempChars;
            _tempChars = _boolStr.ToCharArray();

            foreach(char c in _tempChars)
            {
                if (c == '0')
                {
                    _returnArray.Add(false);
                }
                else
                {
                    _returnArray.Add(true);
                }
            }
            return _returnArray.ToArray();
        }

        public void AssignID()
        {
            this.Id = idCount;
            idCount++;
        }

        public static void ResetIDCount()
        {
            idCount = 0;
        }
        #endregion
    }
}
