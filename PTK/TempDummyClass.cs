using System;
// using System.Linq;
using System.Collections.Generic;

// using System.Runtime.InteropServices;
using System.Windows.Forms;

// using System.Diagnostics;
// using System.IO;
// using System.Threading.Tasks;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace PTK
{
    public class TempDummyClass : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the TESTOPENCLASS class.
        /// </summary>
        public TempDummyClass()
          : base("TESTOPENCLASS", "Nickname",
              "Description",
              "Category", "Subcategory")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Assembly", "", "", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Number", "", "", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Assembly Assembly = new Assembly();


            //Unwrapping the data
            DA.GetData(0, ref Assembly);


           
            List<Node> Node = Assembly.Node;
            List<Element> Element = Assembly.element;

            List<double> Height = new List<double>();
            for (int i = 0; i < Element.Count; i++)
            {
                Height.Add(Element[i].RectSec.Height);
            }

            DA.SetDataList(0, Height);

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
            get { return new Guid("22c345e5-4d0f-4eab-a2e8-b9d626e8fdca"); }
        }
    }
}