using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace PTK.Components
{
    public class PTK_4_3_DimensioningMembers : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the _4_3_DimensioningMembers class.
        /// </summary>
        public PTK_4_3_DimensioningMembers()
          : base("Dimensioning Members", "Dimensioning",
              "Description",
              CommonProps.category, CommonProps.subcat4)
        {
            Message = CommonProps.initialMessage;
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Karamba Model", "Model", "", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Dimension Data", "Dim (PTK)", "", GH_ParamAccess.item);
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
                return PTK.Properties.Resources.ico_dimensioning;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("6639ee92-b0e1-4005-8df8-94d01ab78237"); }
        }
    }
}