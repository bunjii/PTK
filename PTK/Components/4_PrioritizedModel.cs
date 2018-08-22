using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

using Karamba.Models;
using Karamba.Elements;


namespace PTK
{
    public class PTK_4_PrioritizedModel : GH_Component
    {
        public PTK_4_PrioritizedModel()
          : base("Prioritized Model", "PrioriMod",
              "Creating model data by calculating notches between elements according to priority",
              CommonProps.category, CommonProps.subcate4)
        {
            Message = CommonProps.initialMessage;
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new Param_Assembly(), "Assembly", "A", "Assembly", GH_ParamAccess.item);
            pManager.AddTextParameter("Priority Data", "P", "Priority of element category name indicated by comma delimiter", GH_ParamAccess.item);

            pManager[1].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PTK Assembly", "A (PTK)", "PTK Assembly", GH_ParamAccess.item);
            // pManager.AddLineParameter("lines", "lines", "lines", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            

        }


        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return PTK.Properties.Resources.ico_global;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("3cf82ec9-1233-4aa7-b233-a467fcf8c41b"); }
        }
    }
}