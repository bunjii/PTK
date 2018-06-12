using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

using Karamba;

namespace PTK
{
    public class PTK_2_1_Loads : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public PTK_2_1_Loads()
            : base("Loads (PTK)", "Loads",
                "Add loads here",
                CommonProps.category, CommonProps.subcat4)
        {
            Message = CommonProps.initialMessage;
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {

            pManager.AddTextParameter("tag", "tag", "tag", GH_ParamAccess.list,"0");      //We should add default values here.
            pManager.AddIntegerParameter("Load Case", "LC", "Load case", GH_ParamAccess.list, 0);    //We should add default values here.
            pManager.AddPointParameter("Point Load", "pt", "Point to which load will be assigned", GH_ParamAccess.list, new Point3d() );
            pManager.AddVectorParameter("Vector Force Load","load vec","in [kN]. Vector which describe the diretion and value in kN", GH_ParamAccess.list, new Vector3d() );
            pManager.AddVectorParameter("Vector Moment Load", "moment vec", "in [kN]. Vector which describe the diretion and value in kNm", GH_ParamAccess.list, new Vector3d());

            pManager[0].Optional = true;
            pManager[1].Optional = true;
            pManager[2].Optional = true;
            // pManager[3].Optional = true;

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PTK Load", "L (PTK)", "Load data to be send to Assembler(PTK)", GH_ParamAccess.item );
            pManager.RegisterParam(new Karamba.Loads.Param_Load(), "Support", "supp", "Ouput support(s)");
            

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            #region variables
            
            List<string> Tag_list = new List<string>();
            List<int> lcase_list = new List<int>();
            List<Point3d> lpoint_list = new List<Point3d>();
            List<Vector3d> lfvector_list = new List<Vector3d>();
            List<Vector3d> lmvecotr_list = new List<Vector3d>();

            List<PTK_Load> load_list = new List<PTK_Load>();
            List<Karamba.Loads.GH_Load> load_GH_list = new List<Karamba.Loads.GH_Load>();
            #endregion

            #region input
            
            if (!DA.GetDataList(0, Tag_list))       { return; }
            if (!DA.GetDataList(1, lcase_list))     { return; }
            if (!DA.GetDataList(2, lpoint_list))    { return; }
            if (!DA.GetDataList(3, lfvector_list))       { return; }
            if (!DA.GetDataList(4, lmvecotr_list))       { return; }
            #endregion

            #region solve
            

            int id1 = 0;
            foreach (var p in lpoint_list)
            {
                var load_1 = new Karamba.Loads.PointLoad(lpoint_list[id1], lfvector_list[id1], lmvecotr_list[id1], lcase_list[id1], true);
                var krmb_load = new Karamba.Loads.GH_Load(load_1);

                load_GH_list.Add( krmb_load );
                load_list.Add( new PTK_Load(krmb_load) );

                id1++;
            }


            #endregion



            #region output
            DA.SetData(0, load_list);
            DA.SetDataList(1, load_GH_list);
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
            return PTK.Properties.Resources.ico_load;
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
