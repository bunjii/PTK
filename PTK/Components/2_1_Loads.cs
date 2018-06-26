using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace PTK
{
    public class PTK_2_1_Loads : GH_Component
    {
        public PTK_2_1_Loads()
            : base("Load", "Load",
                "Add load here",
                CommonProps.category, CommonProps.subcate4)
        {
            Message = CommonProps.initialMessage;
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Tag", "T", "Tag", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Load Case", "LC", "Load case", GH_ParamAccess.item, 0);
            pManager.AddPointParameter("Load Point", "P", "Point to which load will be assigned", GH_ParamAccess.item );
            pManager.AddVectorParameter("Load Vector","V","in [kN]. Vector which describe the diretion and value in kN", GH_ParamAccess.item);

            pManager[0].Optional = true;
            pManager[1].Optional = true;
            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.RegisterParam(new Param_Load(), "Load", "L", "Load data to be send to Assembler(PTK)", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            #region variables
            string Tag = null;
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
            GH_Load load = new GH_Load(new Load(Tag, lcase, lpoint, lvector));
            #endregion

            #region output
            DA.SetData(0, load);
            #endregion
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
            return PTK.Properties.Resources.ico_load;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("965bef7b-feea-46d1-abe9-f686d28b9c41"); }
        }
    }



}
