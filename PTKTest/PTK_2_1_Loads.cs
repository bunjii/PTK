using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace PTK
{

        public class PTK2_1_Loads : GH_Component
        {
            /// <summary>
            /// Initializes a new instance of the MyComponent1 class.
            /// </summary>
            public PTK2_1_Loads()
              : base("Loads (PTK)", "PTK_2_1_Loads",
                  "Add loads here",
                  "PTK", "2_Inputs")
            {
            }

            /// <summary>
            /// Registers all the input parameters for this component.
            /// </summary>
            protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
            {
            pManager.AddTextParameter("Tag", "Tag", "Tag", GH_ParamAccess.item,"0");      //We should add default values here.
            pManager.AddIntegerParameter("LoadCase", "LC", "Load case", GH_ParamAccess.item, 0);    //We should add default values here.
            pManager.AddPointParameter("PointLoad", "Pt", "Point to which load will be assigned", GH_ParamAccess.item);
            pManager.AddVectorParameter("VectorLoad[kN]","Vec","Vector which describe the diretion and value in kN", GH_ParamAccess.item);

            }

            /// <summary>
            /// Registers all the output parameters for this component.
            /// </summary>
            protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
            {
                pManager.AddGenericParameter("Loads", "L", "Load data to be send to Assembler(PTK)", GH_ParamAccess.item);
            }

            /// <summary>
            /// This is the method that actually does the work.
            /// </summary>
            /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
            protected override void SolveInstance(IGH_DataAccess DA)
            {
                #region variables
                string Tag = "N/A";
                int lcase = 0;
                Point3d lpoint = new Point3d();
                Vector3d lvector = new Vector3d();

                #endregion

                #region input
                DA.GetData(0, ref Tag);
                if (!DA.GetData(1, ref lcase)) { return; }
                if (!DA.GetData(2, ref lpoint)) { return; }
                if (!DA.GetData(3, ref lvector)) { return; }
                #endregion

                #region solve
                Loads PTKloads = new Loads(Tag,lpoint,lvector);
           


                #endregion

                #region output
                DA.SetData(0, PTKloads);
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
                return PTK.Properties.Resources.icontest11;
                }
            }

            /// <summary>
            /// Gets the unique ID for this component. Do not change this ID after release.
            /// </summary>
            public override Guid ComponentGuid
            {
                get { return new Guid("965bef7b-feea-46d1-abe9-f686d28b9c41"); }
            }
        }



}
