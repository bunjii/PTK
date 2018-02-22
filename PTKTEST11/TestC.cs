using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;

using Rhino.Geometry;

namespace PTK
{
    public class PTK3 : GH_Component
    {

        /// <summary>
        /// Initializes a new instance of the TestC class.
        /// </summary>
        public PTK3()
          : base("3", "3",
              "Test component no.3: Decompose Element",
              "PTK", "STR")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("PTK ELEM", "PTK E", "PTK ELEM", GH_ParamAccess.item);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddLineParameter("Lines", "Lines", "Lines", GH_ParamAccess.list);
            pManager.AddIntegerParameter("PTK ELEM ID", "PTK E ID", "PTK ELEM ID", GH_ParamAccess.list);
            pManager.AddIntegerParameter("PTK NODE ID 0", "PTK N0 ID", "PTK NODE ID 0", GH_ParamAccess.list);
            pManager.AddIntegerParameter("PTK NODE ID 1", "PKT N1 ID", "PTK NODE ID 1", GH_ParamAccess.list);
        }


        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            #region variables
            GH_ObjectWrapper wrapElem = new GH_ObjectWrapper();
            List<Element> elems = new List<Element>();

            List<Line> lines = new List<Line>();
            List<int> elemids = new List<int>();
            List<int> n0ids = new List<int>();
            List<int> n1ids = new List<int>();
            #endregion

            #region input
            if (!DA.GetData(0, ref wrapElem)) { return;  }
            wrapElem.CastTo<List<Element>>(out elems);
            #endregion

            #region solve
            foreach (Element e in elems)
            {
                lines.Add(e.Ln);
                elemids.Add(e.ID);
                n0ids.Add(e.N0id);
                n1ids.Add(e.N1id);
            }
            #endregion

            #region output
            DA.SetDataList(0, lines);
            DA.SetDataList(1, elemids);
            DA.SetDataList(2, n0ids);
            DA.SetDataList(3, n1ids);
            #endregion

            #region messagebox
            this.Message = "P T K";
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
                return PTKTEST11.Properties.Resources.icon_truss;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("891a0366-cf2f-4642-b92b-4a93d0389330"); }
        }



        // DDL custom attributes, etc
        /*
        public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);
            Menu_AppendSeparator(menu);
            Menu_AppendItem(menu, "general", Menu_DoClick);
            Menu_AppendItem(menu, "post-process");
            Menu_AppendSeparator(menu);
        }

        private void Menu_DoClick(object sender, EventArgs e)
        {
            myBool = !myBool;
        }
        public bool myBool = false;
        */

        /*
        public override void CreateAttributes()
        {
            m_attributes = new Attributes_Custom(this);
        }
        */
    }

    /*
    public class Attributes_Custom : Grasshopper.Kernel.Attributes.GH_ComponentAttributes
    {
        public Attributes_Custom(GH_Component owner) : base(owner)
        {
        }
        
        // 1. making a layout
        protected override void Layout()
        {
            base.Layout();

            // rec0: area for the component incl. extended area
            Rectangle rec0 = GH_Convert.ToRectangle(Bounds);
            rec0.Height += 30;

            // rec1: only the extended area for buttons
            Rectangle rec1 = rec0;
            rec1.Y = rec1.Bottom - 30;
            rec1.Height = 30;
            rec1.Inflate(-2, -2);

            Bounds = rec0;
            ButtonBounds = rec1;
        }
        private Rectangle ButtonBounds { get; set; }

        // 2. render
        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            base.Render(canvas, graphics, channel);

            if (channel == GH_CanvasChannel.Objects)
            {
                GH_Capsule button = GH_Capsule.CreateTextCapsule(ButtonBounds, ButtonBounds, GH_Palette.Pink, "Button", 2, 0);
                button.Render(graphics, Selected, Owner.Locked, false);
                button.Dispose();
            }

        }

        // 3. define a mouse click event
        public override GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (e.Button == MouseButtons.Left)
            {
                RectangleF rec = ButtonBounds;
                if (rec.Contains(e.CanvasLocation))
                {
                    // MessageBox.Show("The button was clicked", "Button", MessageBoxButtons.OK);
                    return GH_ObjectResponse.Handled;
                }
            }
            return base.RespondToMouseDown(sender, e);
        }

    }
    */

}