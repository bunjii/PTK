using System;
using System.Collections.Generic;

using System.Runtime.InteropServices;
using System.Windows.Forms;

using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace PTK
{
   
    public class PTK2 : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public PTK2()
          : base("Test11B", "B",
              "TestBcomponent",
              "PTK", "STR")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Nodes", "Nodes", "PTK NODE ELEM", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            // pManager.AddGenericParameter("Nodes", "Nodes", "PTK NODE ELEM", GH_ParamAccess.list);
            // pManager.AddPointParameter("Points", "Pts", "Point 3d", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // 1. Declare placeholder variables and assign initial invalid data.
            //    This way, if the input parameters fail to supply valid data, we know when to abort.

            List<Node> nodes = new List<Node>();
            GH_ObjectWrapper wrapper = new GH_ObjectWrapper();

            // 2. Retrieve input data
            if (!DA.GetData(0, ref wrapper)) { return; }
            
            // Solve
            wrapper.CastTo<List<Node>>(out nodes);

            MessageBox.Show(nodes[0].Xval.ToString());
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                // return null;
                return PTKTEST11.Properties.Resources.icon_truss;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("d16b2f49-a170-4d47-ae63-f17a4907fed1"); }
        }
    }
}
