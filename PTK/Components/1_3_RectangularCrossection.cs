using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace PTK
{
    public class PTK_1_3_RectangularCrossSection : GH_Component
    {
        public PTK_1_3_RectangularCrossSection()
          : base("Rectangular Cross Section", "RectSec",
              "CrossSection is being generated based on width, height, alignment and height-direction ",
              CommonProps.category, CommonProps.subcate2)
        {
            Message = CommonProps.initialMessage;
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "N", "Add Cross Section Name", GH_ParamAccess.item);
            pManager.AddNumberParameter("Width", "W", "", GH_ParamAccess.item);  
            pManager.AddNumberParameter("Height", "H", "", GH_ParamAccess.item);

            pManager[0].Optional = true;
            pManager[1].Optional = true;
            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.RegisterParam(new Param_CrossSection(), "Cross Section", "S", "Cross Section data to be connected in the materializer", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            #region variables
            string name = "N/A";
            double width = new double();
            double height = new double();
            #endregion

            #region input
            if (!DA.GetData(0, ref name)) { return; }
            if (!DA.GetData(1, ref width)) { return; }
            if (!DA.GetData(2, ref height)) { return; }
            #endregion

            #region solve
            GH_CrossSection sec = new GH_CrossSection(new CrossSection(name, width, height));
            #endregion

            #region output
            DA.SetData(0, sec);
            #endregion
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return PTK.Properties.Resources.ico_rectangular;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("59eb5896-0ccb-4e37-be5e-ba4ee7931ee1"); }
        }
    }
}