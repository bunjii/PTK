using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

using Karamba.Supports;
using System.Windows.Forms;
using GH_IO.Serialization;
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
              "Add supports here",
              CommonProps.category, "Structure")
        {
            Message = "PTK";
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            // pManager.AddTextParameter("tag", "tag", "tag", GH_ParamAccess.item, "0");      //We should add default values here.
            pManager.AddIntegerParameter("Load Case", "LC", "Load case", GH_ParamAccess.item, 0);    //We should add default values here.
            pManager.AddPointParameter("Point", "pt", "Point to which load will be assigned", GH_ParamAccess.list);
            pManager.AddPlaneParameter("Plane", "plane", "", GH_ParamAccess.list);
            // pManager.AddBooleanParameter("Rotations", "rot", "Rotations Rx,Ry,Rz", GH_ParamAccess.list, new List < bool > { false, false, false });
            // pManager.AddBooleanParameter("Translations", "tra", "Translatons Tx,Ty,Tz", GH_ParamAccess.list, new List<bool> { false, false, false });

            pManager[0].Optional = true;
            pManager[2].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PTK Supports", "S (PTK)", "Support data to be send to Assembler(PTK)", GH_ParamAccess.item);
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
            List<Point3d> lPts = new List<Point3d>();
            List<Plane> supPlns = new List<Plane>();
            List<bool> lRot = new List<bool> { false, false, false };
            List<bool> lTra = new List<bool> { false, false, false };
            List<Supports> sups = new List<Supports>();
            #endregion

            #region input
            // DA.GetData(0, ref Tag);
            DA.GetData(0, ref lCase);
            if (!DA.GetDataList(1, lPts)) { return; }
            DA.GetDataList(2, supPlns);
            #endregion

            #region solve
            /*
            for (int i = 0; i < lPts.Count; i++)
            {

            }
            */

            // Supports PTKsupports = new Supports(Tag, lPt, lrot, ltra);

            // Karamba.Supports.Support news = 
            // new Karamba.Supports.Support(new Point3d(0, 0, 0), new List<bool> { false, false, false, false, false, false }, new Plane(new Point3d(0, 0, 0), new Vector3d(0, 0, 1)));

            #endregion
            Message = boolSupString;
            #region output
            DA.SetData(0, sups);
            // DA.SetData(1, new GH_Support(news));
            #endregion
        }

        
        public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalMenuItems(menu);
            Menu_AppendItem(menu, "Support Settings(&S)...", Menu_CustomOnClick);
        }
        
        // Custom Menu Extension (Right-Clicking at the center of the component)
        private void Menu_CustomOnClick(Object sender, EventArgs e)
        {
            if (boolSupString == "") boolSupString = "000000";
            Forms.F01_Supports frm = new Forms.F01_Supports(boolSupString);
            frm.BoolSupString = boolSupString;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                boolSupString = frm.BoolSupString;
                boolSupArray = Supports.StringToArray(boolSupString);
                
                // ExpireSolution: once form is set, ExpireSolution(true) tells GH 
                // that this components and its downstreams need recalculations.
                ExpireSolution(true);
            }
        }
        

        // Data saving function
        public override bool Write(GH_IWriter writer)
        {
            writer.SetString("boolSupString", boolSupString);
            return base.Write(writer);
        }

        // Data reading function
        public override bool Read(GH_IReader reader)
        {
            boolSupString = reader.GetString("boolSupString");
            return base.Read(reader);
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
                return PTK.Properties.Resources.icontest12;
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
