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
        public string Name { get; private set; }
        public double Width { get; private set; } = 100;
        public double Height { get; private set; } = 100;
        public Material Material { get; private set; }
        #endregion

        #region constructors
        public Section()
        {
            Name = "N/A";
            Material = new Material();
        }
       public Section(string _name, double _width, double _height)
        {
            Name = _name;
            Width = _width;
            Height = _height;
            Material = new Material();
        }
        public Section(string _name, double _width, double _height, Material _material)
        {
            Name = _name;
            Width = _width;
            Height = _height;
            Material = _material;
        }
        #endregion

        #region properties
        #endregion

        #region methods
        public override string ToString()
        {
            string info;
            info = "Name:" + Name + " Width:" + Width.ToString() + " Height:" + Height.ToString() + " Material:" + Material.Name;
            return info; 
        }
        #endregion
    }

}
