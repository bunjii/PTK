using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace PTK
{
    public class PTK1_2_Material : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public PTK1_2_Material()
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
            pManager.AddNumberParameter("Young Modulus", "Y", "Add Young Modulus", GH_ParamAccess.item,10);    //We should add default values here.
            pManager.AddNumberParameter("Density", "D", "Add density here", GH_ParamAccess.item,10);    //We should add default values here.
            pManager.AddNumberParameter("Price", "P", "Price pr M3", GH_ParamAccess.item, 300);
            pManager.AddTextParameter("Currency", "$", "name the currency",GH_ParamAccess.item,"Euro"); //Dollars
           

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
            string MaterialName = "N/A";
            double YoungModulus = new double();
            double Density = new double();
            double Price = new double();
            string Currency = "Dollars";
            #endregion

            #region input
            DA.GetData(0, ref MaterialName);
            if (!DA.GetData(1, ref YoungModulus)) { return; }
            if (!DA.GetData(2, ref Density)) { return; }
            if (!DA.GetData(3, ref Price)) { return; }
            if (!DA.GetData(4, ref Currency)) { return; }
            #endregion

            #region solve
            Material Material = new Material();
            Material.Materialname = MaterialName;
            Material.YoungModulus = YoungModulus;
            Material.Density = Density;
            Material.Price = Price;
            Material.Currency = Currency;

            #endregion

            #region output
            DA.SetData(0, Material);
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
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("965bef7b-feea-46d8-abe9-f686d28b9c41"); }
        }
    }
}