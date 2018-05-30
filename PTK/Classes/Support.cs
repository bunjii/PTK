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
        #endregion

        #region constructors
        public Support(int _loadCase, Plane _supPln, List<bool> _supConditions) 
        {
            Id = -999;
            LCase = _loadCase;
            Conditions = _supConditions; 
            Pln = _supPln;
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
