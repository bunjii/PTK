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
        /// <summary>
        /// Initializes a new instance of the PTK_C_01 class.
        /// </summary>
        public PTK_4_PrioritizedModel()
          : base("Prioritized Model", "PrioriMod",
              "Combine PTK class and Karamba Analysis Data",
              CommonProps.category, CommonProps.subcate4)
        {
            Message = CommonProps.initialMessage;
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("PTK Assembly", "A (PTK)", "PTK Assembly", GH_ParamAccess.item);
            pManager.AddGenericParameter("Dimension Data", "Dim (PTK)", "", GH_ParamAccess.item);

            pManager[1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PTK Assembly", "A (PTK)", "PTK Assembly", GH_ParamAccess.item);
            // pManager.AddLineParameter("lines", "lines", "lines", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            

        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return PTK.Properties.Resources.ico_global;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("3cf82ec9-1233-4aa7-b233-a467fcf8c41b"); }
        }
    }
}