
using GH_IO.Serialization;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;

using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

using Karamba.Models;
using Karamba.Elements;
namespace PTK.Components
{
    public class PTK_4_3_DimensioningMembers : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the _4_3_DimensioningMembers class.
        /// </summary>
        public PTK_4_3_DimensioningMembers()
          : base("Dimensioning Members", "Dimensioning",
              "Description",
              CommonProps.category, CommonProps.subcat4)
        {
            Message = CommonProps.initialMessage;
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("PTK Assembly", "A (PTK)", "PTK Assembly", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PTK Assembly", "A (PTK)", "Assembled project data", GH_ParamAccess.item);
            pManager.AddTextParameter("OUT information", "info", "temporary information from analysis", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            #region variables
            Assembly inassembly;
            GH_ObjectWrapper wrapAssembly = new GH_ObjectWrapper();
            List<string> infolist = new List<string>();
            #endregion

            #region input
            if (!DA.GetData(0, ref wrapAssembly)) { return; }
            wrapAssembly.CastTo<Assembly>(out inassembly);
            #endregion

            #region solve

            #endregion

            #region output
            Assembly outassembly = inassembly;
            DA.SetData(0, outassembly);
            DA.SetDataList(0, infolist);
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
                return PTK.Properties.Resources.ico_dimensioning;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("6639ee92-b0e1-4005-8df8-94d01ab78237"); }
        }
    }
}