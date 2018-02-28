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
    public class PTK_old : GH_Component
    {

        /// <summary>
        /// Initializes a new instance of the TestC class.
        /// </summary>
        public PTK_old()
          : base("3", "3",
              "Test component no.3: Decompose Element",
              "PTK", "5_UTIL")
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
            pManager.AddCurveParameter("Curve", "Lines", "Lines", GH_ParamAccess.list);
            pManager.AddTextParameter("Tag", "Tag", "Tag", GH_ParamAccess.list);
            pManager.AddIntegerParameter("PTK ELEM ID", "PTK E ID", "PTK ELEM ID", GH_ParamAccess.list);
            pManager.AddIntegerParameter("PTK NODE ID 0", "PTK N0 ID", "PTK NODE ID 0", GH_ParamAccess.list);
            pManager.AddIntegerParameter("PTK NODE ID 1", "PKT N1 ID", "PTK NODE ID 1", GH_ParamAccess.list);
            pManager.AddGenericParameter("PTK SECTION", "PTK S", "PTK SECTION", GH_ParamAccess.list);
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
            List<string> elemTags = new List<string>();
            List<Curve> curves = new List<Curve>();
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
                curves.Add(e.Crv);
                elemTags.Add(e.Tag);
                elemids.Add(e.ID);
                n0ids.Add(e.N0id);
                n1ids.Add(e.N1id);
            }
            #endregion

            #region output
            DA.SetDataList(0, curves);
            DA.SetDataList(1, elemTags);
            DA.SetDataList(2, elemids);
            DA.SetDataList(3, n0ids);
            DA.SetDataList(4, n1ids);
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
                return PTK.Properties.Resources.icon_truss;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("891a0366-cf2f-4642-b92b-4a93d0389330"); }
        }

    }

}