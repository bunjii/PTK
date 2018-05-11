using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;


namespace PTK.Components
{
    public class PTK_9_2_FinishedModel : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the _10_1_FinishedModel class.
        /// </summary>
        public PTK_9_2_FinishedModel()
          : base("Finished Model (PTK)", "Finished",
              "Description",
              "PTK", "Assemble")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("PTK Assembly", "A (PTK)", "PTK Assembly", GH_ParamAccess.item);
            pManager.AddGenericParameter("PTK Local Analysis", "LA (PTK)", "PTK Local Analysis", GH_ParamAccess.item);

            pManager[1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PTK Assembly", "A (PTK)", "PTK Assembly", GH_ParamAccess.item);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            #region variables
            List<Node> nodes = new List<Node>();
            List<Element> elems = new List<Element>();
            List<Material> mats = new List<Material>();
            List<Section> secs = new List<Section>();
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

            Assembly outAssemble = new Assembly(nodes, elems, mats, secs);

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
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("406838aa-56ab-4dc9-b1d0-3969191deacd"); }
        }
    }
}