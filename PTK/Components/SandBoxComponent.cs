using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace PTK
{
    public class SandBoxComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent2 class.
        /// </summary>
        public SandBoxComponent()
          : base("Sandbox", "Sandbox",
              "Sandbox",
              "PTK", "Sandbox")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBrepParameter("BrepA", "BrepA", "", GH_ParamAccess.item);
            pManager.AddBrepParameter("BrepB", "BrepB", "", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddBrepParameter("result", "result", "result", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            #region variables
            Brep brepA = new Brep();
            Brep brepB = new Brep();
            #endregion

            #region input
            DA.GetData(0, ref brepA);
            DA.GetData(1, ref brepB);
            #endregion

            #region solve
            Brep[] slashed;
            slashed = Brep.CreateBooleanDifference(brepA, brepB, ProjectProperties.tolerances);
            #endregion

            #region output
            DA.SetDataList(0, slashed);
            #endregion


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
            get { return new Guid("94be282e-5f98-4970-bad1-d2193c34f47f"); }
        }
    }
}