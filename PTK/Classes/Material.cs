// alphanumerical order for namespaces please
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// using Karamba.Loads ;

namespace PTK
{
    public class Material
    {

        #region fields
        public string MatName { get; set; }
        public int Id { get; set; }
        public List<int> ElemIds { get; private set; } = new List<int>();
        public MatProps Properties { get; set; }
        #endregion

        #region constructors
        /*
        public Material(string _materialName, int _materialId, MatProps _properties)
        {
            materialName = _materialName; // inheriting  Class
            id = -999; // inheriting  Class
            properties = _properties;
        }
        */
        public Material(MatProps _properties)
        {
            Id = -999; 
            MatName = "N/A";
            Properties = _properties;
        }
        #endregion

        #region properties
        #endregion

        #region methods
        public void AddElemId(int elemId)
        {
            this.ElemIds.Add(elemId);
        }

        public static Material FindMatById(List<Material> _mats, int _mid)
        {
            Material tempMat;
            tempMat = _mats.Find(m => m.Id == _mid);

            return tempMat;
        }
        #endregion
    }
}

