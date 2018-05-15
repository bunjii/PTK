using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace PTK
{
    public class PTK_1_1_Material : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public PTK_1_1_Material()
          : base("Material (PTK)", "Mat",
              "creates a Material",
              CommonProps.category, "Materialize")
        {
            Message = "PTK";
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("MaterialName","MN", "Names the material", GH_ParamAccess.item, "UntitledMaterial");      //We should add default values here.
            pManager.AddIntegerParameter("MaterialID", "MId", "ID of the material", GH_ParamAccess.item, -999);    //We should add default values here.
            pManager.AddGenericParameter("Material Properties(PTK)", "MP(PTK)", "Add material properties here", GH_ParamAccess.item);    //We should add default values here.

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Material", "M (PTK)", "MaterialData to be connected with Materializer Component", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            #region variables
            string materialName = "N/A";
            int matId = -999;
            MatProps matprops ;
            GH_ObjectWrapper wrapProp = new GH_ObjectWrapper();

            #endregion

            #region input
            DA.GetData(0, ref materialName);
            DA.GetData(1, ref matId);
            if (!DA.GetData(2, ref wrapProp)) { return; }
            wrapProp.CastTo <MatProps>(out matprops);

            #endregion

            #region solve
            Material outMaterial = new Material(matprops); // (materialName, matId, properties);
            outMaterial.MatName = matprops.MaterialName;
            #endregion

            #region output
            Message = outMaterial.MatName;
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