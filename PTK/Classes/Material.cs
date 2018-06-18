// alphanumerical order for namespaces please
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTK
{
    public class Material
    {
        #region fields
        public string Name { get; private set; }
        public MaterialStructuralProp StructuralProp { get; private set; }
        #endregion

        #region constructors
        public Material()
        {
            Name = "N/A";
            StructuralProp = new MaterialStructuralProp();
        }
        public Material(string _name)
        {
            Name = _name;
            StructuralProp = new MaterialStructuralProp();
        }
        public Material(string _name, MaterialStructuralProp _structuralProp)
        {
            Name = _name;
            StructuralProp = _structuralProp;
        }
        #endregion

        #region properties
        #endregion

        #region methods
        public override string ToString()
        {
            string info;
            info = "Name:" + Name + " StructuralProp.Name:" + StructuralProp.Name;
            return info;
        }
        #endregion
    }
}

