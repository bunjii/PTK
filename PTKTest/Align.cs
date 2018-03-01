using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace PTK
{
    public class Align
    {
        #region fields

        private static int alignIDCount = 2000;  //ID Corresponds to the name of the component. 0: Deconstructed vector methdod
        private string alignName;
        private int alignID;

        private int alignmethod;
        private Vector3d alignRotation;
        private Vector2d alignTransformation;
        private double rotationangle;
        private double offsetY;
        private double offsetZ;
        #endregion
        #region constructors
    
        public Align(String _name, Vector3d _alignRotation, double _offsetY, double _offsetZ)// 0Align by rotational vector3d and deconstructed transformational vector
        {
            alignmethod = 0;
            alignID = alignIDCount;
            alignIDCount++;
            alignName = _name;
            alignRotation = _alignRotation;
            Vector3d temp = new Vector3d(0, 0,1);
            rotationangle =  Rhino.Geometry.Vector3d.VectorAngle(alignRotation, temp); 
            alignTransformation = new Vector2d(_offsetY, _offsetZ);
            offsetZ = _offsetZ;
            offsetY = _offsetY;
            

        }
       public Align(String _name, Vector3d _alignRotation, Vector3d _alignTransformation)  // 1 Align by rotational and transformational vector. Not yet finnished
        {
            alignmethod = 1;
            alignID = alignIDCount;
            alignIDCount++;
            alignName = _name;
        }



        #endregion
        #region properties
        #endregion
        #region methods
        #endregion
    }
}
