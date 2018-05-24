using feb;
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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

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
            // pManager.AddGenericParameter("Karamba Model", "Model", "", GH_ParamAccess.item);
            pManager.AddParameter(new Param_Model(), "Model", "Model", "Karamba Model after Analysis", GH_ParamAccess.item);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Dimension Data", "Dim (PTK)", "", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            #region variables
            GH_Model inGHModel = new GH_Model();

            #endregion

            #region input
            if (!DA.GetData(0, ref inGHModel)) return;
            #endregion

            #region solve
            Karamba.Models.Model model = inGHModel.Value;

            // if model is not calculated appropreately, does nothing further.
            if (model.maxDisp < 0) return;

            // clone model, element, analysis data to avoid side effects
            try
            {
                Functions_DDL.CloneKarambaModel(ref model);
            }
            catch (Exception e)
            {
                Debug.WriteLine("&&&&&&&&&&&&&\n" + e.ToString());
            }

            // Debug.WriteLine(model.element_selector.select("G1").Count.ToString());
            #endregion

            #region output
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