using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace PTK
{
    public class PTK_1_2_Material : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public PTK_1_2_Material()
          : base("Material", "MT",
              "Add materialdata here",
              "PTK", "Materializer")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("MaterialName","MN", "Name the material", GH_ParamAccess.item,"UntitledMaterial");      //We should add default values here.
            pManager.AddIntegerParameter("MaterialID", "MId", "ID of the material", GH_ParamAccess.item,0);    //We should add default values here.
            pManager.AddGenericParameter("Properties", "Pro", "Add strutural properties here", GH_ParamAccess.item);    //We should add default values here.

           

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Material", "M", "MaterialData to be connected with MaterializerComponent", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            #region variables
            string materialName = "N/A";
            int materailId = 0;

            Material_properties properties ;
            #endregion
            GH_ObjectWrapper wrapprop = new GH_ObjectWrapper();

            #region input
            DA.GetData(0, ref materialName);
            if (!DA.GetData(1, ref materailId)) { return; }
            if (!DA.GetData(2, ref wrapprop)) { return; }

            wrapprop.CastTo <Material_properties>(out properties);
            #endregion

            #region solve
            Material outMaterial = new Material(materialName, materailId, properties);
            

            #endregion

            #region output
            DA.SetData(0, outMaterial);
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
                return PTK.Properties.Resources.icontest10;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("911bef7b-feea-46d8-abe9-f686d11b9c41"); }
        }
    }
}