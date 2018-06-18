using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace PTK.Components
{
    public class _11_02_ElementLength : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the _11_02_ElementLength class.
        /// </summary>
        public _11_02_ElementLength()
          : base("EM", "ElementAmount",
              "Checks the length of the elements",
              CommonProps.category, CommonProps.subcat6)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("Min Length", "Min", "Add Minimum length of elements in detail", GH_ParamAccess.item, 0);
            pManager.AddIntegerParameter("Max Length", "Max", "Add Maximum length of elements in detail", GH_ParamAccess.item, 100000);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("DetailDescription", "DD", "Outputed detailDescription", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //Inputs
            int minAmount = 0;
            int maxAmount = 100000;
            DA.GetData(0, ref minAmount);
            DA.GetData(1, ref maxAmount);

            //Initializing the object
            ElemAmount ElemAmount = new ElemAmount(minAmount, maxAmount);


            var Verifier = new List<MethodDelegate>();//Initializing a delegatemethodlist
            Verifier.Add(ElemAmount.check); //Stores the methdod in the list-> In this way the instance of the minlength/maxlength is associated with the instance of the method(!)

            Rule Rule = new Rule(Verifier);  //Pushing it through an object

            DA.SetData(0, Rule);
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
            get { return new Guid("7dbd4d6c-34e1-4b54-98bc-d08d5178ae91"); }
        }
    }
}