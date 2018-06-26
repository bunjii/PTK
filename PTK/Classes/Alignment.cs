using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;

namespace PTK
{

    public class Alignment
    {
        #region fields
        public string Name { get; private set; }
        //public Vector3d Rotation { get; set; } = new Vector3d(0, 0, 1);
        //public Vector2d Transformation { get; set; } = new Vector2d(0, 0);
        //private Point3d alignToPoint = new Point3d(0, 0, 0);
        public double OffsetY { get; private set; } = 0;
        public double OffsetZ { get; private set; } = 0;
        public double RotationAngle { get; private set; } = 0;
        #endregion

        #region constructors
        public Alignment()
        {
            Name = "N/A";
        }
        public Alignment(String _name, Vector3d _alignRotation, double _offsetY, double _offsetZ)// 0Align by rotational vector3d and deconstructed transformational vector
        {
            //alignmethod = 0;
            Name = _name;
            //Rotation = _alignRotation;
            Vector3d temp = new Vector3d(0, 0, 1);
            //RotationAngle = Rhino.Geometry.Vector3d.VectorAngle(Rotation, temp);
            //Transformation = new Vector2d(_offsetY, _offsetZ);
            OffsetZ = _offsetZ;
            OffsetY = _offsetY;
        }
        public Alignment(String _name, Vector3d _alignRotation, Vector3d _alignTransformation)  // 1 Align by rotational and transformational vector. Not yet finnished
        {
            //alignmethod = 1;
            Name = _name;
        }
        public Alignment(String _name, Point3d _pt, double _offsetY, double _offsetZ)// 0Align by rotational vector3d and deconstructed transformational vector
        {
            //alignmethod = 2;
            Name = _name;
            //Vector3d temp = new Vector3d(0, 0, 1);
            //alignToPoint = _pt;
            //Transformation = new Vector2d(_offsetY, _offsetZ);
            OffsetZ = _offsetZ;
            OffsetY = _offsetY;
        }
        // DDL added on 2nd Apr for testing purpose.
        public Alignment(String _name, double _offsetY, double _offsetZ, double _rotationAngle)
        {
            OffsetY = _offsetY;
            OffsetZ = _offsetZ;
            RotationAngle = _rotationAngle;
        }
        // DDL added on 2nd Apr for testing purpose.
        public Alignment(double _offsetY, double _offsetZ, double _rotationAngle)
        {
            OffsetY = _offsetY;
            OffsetZ = _offsetZ;
            RotationAngle = _rotationAngle;
        }

        #endregion

        #region properties
        #endregion

        #region methods

        //public void RotationVectorToPoint(Point3d pt)
        //{
        //    //Line ln = new Line(pt, alignToPoint);
        //    Vector3d vt = new Vector3d(pt.X - alignToPoint.X, pt.Y - alignToPoint.Y, pt.Z - alignToPoint.Z);
        //    Rotation = vt;

        //}
        public Alignment DeepCopy()
        {
            return (Alignment)base.MemberwiseClone();
        }
        public override string ToString()
        {
            string info;
            info = "<Alignment> Name:" + Name + 
                " OffsetY:" + OffsetY.ToString() + 
                " OffsetZ:" + OffsetZ.ToString() + 
                " RotationAngle:" + RotationAngle.ToString();
            return info;
        }
        public bool IsValid()
        {
            return Name != "N/A";
        }
        #endregion
    }

    public class GH_Alignment : GH_Goo<Alignment>
    {
        public GH_Alignment() { }
        public GH_Alignment(GH_Alignment other) : base(other.Value) { this.Value = other.Value.DeepCopy(); }
        public GH_Alignment(Alignment sec) : base(sec) { this.Value = sec; }
        public override bool IsValid => base.m_value.IsValid();

        public override string TypeName => "Alignment";

        public override string TypeDescription => "Deformation and movement of section shape";

        public override IGH_Goo Duplicate()
        {
            return new GH_Alignment(this);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class Param_Alignment : GH_PersistentParam<GH_Alignment>
    {
        public Param_Alignment() : base(new GH_InstanceDescription("Alignment", "Align", "Deformation and movement of section shape", CommonProps.category, CommonProps.subcate0)) { }

        protected override System.Drawing.Bitmap Icon { get { return null; } }  //クラスにアイコンを付けたい場合はここ

        public override Guid ComponentGuid => new Guid("76E8567B-EBBD-49F0-A30E-1069F4D92045");

        protected override GH_GetterResult Prompt_Plural(ref List<GH_Alignment> values)
        {
            return GH_GetterResult.success;
        }

        protected override GH_GetterResult Prompt_Singular(ref GH_Alignment value)
        {
            return GH_GetterResult.success;
        }
    }



}
