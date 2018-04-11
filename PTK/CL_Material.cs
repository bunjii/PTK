using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Karamba.Loads ;

namespace PTK
{
    public class Material
    {

        #region fields
        private string materialName;
        private int materialId;

        private Material_properties properties;
        #endregion 

        #region constructors
        public Material(string _materialName, int _materialId, Material_properties _properties)
        {
            materialName = _materialName; // inheriting  Class
            materialId = -999; // inheriting  Class
            properties = _properties;
        }
        #endregion

        #region properties
        public string MaterialName { get { return materialName; } set { materialName = value; } }
        public int MaterialId { get { return materialId; } set { materialId = value; } }

        public Material_properties Properties { get { return properties; } set { properties = value; } }

        #endregion

        #region methods

        #endregion
    }
}

