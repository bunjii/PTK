using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace PTK
{
    public class PTK_6_DetailModel : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the PTK_C_02 class.
        /// </summary>
        public PTK_6_DetailModel()
          : base("Detail Model (PTK)", "DM (PTK)",
              "This is to combine detail logic to PTK Class",
              "PTK", "4_DETAIL")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("DetailingGroupName", "", "", GH_ParamAccess.item);
            pManager.AddGenericParameter("PTK Assembly", "PTK A", "PTK Assembly", GH_ParamAccess.item);
            // pManager.AddGenericParameter("PTK LOGIC", "PTK LOGIC", "COLLECTIONS OF DETAIL SELECTIONS", GH_ParamAccess.item);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Eleements", "PTK A", "PTK Assembly", GH_ParamAccess.list);
            pManager.AddPointParameter("nodes", "", "", GH_ParamAccess.list);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            #region variables

            Assembly assemble = null;
            string Name = "";

            DA.GetData(0, ref Name);

            DA.GetData(1, ref assemble);

            

            List<Detail> Details = assemble.DetailingGroups.Find(t => t.Name == Name).Details;


            List<Curve> curves = new List<Curve>();
            List<Point3d> points = new List<Point3d>();

            foreach(Detail Detail in Details)
            {
                
                    foreach (Element elem in Detail.Elems)
                {
                    curves.Add(elem.Crv);
                }
                    foreach (Node node in Detail.Nodes)
                {
                    points.Add(node.Pt3d);
                }


            }

            DA.SetDataList(0, curves);
            DA.SetDataList(1, points);



            #endregion



            #region solve


            #endregion

            #region output
            DA.SetData(0, assemble);
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
                return PTK.Properties.Resources.icontest14;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("6b0f9e23-3c7b-4ecd-8f8a-f3f3d3487703"); }
        }
    }
}