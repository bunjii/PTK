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
          : base("2", "2",
              "Test component no.2: Family Gatherer",
              "PTK", "STR")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("PTK ELEM", "PTK E", "PTK ELEM", GH_ParamAccess.list);
            // pManager.AddGenericParameter("PTK NODE", "PTK N", "PTK NODE", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PTK NODE", "PTK N", "PTK NODE", GH_ParamAccess.item);
            pManager.AddGenericParameter("PTK ELEM", "PTK E", "PTK ELEM", GH_ParamAccess.item);
            pManager.AddLineParameter("Line", "Line", "Line", GH_ParamAccess.list);
            pManager.AddPointParameter("Points", "Pts", "Points", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // 1. Declare placeholder variables

            int nodeIdNum = 0;
            int elemIdNum = 0;
            List<Node> nodes = new List<Node>();
            List<Line> lines = new List<Line>();
            List<Point3d> pts = new List<Point3d>();
            List<Element> elems = new List<Element>();
            List<Element> tempElemList = new List<Element>();

            GH_ObjectWrapper wrapNode = new GH_ObjectWrapper();
            List<GH_ObjectWrapper> wrapElemList = new List<GH_ObjectWrapper>();

            // 2. Retrieve input data
            if (!DA.GetDataList(0, wrapElemList)) { return; }

            // 3. Solve
            // DDL "unwrap wrapped element class" and "merge multiple element class"
            for (int i = 0; i < wrapElemList.Count; i++)
            {
                wrapElemList[i].CastTo<List<Element>>(out tempElemList);
                elems.AddRange(tempElemList);
            }

            // DDL "generate Elem ID"
            for (int i = 0; i < elems.Count; i++)
            {
                elems[i].ID = elemIdNum;
                elemIdNum++;
            }

            // DDL "create grasshopper lines" and "create node"
            for (int i = 0; i < elems.Count; i++)
            {
                lines.Add(elems[i].Ln);
                Node tempNode0 = new Node(elems[i].Ln.From);
                Node tempNode1 = new Node(elems[i].Ln.To);

                Node.AddElemIds(nodes, elems[i], tempNode0);
                Node.AddElemIds(nodes, elems[i], tempNode1);

            }

            // DDL "generate Node ID" and "grasshopper pts"
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].ID = nodeIdNum;
                nodeIdNum++;

                pts.Add(nodes[i].Pt3d);

            }

            // DDL "output"
            DA.SetData(0, nodes);
            DA.SetData(1, elems);
            DA.SetDataList(2, lines);
            DA.SetDataList(3, pts);
            
            
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
