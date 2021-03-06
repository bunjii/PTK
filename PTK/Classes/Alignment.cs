﻿using Rhino.Geometry;
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
        public string Name { get; private set; }
        public AlignmentAnchorVert AnchorVert { get; private set; } = AlignmentAnchorVert.Center;
        public AlignmentAnchorHori AnchorHori { get; private set; } = AlignmentAnchorHori.Center;
        public double OffsetY { get; private set; } = 0;
        public double OffsetZ { get; private set; } = 0;
        public double RotationAngle { get; private set; } = 0;  //degree
        public Vector3d AlongVector { get; private set; }

        public Alignment()
        {
            Name = "N/A";
        }
        
        public Alignment(string _name, double _offsetY, double _offsetZ, double _rotationAngle)
        {
            OffsetY = _offsetY;
            OffsetZ = _offsetZ;
            RotationAngle = _rotationAngle;
            AlongVector = new Vector3d(0, 0, 0);
        }

        public Alignment(string _name, double _offsetY, double _offsetZ, double _rotationAngle, Vector3d _alongVector)
        {
            OffsetY = _offsetY;
            OffsetZ = _offsetZ;
            RotationAngle = _rotationAngle;
            AlongVector = _alongVector;
        }

        public void SetAnchor(AlignmentAnchorVert _ver,AlignmentAnchorHori _hor)
        {
            AnchorVert = _ver;
            AnchorHori = _hor;

        }

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
                " RotationAngle:" + RotationAngle.ToString() +
                " AlongVector:" + AlongVector.ToString();
            return info;
        }
        public bool IsValid()
        {
            return Name != "N/A";
        }
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

        protected override System.Drawing.Bitmap Icon { get { return null; } }  //Icon image setting

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
