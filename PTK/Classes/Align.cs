using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTK
{
    public class Align
    {
        #region fields
        private static int idCount = 2000;  //ID Corresponds to the name of the component. 0: Deconstructed vector methdod
        private int alignmethod;
        private Point3d alignToPoint = new Point3d(0, 0, 0);
        public string Name { get; private set; }
        public int ID { get; private set; }
        public Vector3d Rotation { get; set; } = new Vector3d(0, 0, 1);
        public Vector2d Transformation { get; set; } = new Vector2d(0, 0);
        public double OffsetY { get; private set; } = 0;
        public double OffsetZ { get; private set; } = 0;
        public double RotationAngle { get; private set; } = 0;
        #endregion

        #region constructors
        public Align(String _name, Vector3d _alignRotation, double _offsetY, double _offsetZ)// 0Align by rotational vector3d and deconstructed transformational vector
        {
            alignmethod = 0;
            ID = idCount;
            idCount++;
            Name = _name;
            Rotation = _alignRotation;
            Vector3d temp = new Vector3d(0, 0, 1);
            RotationAngle = Rhino.Geometry.Vector3d.VectorAngle(Rotation, temp);
            Transformation = new Vector2d(_offsetY, _offsetZ);
            OffsetZ = _offsetZ;
            OffsetY = _offsetY;
        }
        public Align(String _name, Vector3d _alignRotation, Vector3d _alignTransformation)  // 1 Align by rotational and transformational vector. Not yet finnished
        {
            alignmethod = 1;
            ID = idCount;
            idCount++;
            Name = _name;
        }

        public Align(String _name, Point3d _pt, double _offsetY, double _offsetZ)// 0Align by rotational vector3d and deconstructed transformational vector
        {
            alignmethod = 2;
            ID = idCount;
            idCount++;
            Name = _name;
            Vector3d temp = new Vector3d(0, 0, 1);
            alignToPoint = _pt;
            Transformation = new Vector2d(_offsetY, _offsetZ);
            OffsetZ = _offsetZ;
            OffsetY = _offsetY;
        }

        // DDL added on 2nd Apr for testing purpose.
        public Align(double _offsetY, double _offsetZ, double _rotationAngle)
        {
            OffsetY = _offsetY;
            OffsetZ = _offsetZ;
            RotationAngle = _rotationAngle;
        }

        #endregion
        #region properties
        #endregion
        #region methods

        public void RotationVectorToPoint(Point3d pt)
        {
            //Line ln = new Line(pt, alignToPoint);
            Vector3d vt = new Vector3d(pt.X - alignToPoint.X, pt.Y - alignToPoint.Y, pt.Z - alignToPoint.Z);
            Rotation = vt;

        }



        #endregion
    }
}
