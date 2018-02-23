using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace PTK
{
    public class PTK6 : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the PTK6 class.
        /// </summary>
        public PTK6()
          : base("6", "6",
              "Test component no.6: PTK Rectangular Section",
              "PTK", "1_INPUT")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Tag", "Tag", "Tag", GH_ParamAccess.item);
            pManager.AddNumberParameter("Width", "Width", "Width", GH_ParamAccess.item);
            pManager.AddNumberParameter("Height", "Height", "Height", GH_ParamAccess.item);
            pManager.AddVectorParameter("Offset", "Offset", "Offset", GH_ParamAccess.item);

            pManager[0].Optional = true;
            pManager[3].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PTK SECTION", "PTK_S", "PTK_SECTION", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            #region variables
            string sectionTag = "N/A";
            double width = new double();
            double height = new double();
            Vector3d offset = new Vector3d(0, 0, 0);
            
            #endregion

            #region input
            DA.GetData(0, ref sectionTag);
            if (!DA.GetData(1, ref width)) { return; }
            if (!DA.GetData(2, ref height)) { return; }
            DA.GetData(3, ref offset);
            #endregion

            #region solve
            Section rectSec = new Section(sectionTag, width, height, offset);
            string test = "";
            test += rectSec.Tag + ", " + rectSec.Height.ToString();
            // MessageBox.Show(test);
            #endregion

            #region output
            DA.SetData(0, rectSec);
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
                return PTK.Properties.Resources.icon_truss;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("59eb5896-0ccb-4e37-be5e-ba4ee7931ee1"); }
        }
    }
}