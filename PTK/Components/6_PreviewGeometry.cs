using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace PTK.Components
{
    public class PTK_6_PreviewGeometry : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the _6_PreviewGeometry class.
        /// </summary>
        public PTK_6_PreviewGeometry()
          : base("Preview Geometry", "PrevGeom",
              "Preview Assembly",
              CommonProps.category, CommonProps.subcate6)
        {
            Message = CommonProps.initialMessage;
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new Param_Assembly(), "Assembly", "A", "connect an Assembly here", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddBrepParameter("Model", "M", "3d model", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            /////////////////////////////////////////////////////////////////////////////////
            // variables
            /////////////////////////////////////////////////////////////////////////////////
            GH_Assembly gAssembly = null;
            Assembly assembly = null;

            /////////////////////////////////////////////////////////////////////////////////
            // input
            /////////////////////////////////////////////////////////////////////////////////
            if (!DA.GetData(0, ref gAssembly))
            {
                return;
            }
            else
            {
                assembly = gAssembly.Value;
            }
            
            /////////////////////////////////////////////////////////////////////////////////
            // solve
            /////////////////////////////////////////////////////////////////////////////////

            /////////////////////////////////////////////////////////////////////////////////
            // output
            /////////////////////////////////////////////////////////////////////////////////
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
            get { return new Guid("bd28cf4d-1b9a-41cc-abca-e29bb12f09e9"); }
        }
    }
}