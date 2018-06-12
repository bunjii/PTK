using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

using Karamba.Models;
using Karamba.Elements;


namespace PTK
{
    public class PTK_5_GlobalModel : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the PTK_C_01 class.
        /// </summary>
        public PTK_5_GlobalModel()
          : base("Global Model (PTK)", "Global",
              "Combine PTK class and Karamba Analysis Data",
              CommonProps.category, CommonProps.subcat1)
        {
            Message = CommonProps.initialMessage;
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("PTK Assembly", "A (PTK)", "PTK Assembly", GH_ParamAccess.item);
            pManager.AddGenericParameter("Dimension Data", "Dim (PTK)", "", GH_ParamAccess.item);

            pManager[1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PTK Assembly", "A (PTK)", "PTK Assembly", GH_ParamAccess.item);
            // pManager.AddLineParameter("lines", "lines", "lines", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            #region variables
            List<Node> nodes = new List<Node>();
            List<PTK_Element> elems = new List<PTK_Element>();
            List<PTK_Material> mats = new List<PTK_Material>();
            List<Section> secs = new List<Section>();
            List<PTK_Support> sups = new List<PTK_Support>();
            List<PTK_Load> loads = new List<PTK_Load>();
            GH_ObjectWrapper wrapAssembly = new GH_ObjectWrapper();
            Assembly assemble;
            #endregion

            #region input
            if (!DA.GetData(0, ref wrapAssembly)) { return; }
            #endregion

            #region solve

            wrapAssembly.CastTo<Assembly>(out assemble);

            nodes = assemble.Nodes;
            elems = assemble.Elems;
            mats = assemble.Mats;
            secs = assemble.Secs;
            sups = assemble.Sups;

            Assembly outAssemble = new Assembly(nodes, elems, mats, secs, sups, loads);
            
            #endregion

            #region output
            DA.SetData(0, outAssemble);

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
                return PTK.Properties.Resources.ico_global;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("3cf82ec9-1233-4aa7-b233-a467fcf8c41b"); }
        }
    }
}