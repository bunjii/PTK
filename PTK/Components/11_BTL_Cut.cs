using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace PTK.Components
{
    public class _11_BTL_Cut : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the _11_BTL_Cut class.
        /// </summary>
        public _11_BTL_Cut()
          : base("_11_BTL_Cut", "Nickname",
              "Description",
              CommonProps.category, CommonProps.subcate11)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("A", "Assembly", "", GH_ParamAccess.item);
            pManager.AddPlaneParameter("P", "CutPlane", "", GH_ParamAccess.list);
            pManager.AddIntegerParameter("ID", "ElemID", "", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("B", "BTL-Cut", "", GH_ParamAccess.list);
            pManager.AddBrepParameter("Brep", "BTL-Cut", "", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            /*
            Assembly Assembly = new Assembly();
            List<Plane> cutPlanes = new List<Plane>();

            List<int> ElemIDs = new List<int>();

            DA.GetData(0, ref Assembly);
            DA.GetDataList(1, cutPlanes);
            DA.GetDataList(2, ElemIDs);

            List<BTLprocess> Processes = new List<BTLprocess>();
            List<Brep> Breps = new List<Brep>();

            int i = 0;
            foreach (int ElemID in ElemIDs)
            {
                Plane cutPlane = cutPlanes[i];
                PTK_Element elem = Assembly.Elems.Find(t => t.Id == ElemID);
                Processes.Add(BTLprocess.Cut(elem, cutPlane));
                Breps.Add(Processes[i].Voidgeometry);

                i++;

            }

            DA.SetDataList(0, Processes);
            DA.SetDataList(1, Breps);
            */
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
            get { return new Guid("9ef81ec8-a7e3-4ced-8ed2-fb05a2fd5ef7"); }
        }
    }
}