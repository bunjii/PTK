using System;
using System.Collections.Generic;
using Rhino.Geometry;

namespace PTK
{
    public class Supports
    {
        #region fields
        private string supports_tag;
        private int supports_id;

        private Point3d supports_point;
        private List<bool> rotations;
        private List<bool> translations;

        #endregion

        #region constructors
        public Supports(string _supports_tag, Point3d _supports_point, List<bool> _rotations, List<bool> _translations)
        {
            supports_tag = _supports_tag; // inheriting  Class
            supports_id = -999; // inheriting  Class
            rotations = _rotations; // inheriting  Class
            translations = _translations; // inheriting  Class
            supports_point = _supports_point;
        }
        #endregion

        #region properties
        public string Supports_Tag { get { return supports_tag; } set { supports_tag = value; } }
        public int Supports_ID { get { return supports_id; } set { supports_id = value; } }
        
        public Point3d Supports_point { get { return supports_point; } set { supports_point = value; } }

        public List<bool> Rotations { get { return rotations; } set { rotations = value; } }
        public List<bool> Translations { get { return translations; } set { translations = value; } }


        #endregion

        #region methods

        #endregion
    }
}
