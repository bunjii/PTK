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

        public static List<bool> ArrayToList(bool[] _boolArray)
        {
            List<bool> _boolList = new List<bool>();
            for (int i=0;i<6; i++)
            {
                _boolList.Add(_boolArray[i]);
            }
            return _boolList;
        }

        public static bool[] StringToArray(string _boolStr)
        {
            bool[] _returnArray = new bool[6];
            char[] _tempChar;

            _tempChar = _boolStr.ToCharArray();
            for (int i = 0; i < 6; i++)
            {
                bool _tempBool;
                if (_tempChar[i] == '0')
                {
                    _tempBool = false;
                }
                else
                {
                    _tempBool = true;
                }

                _returnArray[i] = _tempBool;
            }

            return _returnArray;
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
