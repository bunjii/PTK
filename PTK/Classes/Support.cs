using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Rhino.Geometry;

namespace PTK
{
    public class Support
    {
        #region fields
        public int LoadCase { get; private set; } = 0;
        public Plane FixingPlane { get; private set; }
        public List<bool> Conditions { get; private set; }
        #endregion

        #region constructors
        public Support()
        {
            FixingPlane = new Plane();
            Conditions = new List<bool>();
        }
        public Support(int _loadCase, Plane _fixingPlane, List<bool> _conditions) 
        {
            LoadCase = _loadCase;
            FixingPlane = _fixingPlane;
            Conditions = _conditions;
        }
        #endregion

        #region properties
        #endregion

        #region methods
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

        #endregion
    }
}
