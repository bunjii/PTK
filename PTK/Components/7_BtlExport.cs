﻿using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace PTK
{
    public class PTK_7_BtlExport : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the PTK_C_06 class.
        /// </summary>
        public PTK_7_BtlExport()
          : base("BTL EXPORTER (PTK)", "Export BTL",
              "Exporting BTL file to the designated location",
              CommonProps.category, CommonProps.subcate7)
        {
            Message = CommonProps.initialMessage;
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("PTK Assembly", "A (PTK)", "PTK DATA INPUT", GH_ParamAccess.item);
            pManager.AddTextParameter("File Path", "Path", "FILE LOCATION OF EXPORTED BTL FILE", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Enable?", "Enable?", "ENABLE EXPORTING?", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
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
                return PTK.Properties.Resources.ico_BTL;

            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("a638c80f-bcd6-4ecd-a075-9dc9a9c73a98"); }
        }
    }
}