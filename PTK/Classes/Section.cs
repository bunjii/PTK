// alphanumerical order for namespaces please
using System;
using System.Collections.Generic;
using System.Security.Cryptography; // needed to create hash
using System.Windows.Forms;

using Rhino.Geometry;

namespace PTK
{
    public class Section
    {
        #region fields
        private static int sectionIDCount = 2000;
        public int Id { get; set; }
        public string SectionName { get; private set; }
        public double Width { get; private set; } = 100;
        public double Height { get; private set; } = 100;
        public string TxtHash { get; private set; } = "";
        public List<int> ElemIds { get; private set; } = new List<int>();
        #endregion

        #region constructors
        public Section(string _name, double _width, double _height)
        {
            Id = sectionIDCount;
            sectionIDCount++;

            SectionName = _name;
            Width = _width;
            Height = _height;

            TxtHash = CreateHashFromSP(this);
        }
        #endregion

        #region properties
        #endregion

        #region methods
        private static string CreateHashFromSP(Section _sec) // SP : short for "section properties"
        {
            string _key = _sec.Height.ToString() + _sec.Width.ToString() + _sec.SectionName ;
            return Functions_DDL.CreateHash(_key);
        }

        public void AddElemId(int elemId)
        {
            ElemIds.Add(elemId);
        }

        public static Section FindSecById(List<Section> _secs, int _sid)
        {
            Section tempSec;
            tempSec = _secs.Find(s => s.Id == _sid);

            return tempSec;
        }
        #endregion
    }

}
