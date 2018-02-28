using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace PTK
{
    public class PTK6_RectangularCrossection : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the PTK6 class.
        /// </summary>
        public PTK6_RectangularCrossection()
          : base("Rectangular CrossSection", "R CS",
              "CrossSection is being generated based on width, height, alignment and height-direction ",
              "PTK", "Materializer")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Width", "W", "Width", GH_ParamAccess.item,100);  
            pManager.AddNumberParameter("Height", "H", "Height", GH_ParamAccess.item,100);
            pManager.AddNumberParameter("Offset Y", "O:Y", "Offset from Width in positive or negative direction", GH_ParamAccess.item,0);
            pManager.AddNumberParameter("Offset Z", "O:Z", "Offset from HeightDirection in positive or negative direction", GH_ParamAccess.item,0);
            pManager.AddVectorParameter("Z direction", "Z", "A vector that describe the Height-direction of the element",GH_ParamAccess.item, new Vector3d(0,0,1));

            pManager[0].Optional = true;
            pManager[1].Optional = true;

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CrossSection", "CS", "Crossection data to be connected in the materializer", GH_ParamAccess.item);
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
           
            if (!DA.GetData(0, ref width)) { return; }
            if (!DA.GetData(1, ref height)) { return; }
            DA.GetData(4, ref offset);
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