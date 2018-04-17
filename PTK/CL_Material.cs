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
        private int matId;

        private MatProps properties;
        #endregion 

        #region constructors
        public Material(string _materialName, int _materialId, MatProps _properties)
        {
            materialName = _materialName; // inheriting  Class
            matId = -999; // inheriting  Class
            properties = _properties;
        }
        public Material(MatProps _properties)
        {
            matId = -999; // inheriting  Class
            materialName = "N/A";
            properties = _properties;
        }
        #endregion

        #region properties
        public string MaterialName { get { return materialName; } set { materialName = value; } }
        public int MaterialId { get { return matId; } set { matId = value; } }

        public MatProps Properties { get { return properties; } set { properties = value; } }

        #endregion

        #region methods

        #endregion
    }
}

