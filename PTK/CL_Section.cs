using System;
using System.Collections.Generic;
using Rhino.Geometry;



using System.Windows.Forms;

//git_test_text

namespace PTK
{
    

    public class Section
    {
        #region fields
        private static int sectionIDCount = 2000;  //ID Corresponds to the name of the component
        private string sectionName;
        private int sectionID;
        
        private double width = 100;
        private double height = 100;
        #endregion

        #region constructors
        public Section(string _name, double _width, double _height)
        {
            sectionID = sectionIDCount;
            sectionIDCount++;
            sectionName = _name;
            width = _width;
            height = _height;
        }
        #endregion

        #region properties
        public double Width { get { return width; }  }
        public double Height { get { return height; }  }
        public string SectionName { get { return sectionName; } set { sectionName = value; } }
        public int SectionID { get { return sectionID; } set { sectionID = value; } }
        #endregion

        #region methods

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
