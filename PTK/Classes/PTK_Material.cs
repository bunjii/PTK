﻿// alphanumerical order for namespaces please
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// using Karamba.Loads ;

namespace PTK
{
    public class PTK_Material
    {
        #region fields
        private string materialName;
        private int id;
        private List<int> elemIds = new List<int>();
        private PTK_MatProps properties;
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
        public PTK_Material(PTK_MatProps _properties)
        {
            id = -999; 
            materialName = "N/A";
            properties = _properties;
        }
        #endregion

        #region properties
        public string MatName
        {
            get { return materialName; }
            set { materialName = value; }
        }
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public ReadOnlyCollection<int> ElemIds
        {
            get { return elemIds.AsReadOnly(); }
        }
        public PTK_MatProps Properties
        {
            get { return properties; }
            set { properties = value; }
        }

        #endregion

        #region methods
        public void AddElemId(int elemId)
        {
            this.elemIds.Add(elemId);
        }

        public static PTK_Material FindMatById(List<PTK_Material> _mats, int _mid)
        {
            PTK_Material tempMat;
            tempMat = _mats.Find(m => m.Id == _mid);

            return tempMat;
        }
        #endregion
    }
}
