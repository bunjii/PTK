using Grasshopper.Kernel;
using Karamba;
using Karamba.Algorithms;
using Karamba.Algorithms.BESOShell;
using Karamba.Algorithms.Deprecated;
using Karamba.Algorithms.GUI;
using Karamba.CrossSections;
using Karamba.CrossSections.Deprecated;
using Karamba.CrossSections.GUI;
using Karamba.Elements;
using Karamba.Elements.Deprecated;
using Karamba.Elements.GUI;
using Karamba.Exporters;
using Karamba.Exporters.Deprecated;
using Karamba.Exporters.GUI;
using Karamba.Licenses;
using Karamba.Loads;
using Karamba.Loads.Deprecated;
using Karamba.Loads.GUI;
using Karamba.Materials;
using Karamba.Materials.Deprecated;
using Karamba.Materials.GUI;
using Karamba.Models;
using Karamba.Models.Deprecated;
using Karamba.Models.GUI;
using Karamba.Nodes;
using Karamba.Results;
using Karamba.Results.Deprecated;
using Karamba.Results.GUI;
using Karamba.Supports;
using Karamba.Supports.Deprecated;
using Karamba.Supports.GUI;
using Karamba.Utilities;
using Karamba.Utilities.AABBTrees;
using Karamba.Utilities.Components;
using Karamba.Utilities.Deprecated;
using Karamba.Utilities.Geometry.Mesh;
using Karamba.Utilities.GUI;
using Karamba.Utilities.Mappings;
using Karamba.Utilities.Mappings.GUI;
using Karamba.Utilities.UIWidgets;
using Karamba.Utilities.UIWidgets.switcher;
using Rhino.Geometry;
using Rhino.Geometry;
using System;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;


namespace PTK.Components
{
    public class SandBox : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the SandBox class.
        /// </summary>
        public SandBox()
          : base("SandBox", "Nickname",
              "Description",
              "Category", "Subcategory")
        {
            Message = "";
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {

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
            int a = 2;
            int b = 3;
            Message = ((a + b) * 2).ToString();
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
            get { return new Guid("71b91c48-f9a3-4021-9f7d-bab6431cdd6f"); }
        }
    }
}