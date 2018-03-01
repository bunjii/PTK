using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

using Karamba;

namespace PTK
{

    public class PTK_2_2_Supports : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public PTK_2_2_Supports()
          : base("Supports (PTK)", "PTK_2_2_Supports",
              "Add supports here",
              "PTK", "2_Inputs")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Tag", "Tag", "Tag", GH_ParamAccess.item, "0");      //We should add default values here.
            pManager.AddIntegerParameter("LoadCase", "LC", "Load case", GH_ParamAccess.item, 0);    //We should add default values here.
            pManager.AddPointParameter("PointLoad", "Pt", "Point to which load will be assigned", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Rotations", "Rot", "Rotations Rx,Ry,Rz", GH_ParamAccess.list, new List < bool > { false, false, false });
            pManager.AddBooleanParameter("Translations", "Tra", "Translatons Tx,Ty,Tz", GH_ParamAccess.list, new List<bool> { false, false, false });

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Supports", "S", "Support data to be send to Assembler(PTK)", GH_ParamAccess.item);
            pManager.RegisterParam(new Karamba.Supports.Param_Support(), "supportK", "SK", "Support data to be send to Assembler(Karamba)");
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
            List<bool> lrot = new List<bool> { false, false, false };
            List<bool> ltra = new List<bool> { false, false, false };
            #endregion


            Karamba.Supports.Support news = new Karamba.Supports.Support(new Point3d(0,0,0), new List<bool> { false,false, false, false,false,false }, new Plane(new Point3d(0, 0, 0), new Vector3d(0,0,1)));




            #region input
            DA.GetData(0, ref Tag);
            if (!DA.GetData(1, ref lcase)) { return; }
            if (!DA.GetData(2, ref lpoint)) { return; }
            if (!DA.GetDataList(3, lrot)) { return; }
            if (!DA.GetDataList(4, ltra)) { return; }
            #endregion

            #region solve
            Supports PTKsupports = new Supports(Tag, lpoint, lrot, ltra);



            #endregion

            #region output
            DA.SetData(0, PTKsupports);
            DA.SetData(1, new Karamba.Supports.GH_Support(news));
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
            get { return new Guid("965bef7b-feea-46d1-abe9-f686d28c4c41"); }
        }
    }



}
