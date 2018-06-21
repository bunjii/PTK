
using GH_IO.Serialization;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;

using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

using System.Xml.Serialization;
using System.IO;

using Karamba.Models;
using Karamba.Elements;
/*
namespace PTK.Components
{
    class PTK_4_4_Utilization_preview : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the _4_3_DimensioningMembers class.
        /// </summary>
        public PTK_4_4_Utilization_preview()
          : base("Utilization preview", "Preview",
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
            get { return new Guid("6632ee12-b0e1-4005-8df8-94d01ab78237"); }
        }
    }
}
        /*
    double maxutilization = list_of_utilizations.Max();
            infolist.Add(maxutilization.ToString());
                    _breps.Add(e1.ElementGeometry);
                    var C = System.Drawing.Color.FromName("Blue");
            _materials.Add(new Rhino.Display.DisplayMaterial(C));
        private readonly List<Brep> _breps = new List<Brep>() ;
            private readonly List<Rhino.Display.DisplayMaterial> _materials = new List<Rhino.Display.DisplayMaterial>() ;
            private BoundingBox _box;

            public override BoundingBox ClippingBox
            {
                get { return _box; }
            }
            public override void DrawViewportMeshes(IGH_PreviewArgs args)
            {
                for (int i = 0; i < _breps.Count; i++)
                    args.Display.DrawBrepWires(_breps[i], _materials[i].Diffuse, 0);
            }
            public override void DrawViewportWires(IGH_PreviewArgs args)
            {
                for (int i = 0; i < _breps.Count; i++)
                    args.Display.DrawBrepShaded(_breps[i], _materials[i]);
            }

        */
   
    