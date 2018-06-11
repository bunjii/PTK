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
        // ID Corresponds to the name of the component
        // Bunji: need to understand this and its usage.
        private string sectionName;
        private int id;
        
        private double width = 100;
        private double height = 100;
        private string txtHash = "";

        private List<int> elemIds = new List<int>();
        #endregion

        #region constructors
        public Section(string _name, double _width, double _height)
        {
            id = sectionIDCount;
            sectionIDCount++;
            sectionName = _name;
            width = _width;
            height = _height;

            txtHash = CreateHashFromSP(this);
        }
        #endregion

        #region properties
        public double Width { get { return width; }  }
        public double Height { get { return height; }  }
        public string SectionName { get { return sectionName; } set { sectionName = value; } }
        public int Id { get { return id; } set { id = value; } }
        public string TxtHash { get { return txtHash; } }
        public List<int> ElemIds { get { return elemIds;  } }
        #endregion

        #region methods
        private static string CreateHashFromSP(Section _sec) // SP : short for "section properties"
        {
            string _key = _sec.Height.ToString() + _sec.Width.ToString() + _sec.SectionName ;
            return Functions_DDL.CreateHash(_key);
        }
        public void AddElemId(int elemId)
        {
            this.elemIds.Add(elemId);
        }

        public static Section FindSecById(List<Section> _secs, int _sid)
        {
            Section tempSec;
            tempSec = _secs.Find(s => s.Id == _sid);

            return tempSec;
        }
        #endregion
    }

    /*
    public class SomeNewClass
    {
        #region fields
        #endregion
        #region constructors
        #endregion
        #region properties
        #endregion
        #region methods
        #endregion
    }
    
        private double width;
        private double height;
        private Vector2d offsetVec;
    */
}
