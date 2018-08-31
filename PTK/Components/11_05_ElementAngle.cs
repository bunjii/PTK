using System;
using System.Collections.Generic;
using Grasshopper.Documentation;
using Grasshopper.Kernel;
using Karamba.Utilities.UIWidgets;
using Rhino.Geometry;

namespace PTK.Components
{
    public class _11_05_ElementAngle : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the _11_03_ElementTag class.
        /// </summary>
        public _11_05_ElementAngle()
          : base("ET", "ElementTag",
              "Checks the tags of the element",
              CommonProps.category, CommonProps.subcat6)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("Tags Are", "Tags", "Input the tags the element must contain", GH_ParamAccess.list,0);
            pManager.AddIntegerParameter("Mode", "Mode", "Mode 0 - EitherOf - The detail must contain either of the inputted tags"  , GH_ParamAccess.item, 0);
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
            
            List<string> tagsAre = new List<string>();
            int strict = 0;

            DA.GetDataList(0, tagsAre);
            DA.GetData(1, ref strict);

            //Initializing the object
            ElementTag ElementTag = new ElementTag(tagsAre, strict);

            var Verifier = new List<MethodDelegate>();//Initializing a delegatemethodlist
            
            Verifier.Add(ElementTag.check); //Stores the methdod in the list-> In this way the instance of the tagIs is associated with the instance of the method(!)


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
            get { return new Guid("7dbd4d6c-34e1-4b54-98bc-d08d5178ae22"); }
        }
    }
}