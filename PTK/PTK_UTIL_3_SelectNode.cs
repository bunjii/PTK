using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;


namespace PTK
{
    public class PTK_UTIL_3 : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the TestE class.
        /// </summary>
        public PTK_UTIL_3()
          : base("Select Node", "Sel N (PTK)",
              "Select Node (PTK)",
              "PTK", "UTIL")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("PTK NODE", "PTK N", "PTK NODE", GH_ParamAccess.item);
            pManager.AddIntegerParameter("PTK NODE ID", "PTK N ID", "PTK NODE ID", GH_ParamAccess.list);
            
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PTK NODE", "PTK N", "PTK NODE", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            #region variables
            GH_ObjectWrapper wrapNode = new GH_ObjectWrapper();
            List<Node> nodes = new List<Node>();
            List<int> nodeIds = new List<int>();
            List<Node> outNodes = new List<Node>();
            #endregion

            #region input
            if (!DA.GetData(0, ref wrapNode)) { return; }
            wrapNode.CastTo<List<Node>>(out nodes);
            if (!DA.GetDataList(1, nodeIds)) { return; }
            #endregion

            #region solve
            // foreach (Node n in nodes)
            for (int i = 0; i < nodeIds.Count; i++)
            {
                outNodes.Add(Node.FindNodeById(nodes, nodeIds[i]));
            }
            
            #endregion

            #region output
            DA.SetData(0, outNodes);
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
            get { return new Guid("d9fab058-b10a-459e-8cb9-8afe2d3a90d3"); }
        }
    }
}