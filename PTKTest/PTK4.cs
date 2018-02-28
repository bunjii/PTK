using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

using Rhino.Geometry;

using System.Numerics;

namespace PTK
{
    public class PTK4 : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the TestD class.
        /// </summary>
        public PTK4()
          : base("4", "4",
              "Test component no.4: Decompose Node (Extract Node)",
              "PTK", "5_UTIL")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("PTK NODE", "PTK N", "PTK NODE", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Points", "Points", "Points", GH_ParamAccess.list);
            pManager.AddIntegerParameter("PTK NODE ID", "PTK N ID", "PTK NODE ID", GH_ParamAccess.list);
            pManager.AddIntegerParameter("PTK NODE ELEM ID", "PTK N E ID", "PTK NODE ELEM ID", GH_ParamAccess.tree);
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

            List<Point3d> points = new List<Point3d>();
            List<int> nodeIds = new List<int>();
            DataTree<int> elemIdTree = new DataTree<int>();
            #endregion

            #region input
            if (!DA.GetData(0, ref wrapNode)) { return; }
            wrapNode.CastTo<List<Node>>(out nodes);
            #endregion

            #region solve
            /* foreach (Node n in nodes)
            for (int i = 0; i < nodes.Count; i++)
            {
                points.Add(nodes[i].Pt3d);
                nodeIds.Add(nodes[i].ID);
                GH_Path path = new GH_Path(i);
                foreach (int j in nodes[i].ElemIds)
                {
                    elemIdTree.Add(j, path);

                }
            }
            */
            #endregion


            #region output
            DA.SetDataList(0, points);
            DA.SetDataList(1, nodeIds);
            DA.SetDataTree(2, elemIdTree);
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
            get { return new Guid("dd8adcf2-521c-44a4-8448-f0335469c0dd"); }
        }
    }
}