using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace PTK
{
    public class PTK_1_4_Alignment_Simple : GH_Component
    {
        public PTK_1_4_Alignment_Simple()
          : base("Simple Alignment", "Align",
              "Simple Alignment",
              CommonProps.category, CommonProps.subcate2)
        {
            Message = CommonProps.initialMessage;
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "N", "Alignment Name", GH_ParamAccess.item);
            pManager.AddNumberParameter("Offset Y", "Y", "Offset length local y", GH_ParamAccess.item, 0.0);
            pManager.AddNumberParameter("Offset Z", "Z", "Offset length local z", GH_ParamAccess.item, 0.0);
            pManager.AddNumberParameter("Rotation Angle", "R", "Rotational angle in degree", GH_ParamAccess.item, 0.0);

            pManager[0].Optional = true;
            pManager[1].Optional = true;
            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.RegisterParam(new Param_Alignment(), "Alignment", "A", "AlignmentData to be added to materializer", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            #region variables
            string name = null;
            double offsetY = new double();
            double offsetZ = new double();
            double rotationAngle = new double();
            #endregion

            #region input
            if (!DA.GetData(0, ref name)) { return; }
            if (!DA.GetData(1, ref offsetY)) { return; }
            if (!DA.GetData(2, ref offsetZ)) { return; }
            if (!DA.GetData(3, ref rotationAngle)) { return; }
            #endregion

            #region solve
            GH_Alignment ali = new GH_Alignment(new Alignment(name, offsetY, offsetZ, rotationAngle));
            #endregion

            #region output
            DA.SetData(0, ali);
            #endregion
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return PTK.Properties.Resources.ico_align;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("a2017425-8288-4c78-9111-f9d9588a34df"); }
        }
    }
}