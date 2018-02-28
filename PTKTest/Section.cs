using System;
using System.Collections.Generic;
using Rhino.Geometry;



using System.Windows.Forms;

namespace PTK
{
    

    public class Section
    {
        #region fields
        private string tag;
        private int id;
        private Vector3d offset;
        private double width = 100;
        private double height = 100;
        #endregion

        #region constructors
        public Section(string _tag, double _width, double _height, Vector3d _offset)
        {
            tag = _tag; // inheriting Section Class
            id = -999; // inheriting Section Class
            offset = _offset; // inheriting Section Class
            width = _width;
            height = _height;
        }
        #endregion

        #region properties
        public double Width { get { return width; }  }
        public double Height { get { return height; }  }
        public string Tag { get { return tag; } set { tag = value; } }
        public int ID { get { return id; } set { id = value; } }
        public Vector3d Offset { get { return offset; } set { offset = value; } }
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
