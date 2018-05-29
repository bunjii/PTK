using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Rhino.Geometry;

namespace PTK
{
    public class Support
    {
        #region fields
        static int idCount = 0;

        private int id;
        private int lCase;
        private List<bool> conditions;
        private Plane pln;
        #endregion

        #region constructors
        public Support(int _loadCase, Plane _supPln, List<bool> _supConditions) 
        {
            id = -999;
            lCase = _loadCase;
            conditions = _supConditions; 
            pln = _supPln;
        }
        #endregion

        #region properties
        public int Id { get { return id; } set { id = value; } }
        public int LCase { get { return lCase; } set { lCase = value; } }
        public Plane Pln { get { return pln; } set { pln = value; } }
        public ReadOnlyCollection<bool> Conditions
        {
            get { return conditions.AsReadOnly(); }
        }
        #endregion

        #region methods
        public void UpdateConditions(List<bool> _conditions)
        {
            this.conditions = _conditions;
        }

        //public static List<bool> ArrayToList(bool[] _boolArray)
        //{
        //    List<bool> _boolList = new List<bool>();
        //    for (int i=0;i<6; i++)
        //    {
        //        _boolList.Add(_boolArray[i]);
        //    }
        //    return _boolList;
        //}

        public static bool[] StringToArray(string _boolStr)
        {
            List<bool> _returnArray = new List<bool>();
            char[] _tempChar;

            _tempChar = _boolStr.ToCharArray();
            foreach(char c in _tempChar)
            {
                bool _tempBool;
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
            this.id = idCount;
            idCount++;
        }

        public static void ResetIDCount()
        {
            idCount = 0;
        }
        #endregion
    }
}
