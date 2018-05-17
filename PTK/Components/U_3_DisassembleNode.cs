using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

using Rhino.Geometry;

// using System.Numerics;

namespace PTK
{
    public class PTK_U_3 : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the TestD class.
        /// </summary>
        public PTK_U_3()
          : base("Disassemble PTK Node", "X Node",
              "Disassemble Node (PTK)",
              CommonProps.category, CommonProps.subcat5)
        {
            Message = CommonProps.initialMessage;
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("PTK NODE", "N (PTK)", "PTK NODE", GH_ParamAccess.item);
            pManager.AddTextParameter("PTK NODE ID", "N (PTK) ID", "Node IDs to be disassembled.", GH_ParamAccess.list);

            pManager[1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("points", "pts", "points", GH_ParamAccess.list);
            pManager.AddIntegerParameter("PTK NODE ID", "N (PTK) ID", "PTK NODE ID", GH_ParamAccess.list);
            pManager.AddIntegerParameter("PTK NODE ELEM ID", "N (PTK) EID", "PTK NODE ELEM ID", GH_ParamAccess.tree);
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
            List<Node> outNodes = new List<Node>();
            List<string> inputIdsTxt = new List<string>();
            List<int> nodeIds = new List<int>();
            List<Point3d> points = new List<Point3d>();
            DataTree<int> elemIdTree = new DataTree<int>();
            #endregion

            #region input
            if (!DA.GetData(0, ref wrapNode)) { return; }
            wrapNode.CastTo<List<Node>>(out nodes);
            DA.GetDataList(1, inputIdsTxt);
            #endregion

            #region solve
            if (inputIdsTxt.Count == 0) outNodes = nodes;
            else
            {
                for (int i = 0; i < inputIdsTxt.Count; i++)
                {
                    inputIdsTxt[i] = inputIdsTxt[i].Trim();
                }

                foreach (Node n in nodes)
                {
                    if (!inputIdsTxt.Contains(n.Id.ToString())) continue;

                    outNodes.Add(n);
                }

            }


            for (int i=0;i<outNodes.Count;i++)
            {
                points.Add(outNodes[i].Pt3d);
                nodeIds.Add(outNodes[i].Id);

                GH_Path path;

                if (inputIdsTxt.Count == 0) path = new GH_Path(i);
                else path = new GH_Path(int.Parse(inputIdsTxt[i]));

                foreach (int j in outNodes[i].ElemIds)
                {
                    elemIdTree.Add(j, path);
                }
            }
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
                return PTK.Properties.Resources.ico_xnode;
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