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

using Karamba.Supports;
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
            pManager.AddIntegerParameter("Load Case", "LC", "Load case", GH_ParamAccess.item, 0); 
            pManager.AddPlaneParameter("Plane", "plane", "", GH_ParamAccess.list);
            
            pManager[0].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PTK Supports", "Sup (PTK)", "Support data to be send to Assembler(PTK)", GH_ParamAccess.item);
            pManager.RegisterParam(new Karamba.Supports.Param_Support(), "Support", "supp", "Ouput support(s)");
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
            List<Plane> supPlns = new List<Plane>();
            List<Support> sups = new List<Support>();
            List<Karamba.Supports.GH_Support> sups_GH_krmb = new List<Karamba.Supports.GH_Support>();
            #endregion

            #region input
            if (!DA.GetDataList(1, supPlns)) { return; }
            DA.GetData(0, ref lCase);
            #endregion

            #region solve
            for (int i=0;i<supPlns.Count;i++)
            {
                Support tmpSup = new Support(lCase, supPlns[i], new List<bool>(boolSupArray));
                sups.Add(tmpSup);
                Karamba.Supports.Support tmp_sup = new Karamba.Supports.Support(supPlns[i].Origin, new List<bool>(boolSupArray), supPlns[i]);
                Karamba.Supports.GH_Support krmb_sup = new Karamba.Supports.GH_Support(tmp_sup);
                sups_GH_krmb.Add(krmb_sup);
            }
            #endregion

            #region output
            Message = boolSupString;
            DA.SetData(0, sups);
            DA.SetData(1, sups_GH_krmb);
            #endregion
        }

        public override void CreateAttributes()
        {
            // base.CreateAttributes();
            m_attributes = new Attributes_Custom(this);
        }

        public static void Menu_CustomOnClick(PTK_2_2_Supports _comp)
        {
            if (_comp.boolSupString == "") _comp.boolSupString = "000000";
            Forms.F01_Supports frm = new Forms.F01_Supports(_comp.boolSupString);
            frm.BoolSupString = _comp.boolSupString;

            if (frm.ShowDialog() == DialogResult.OK)
            {
                _comp.boolSupString = frm.BoolSupString;
                _comp.boolSupArray = Support.StringToArray(_comp.boolSupString);
                
                _comp.ExpireSolution(true);
            }
        }

        public class Attributes_Custom : GH_ComponentAttributes
        {
            private Rectangle ButtonBounds { get; set; }

            public Attributes_Custom(GH_Component owner) : base(owner) { }

            protected override void Layout()
            {
                base.Layout();

                Rectangle rec0 = GH_Convert.ToRectangle(Bounds);
                rec0.Height += 26;

                Rectangle rec1 = rec0;
                rec1.Y = rec1.Bottom - 26;
                rec1.Height = 26;
                rec1.Inflate(-4, -4);
                
                Bounds = rec0;
                ButtonBounds = rec1;
                
            }

            protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
            {
                base.Render(canvas, graphics, channel);
                if (channel == GH_CanvasChannel.Objects)
                {
                    GH_Capsule button = GH_Capsule.CreateTextCapsule
                        (ButtonBounds, ButtonBounds, GH_Palette.Black, "Set Supports", 2, 0);
                    button.Render(graphics, Selected, Owner.Locked, false);
                    button.Dispose();
                }
            }

            public override GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e)
            {
                if (e.Button == MouseButtons.Left)
                {
                    RectangleF rec = ButtonBounds;
                    if (rec.Contains(e.CanvasLocation))
                    {
                        Menu_CustomOnClick((PTK_2_2_Supports)Owner);
                        return GH_ObjectResponse.Handled;
                    }
                }
                return base.RespondToMouseDown(sender, e);
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
            // set boolSupArray values when it loads.
            this.boolSupArray = Support.StringToArray(boolSupString);
            return base.Read(reader);
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
