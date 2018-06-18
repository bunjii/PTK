using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace PTK.Components
{
    public class ExtractNode : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ExtractNode class.
        /// </summary>
        public ExtractNode()
          : base("ExtractNode", "Nickname",
              "Description",
              "Category", "Subcategory")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("N", "Node", "", GH_ParamAccess.list);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddIntegerParameter("ID", "", "", GH_ParamAccess.list);
            pManager.AddPointParameter("Point", "", "", GH_ParamAccess.list);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            
            List<Node> Nodes = new List<Node>();

            DA.GetDataList(0, Nodes);

            List<int> id = new List<int>();
            List<Point3d> pt = new List<Point3d>();
            






            foreach (Node node in Nodes)
            {
                id.Add(node.Id);

                pt.Add(node.Pt3d);


            }

            DA.SetDataList(0, id);
            DA.SetDataList(1, pt);



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
            get { return new Guid("a73bbf40-c8e1-4855-9ae3-422330ebbee7"); }
        }
    }
}