using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;

namespace PTK
{
    namespace PTK
    {
        public class Joint
        {
            #region fields
            public int LoadCase { get; private set; } = 0;
            public Vector3d TranslateSpringAtStart { get; private set; }
            public Vector3d RotateSpringAtStart { get; private set; }
            public Vector3d TranslateSpringAtEnd { get; private set; }
            public Vector3d RotateSpringAtEnd { get; private set; }

            #endregion

            #region constructors
            public Joint()
            {
            }
            public Joint(int _loadCase, double _fx, double _fy, double _fz, double _mx, double _my, double _mz)
            {
                LoadCase = _loadCase;
            }
            #endregion

            #region properties
            #endregion

            #region methods
            public Joint DeepCopy()
            {
                return (Joint)base.MemberwiseClone();
            }
            public override string ToString()
            {
                string info;
                info = "<Joint> FX:" + FX.ToString() +
                    " FY:" + FY.ToString() +
                    " FZ:" + FZ.ToString() +
                    " MX:" + MX.ToString() +
                    " MY:" + MY.ToString() +
                    " MZ:" + MZ.ToString();
                return info;
            }
            public bool IsValid()
            {
                return true;
            }
            #endregion
        }

        public class GH_Joint : GH_Goo<Joint>
        {
            public GH_Joint() { }
            public GH_Joint(GH_Joint other) : base(other.Value) { this.Value = other.Value.DeepCopy(); }
            public GH_Joint(Joint sec) : base(sec) { this.Value = sec; }
            public override bool IsValid => base.m_value.IsValid();

            public override string TypeName => "Joint";

            public override string TypeDescription => "Joint";

            public override IGH_Goo Duplicate()
            {
                return new GH_Joint(this);
            }

            public override string ToString()
            {
                return Value.ToString();
            }
        }

        public class Param_Joint : GH_PersistentParam<GH_Joint>
        {
            public Param_Joint() : base(new GH_InstanceDescription("Joint", "Joint", "Joint", CommonProps.category, CommonProps.subcate0)) { }

            protected override System.Drawing.Bitmap Icon { get { return null; } }  //クラスにアイコンを付けたい場合はここ

            public override Guid ComponentGuid => new Guid("FB9C3075-220A-424E-AC7B-E515303D8A2F");

            protected override GH_GetterResult Prompt_Plural(ref List<GH_Joint> values)
            {
                return GH_GetterResult.success;
            }

            protected override GH_GetterResult Prompt_Singular(ref GH_Joint value)
            {
                return GH_GetterResult.success;
            }
        }
    }
