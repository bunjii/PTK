// alphanumerical order for namespaces please
using System;
using System.Collections.Generic;
using System.Security.Cryptography; // needed to create hash
using System.Windows.Forms;

using Rhino.Geometry;

namespace PTK
{
    public class PTK_Section
    {
        #region fields
        private static int sectionIDCount = 2000;  
        // ID Corresponds to the name of the component
        // Bunji: need to understand this and its usage.
        private string sectionName;
        private int id;
        
        private double width;
        private double height;
        private string txtHash = "";

        private double analysis_area;
        private List<double> analysis_moment_of_inertia;
        private List<double> analysis_section_modulus;
        private List<double> analysis_radius_of_gyration;



        private List<int> elemIds = new List<int>();
        #endregion

        #region constructors
        public PTK_Section(string _name, double _width, double _height)
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
        public double Width { get { return width; } set { width = value; } }
        public double Height { get { return height; } set { height = value; } }
        public string SectionName { get { return sectionName; } set { sectionName = value; } }
        public int Id { get { return id; } set { id = value; } }
        public string TxtHash { get { return txtHash; } }
        public List<int> ElemIds { get { return elemIds;  } }

        public double Structural_Area { get { return analysis_area; } set { analysis_area = value; } }
        public List<double> Structural_Radius_of_gyration { get { return analysis_radius_of_gyration; } set { analysis_radius_of_gyration = value; }}
        public List<double> Structural_Moment_of_inertia { get { return analysis_moment_of_inertia; } set { analysis_moment_of_inertia = value; } }
        #endregion

        #region methods
        private static string CreateHashFromSP(PTK_Section _sec) // SP : short for "section properties"
        {
            string _key = _sec.Height.ToString() + _sec.Width.ToString() + _sec.SectionName ;
            return Functions_DDL.CreateHash(_key);
        }
        public void AddElemId(int elemId)
        {
            this.elemIds.Add(elemId);
        }

        public static PTK_Section FindSecById(List<PTK_Section> _secs, int _sid)
        {
            PTK_Section tempSec;
            tempSec = _secs.Find(s => s.Id == _sid);

            return tempSec;
        }

        

        #endregion
      
    }

}
