using System;
using System.Collections.Generic;
using Rhino.Geometry;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;

namespace PTK
{
    public class Load
    {
        #region fields
        public string Tag { get; private set; }
        public int LoadCase { get; private set; } = 0;
        public Point3d LoadPoint { get; private set; }
        public Vector3d LoadVector { get; private set; }
        #endregion

        #region constructors
        public Load()
        {
            Tag = "N/A";
            LoadPoint = new Point3d();
            LoadVector = new Vector3d();
        }
        public Load(string _tag, int _loadCase, Point3d _loadPoint, Vector3d _loadVector)
        {
            Tag = _tag;
            LoadCase = _loadCase;
            LoadPoint = _loadPoint;
            LoadVector = _loadVector;
        }
        #endregion

        #region properties
        #endregion

        #region methods
        public Load DeepCopy()
        {
            return (Load)base.MemberwiseClone();
        }
        public override string ToString()
        {
            string info;
            info = "<Load> Tag:" + Tag +
                " LoadCase:" + LoadCase.ToString() +
                " LoadPoint:" + LoadPoint.ToString() +
                " LoadVector:" + LoadVector.ToString() ;
            return info;
        }
        public bool IsValid()
        {
            return true;
        }
        #endregion
    }

    public class GH_Load : GH_Goo<Load>
    {
        public GH_Load() { }
        public GH_Load(GH_Load other) : base(other.Value) { this.Value = other.Value.DeepCopy(); }
        public GH_Load(Load sec) : base(sec) { this.Value = sec; }
        public override bool IsValid => base.m_value.IsValid();

        public override string TypeName => "Load";

        public override string TypeDescription => "Description";

        public override IGH_Goo Duplicate()
        {
            return new GH_Load(this);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class Param_Load : GH_PersistentParam<GH_Load>
    {
        public Param_Load() : base(new GH_InstanceDescription("Load", "Load", "Description", CommonProps.category, CommonProps.subcate0)) { }

        protected override System.Drawing.Bitmap Icon { get { return null; } }  //クラスにアイコンを付けたい場合はここ

        public override Guid ComponentGuid => new Guid("01A7F933-62C3-4780-87EB-381F3344D370");

        protected override GH_GetterResult Prompt_Plural(ref List<GH_Load> values)
        {
            return GH_GetterResult.success;
        }

        protected override GH_GetterResult Prompt_Singular(ref GH_Load value)
        {
            return GH_GetterResult.success;
        }
    }
}

