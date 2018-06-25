using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace PTK
{
    public class _11_00_DetailDescription : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the _11_00_DetailDescription class.
        /// </summary>
        public _11_00_DetailDescription()
          : base("DS", "DetailinDescription",
              "Description",
              CommonProps.category, CommonProps.subcat6)
        {
        }
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "N", "Add name here", GH_ParamAccess.item);
            pManager.AddGenericParameter("True", "T", "Add descriptions of the detail that are true here", GH_ParamAccess.list);
            pManager.AddGenericParameter("False", "F", "Add descriptions of the detail that are false here", GH_ParamAccess.list);
            pManager.AddGenericParameter("PlaneOrientation", "PO", "Add the plane orientation component here", GH_ParamAccess.item);
            pManager.AddGenericParameter("Support?", "S", "Optional: Add support component here if the details are supports", GH_ParamAccess.item);

            pManager[2].Optional = true;
            pManager[3].Optional = true;
            pManager[4].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("DetailingGroup", "DG", "Add this output to the Assembly-component", GH_ParamAccess.item);
        }




        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string name = "Untitled";

            List<Rule> Rules = new List<Rule>();

            var verifier = new List<MethodDelegate>();


            DA.GetDataList(1, Rules);
            DA.GetData(0, ref name);




            foreach (Rule rule in Rules)
            {
                verifier.AddRange(rule.Rules);
            }


            DetailingGroup DetailingGroup = new DetailingGroup(name, verifier);

            DA.SetData(0, DetailingGroup);







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
            get { return new Guid("0836e93b-8b3c-4d11-96bb-03542d4cd8f9"); }
        }
    }
}