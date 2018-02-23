using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
// using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace PTK
{
    public class PTK1 : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public PTK1()
          : base("1", "1",
              "Test component no.1: Family Maker",
              "PTK", "1_INPUT")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddLineParameter("Lines", "Lines", "Lines", GH_ParamAccess.list);
            // pManager.AddGenericParameter();
            pManager.AddTextParameter("Tag", "Tag", "Tag", GH_ParamAccess.item);
            pManager.AddVectorParameter("Vec z", "Vec z", "Vec z", GH_ParamAccess.list);

            pManager[1].Optional = true;
            pManager[2].Optional = true;

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PTK ELEM", "PTK E", "PTK ELEM", GH_ParamAccess.item);
            // pManager.AddGenericParameter("PTK NODE", "PTK N", "PTK NODE", GH_ParamAccess.item);
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
            List<Line> lines = new List<Line>();
            List<Point3d> pts = new List<Point3d>();
            List<Element> elems = new List<Element>();
            List<Node> nodes = new List<Node>();

            // 2. Retrieve input data
            if (!DA.GetDataList(0, lines)) { return; }

            // 3. Abort on invalid inputs
            // if (!lines.IsValid) { return; }

            // Solve
            for (int i = 0; i < lines.Count; i++)
            {
                if (!lines[i].IsValid) { return; }

                elems.Add(new Element(lines[i]));

                pts.Add(lines[i].From);
                pts.Add(lines[i].To);
            }

            for (int i = 0; i < pts.Count; i++)
            {
                nodes.Add(new Node(pts[i]));
            }

            DA.SetData(0, elems);
            
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
                return PTK.Properties.Resources.icon_truss;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("0f259d4d-3cf2-4337-9545-c392178e1fe1"); }
        }
    }
}
