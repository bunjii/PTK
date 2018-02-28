using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace PTK
{
    public class PTK_UTIL_1 : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the PTK_UTIL_1 class.
        /// </summary>
        public PTK_UTIL_1()
          : base("GENERATE GEOMETRY", "GEOMETRY",
              "Generating Mesh or Brep Geometry",
              "PTK", "5_UTIL")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("PTK INPUT", "PTK IN", "PTK DATA INPUT", GH_ParamAccess.item);
            pManager.AddBooleanParameter("IsMesh", "IsMesh", "IsMesh", GH_ParamAccess.item);
            pManager.AddBooleanParameter("IsBrep", "IsBrep", "IsBrep", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddBrepParameter("BREP", "BREP", "BREP", GH_ParamAccess.list);
            pManager.AddMeshParameter("MESH", "MESH", "MESH", GH_ParamAccess.list);
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
                return PTK.Properties.Resources.icon_truss;

            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("38eb4f7f-a3bc-4563-8ebe-dd37784db737"); }
        }
    }
}