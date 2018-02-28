using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace PTK
{
    public class PTK1_5_Forces : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Forces class.
        /// </summary>
        public PTK1_5_Forces()
          : base("Forces", "F",
              "Adding forces here if data allready is provided ",
              "Category", "Subcategory")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("FX", "FX", "Add FX", GH_ParamAccess.tree, 0);   //Should be tree, cause more than force pr element
            pManager.AddNumberParameter("FX", "FX", "Add FX", GH_ParamAccess.tree, 0);
            pManager.AddNumberParameter("FX", "FX", "Add FX", GH_ParamAccess.tree, 0);
            pManager.AddNumberParameter("FX", "FX", "Add FX", GH_ParamAccess.tree, 0);
            pManager.AddNumberParameter("FX", "FX", "Add FX", GH_ParamAccess.tree, 0);
            pManager.AddNumberParameter("FX", "FX", "Add FX", GH_ParamAccess.tree, 0);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Forces", "F", "Forces to be added to Materializer", GH_ParamAccess.item);
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
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("76a606c9-f75b-4c7f-a30e-02baf83adb53"); }
        }
    }
}