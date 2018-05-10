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

        private static int idCount = 2000;  //ID Corresponds to the name of the component. 0: Deconstructed vector methdod
        private string name;
        private int id;

        private int alignmethod;
        private Vector3d rotation = new Vector3d(0, 0, 1);
        private Vector2d transformation = new Vector2d(0, 0);
        private double rotationangle = 0;
        private double offsetY = 0;
        private double offsetZ = 0;
        private Point3d alignToPoint = new Point3d(0, 0, 0);
        #endregion
        #region constructors
    
        

        public Align(String _name, Vector3d _alignRotation, double _offsetY, double _offsetZ)// 0Align by rotational vector3d and deconstructed transformational vector
        {
            alignmethod = 0;
            id = idCount;
            idCount++;
            name = _name;
            rotation = _alignRotation;
            Vector3d temp = new Vector3d(0, 0,1);
            rotationangle =  Rhino.Geometry.Vector3d.VectorAngle(rotation, temp); 
            transformation = new Vector2d(_offsetY, _offsetZ);
            offsetZ = _offsetZ;
            offsetY = _offsetY;
            

        }
       public Align(String _name, Vector3d _alignRotation, Vector3d _alignTransformation)  // 1 Align by rotational and transformational vector. Not yet finnished
        {
            alignmethod = 1;
            id = idCount;
            idCount++;
            name = _name;
        }

        public Align(String _name, Point3d _pt, double _offsetY, double _offsetZ)// 0Align by rotational vector3d and deconstructed transformational vector
        {
            alignmethod = 2;
            id = idCount;
            idCount++;
            name = _name;
            Vector3d temp = new Vector3d(0, 0, 1);
            alignToPoint = _pt;
            transformation = new Vector2d(_offsetY, _offsetZ);
            offsetZ = _offsetZ;
            offsetY = _offsetY;
        }

        // DDL added on 2nd Apr for testing purpose.
        public Align(double _offsetY, double _offsetZ, double _rotationAngle)
        {
            offsetY = _offsetY;
            offsetZ = _offsetZ;
            rotationangle = _rotationAngle;

        }

        #endregion
        #region properties

        public string Name { get { return name; } set { name = value; } }
        public int ID { get { return id; } }
        public Vector3d Rotation { get { return rotation; } set { rotation = value; } }
        public Vector2d Transformation { get { return transformation; } set { transformation = value; } }
        public double OffsetY { get { return offsetY; } set { offsetY = value; } }
        public double OffsetZ { get { return offsetZ; } set { offsetZ = value; } }
        public double RotationAngle
        {
            get { return rotationangle; }
            set { rotationangle = value; }
        }

        #endregion
        #region methods

        public void rotationVectorToPoint(Point3d pt)
        {
            Line ln = new Line(pt, alignToPoint);

            Vector3d vt = new Vector3d(pt.X - alignToPoint.X, pt.Y - alignToPoint.Y, pt.Z - alignToPoint.Z);
            rotation = vt;
            
        }



        #endregion
    }
}
