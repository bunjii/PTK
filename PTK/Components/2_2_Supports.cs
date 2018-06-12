using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using GH_IO.Serialization;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;

using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Rhino.Geometry;

using Karamba;


// using Karamba;

namespace PTK
{

    public class PTK_2_2_Supports : GH_Component
    {
        
        private string boolSupString = "";
        private bool[] boolSupArray = { false, false, false, false, false, false }; // six degrees of freedom
        
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public PTK_2_2_Supports()
          : base("Supports (PTK)", "Supports",
              "Add Supports Conditions here",
              CommonProps.category, CommonProps.subcat4)
        {
            Message = CommonProps.initialMessage;
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Position", "point", "Boundary point", GH_ParamAccess.list );
            pManager.AddPlaneParameter("Orientation", "orientation", "Plane which defines rotatation of support point", GH_ParamAccess.list, Plane.WorldXY);
            pManager.AddTextParameter("Conditions", "bc", "Boundary condtitions", GH_ParamAccess.item,"111000");
 
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        /// 
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PTK Supports", "Sup (PTK)", "Support data to be send to Assembler(PTK)", GH_ParamAccess.item);
            pManager.RegisterParam(new Karamba.Supports.Param_Support(), "Support", "supp", "Ouput support(s)");
            pManager.AddTextParameter("BCinfo", "bccond", "Information about realeses", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            #region variables

            string Tag = "N/A";
            int lCase = 0;
            string bc="111000";

            List<PTK_Support> sups = new List<PTK_Support>();
            List<Karamba.Supports.GH_Support> sups_GH_krmb = new List<Karamba.Supports.GH_Support>();

            List<bool> sup_bc_list = new List<bool>();
            List<string> sup_bc_info = new List<string>();

            List<string> Tag_list= new List<string>();
            List<int> lcase_list = new List<int>();

            List<Point3d> sup_point_list = new List<Point3d>();
            List<Plane> sup_orientation_list = new List<Plane>();
            
            #endregion

            #region input
            
            if (!DA.GetDataList(0,  sup_point_list)) { return; }
            if (!DA.GetDataList(1,  sup_orientation_list)) { return; }

            if (!DA.GetData(2, ref bc)) { return; }
            #endregion

            #region solve
            // 
            //Message = boolSupString;
            //bc = boolSupString;
            char[] bool_sup_a = bc.ToCharArray();

            List<string> infolist = new List<string> {
            "Translation in dir 1 is ",
            "Translation in dir 2 is ",
            "Translation in dir 3 is ",
            "Rotation in dir 1 is ",
            "Rotation in dir 2 is ",
            "Rotation in dir 3 is ",
            };

            int i=0;
            foreach (char c in bool_sup_a)
            {
                string tempc = c.ToString();
                if (tempc == "0")
                {
                    sup_bc_list.Add(false);
                    sup_bc_info.Add(" " + infolist[i] + " free");
                    i++;
                }
                else
                {
                    sup_bc_list.Add(true);
                    sup_bc_info.Add(" " + infolist[i] + " blocked");
                    i++;
                }

            }

            List<bool> bc_list = sup_bc_list;


            int id1 = 0;
            foreach (var p in sup_point_list)
            {
                Karamba.Supports.Support tmp_sup = new Karamba.Supports.Support(sup_point_list[id1], bc_list , sup_orientation_list[id1]);
                Karamba.Supports.GH_Support krmb_sup = new Karamba.Supports.GH_Support(tmp_sup);
                
                sups.Add(new PTK_Support(krmb_sup));
                sups_GH_krmb.Add( krmb_sup );

                id1++;
            }

            

            #endregion

            #region output


            DA.SetData(0, sups) ;
            DA.SetDataList(1, sups_GH_krmb) ;
            DA.SetDataList(2, sup_bc_info );
            #endregion
        }
        



        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return PTK.Properties.Resources.ico_support;
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
