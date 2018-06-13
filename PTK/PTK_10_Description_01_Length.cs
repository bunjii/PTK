using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Grasshopper.Kernel.Types;

namespace PTK
{
    public class PTK_10_Description_01_Length : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the PTK_10_Description_01_Length class.
        /// </summary>
        public PTK_10_Description_01_Length()
          : base("ElementLengthRule", "ElementLengthRule",
              "Description",
              "PTK", "Description")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Min Length", "Min", "Add Minimum length of elements in detail", GH_ParamAccess.item,0);
            pManager.AddNumberParameter("Max Length", "Max", "Add Maximum length of elements in detail", GH_ParamAccess.item, 100000000000);
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
            double minLength = 0;
            double maxLength = 100000000000;
            DA.GetData(0, ref minLength);
            DA.GetData(1, ref maxLength);
            
            //Initializing the object
            ElemLength Elemlength = new ElemLength(minLength, maxLength);


            var Verifier = new List<MethodDelegate>();//Initializing a delegatemethodlist
            Verifier.Add(Elemlength.check); //Stores the methdod in the list-> In this way the instance of the minlength/maxlength is associated with the instance of the method(!)

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
            get { return new Guid("cd4bf2b0-002d-474c-9274-40c96d741805"); }
        }
    }
}