
using Grasshopper.Kernel;
using System;


namespace PTK
{
    public class PTK_1_2_1_Material_properties : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public PTK_1_2_1_Material_properties()
          : base("1_2_1_Material_properties", "MTp",
              "Add material properties here",
              "PTK", "Materializer")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("MaterialName", "MN", "Name the material", GH_ParamAccess.item, "GL26C");      //We should add default values here.

            pManager.AddNumberParameter("f m,g,k", "fmgk", "Charasteric bending strentgh", GH_ParamAccess.item, 26 );
            pManager.AddNumberParameter("f t,0,g,k", "ft0gk", "Charasteric bending strentgh", GH_ParamAccess.item, 19 );
            pManager.AddNumberParameter("f t,90,g,k", "ft90gk", "Charasteric bending strentgh", GH_ParamAccess.item, 0.5 );
            pManager.AddNumberParameter("f c,0,g,k", "fc0gk", "Charasteric bending strentgh", GH_ParamAccess.item, 23.5 );
            pManager.AddNumberParameter("f c,90,g,k", "fc90gk", "Charasteric bending strentgh", GH_ParamAccess.item, 2.5 );
            pManager.AddNumberParameter("f v,g,k", "fvgk", "Charasteric bending strentgh", GH_ParamAccess.item, 3.5 );
            pManager.AddNumberParameter("f r,g,k", "frgk", "Charasteric bending strentgh", GH_ParamAccess.item, 1.2 );

            pManager.AddNumberParameter("E 0,g,mean", "E0gmean", "Charasteric bending strentgh", GH_ParamAccess.item, 12000 );
            pManager.AddNumberParameter("E 0,g,05", "E0g05", "Charasteric bending strentgh", GH_ParamAccess.item, 10000 );
            pManager.AddNumberParameter("E 90,g,mean", "E90gmean", "Charasteric bending strentgh", GH_ParamAccess.item, 300 );
            pManager.AddNumberParameter("E 90,g,05", "E90g05", "Charasteric bending strentgh", GH_ParamAccess.item, 250);

            pManager.AddNumberParameter("G g,mean", "Ggmean", "Charasteric bending strentgh", GH_ParamAccess.item, 650 );
            pManager.AddNumberParameter("G g,05", "Gg05", "Charasteric bending strentgh", GH_ParamAccess.item, 542 );
            pManager.AddNumberParameter("G r,g,mean", "Grgmean", "Charasteric bending strentgh", GH_ParamAccess.item, 65 );
            pManager.AddNumberParameter("G r,g,05", "Grg05", "Charasteric bending strentgh", GH_ParamAccess.item, 54);

            pManager.AddNumberParameter("Rho g,k", "Rhogk", "Charasteric bending strentgh", GH_ParamAccess.item, 385);
            pManager.AddNumberParameter("Rho g,meam", "Rhogmean" , "Charasteric bending strentgh", GH_ParamAccess.item, 420);


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

            // for glulam according LIMTREBOKA
            double fmgk = new double() ;
            double ft0gk = new double();
            double ft90gk = new double();

            double fc0gk = new double();
            double fc90gk = new double();

            double fvgk = new double();
            double frgk = new double();

            double E0gmean = new double();
            double E0g05 = new double();
            double E90gmean = new double();
            double E90g05 = new double();

            double Ggmean = new double();
            double Gg05 = new double();
            double Gtgmean = new double();
            double Grg05 = new double();

            double Rhogk = new double();
            double Rhogmean = new double();
            #endregion

            #region input
            DA.GetData(0, ref MaterialName);
            if (!DA.GetData(1, ref fmgk)) { return; }
            if (!DA.GetData(2, ref ft0gk)) { return; }
            if (!DA.GetData(3, ref ft90gk)) { return; }
            if (!DA.GetData(4, ref fc0gk)) { return; }
            if (!DA.GetData(5, ref fc90gk)) { return; }
            if (!DA.GetData(6, ref fvgk)) { return; }
            if (!DA.GetData(7, ref frgk)) { return; }

            if (!DA.GetData(8, ref E0gmean)) { return; }
            if (!DA.GetData(9, ref E0g05)) { return; }
            if (!DA.GetData(10, ref E90gmean)) { return; }
            if (!DA.GetData(11, ref E90g05)) { return; }

            if (!DA.GetData(12, ref Ggmean)) { return; }
            if (!DA.GetData(13, ref Gg05)) { return; }
            if (!DA.GetData(14, ref Gtgmean)) { return; }
            if (!DA.GetData(15, ref Grg05)) { return; }

            if (!DA.GetData(16, ref Rhogk)) { return; }
            if (!DA.GetData(17, ref Rhogmean)) { return; }
            #endregion

            #region solve
            Material_properties Material_prop = new Material_properties(
                MaterialName,
                fmgk,

                ft0gk,
                ft90gk,
             
                fc0gk,
                fc90gk,
             
                fvgk,
                frgk, 

                E0gmean,
                E0g05,
                E90gmean,
                E90g05,
             
                Ggmean,
                Gg05,
                Gtgmean,
                Grg05,

                Rhogk,
                Rhogmean   
            
            );
            

            #endregion

            #region output
            DA.SetData(0, Material_prop);
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
                return PTK.Properties.Resources.icontest1;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("965bef7b-feea-54d8-abe9-f686d28b9c41"); }
        }
    }
}
