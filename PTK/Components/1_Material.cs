﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace PTK
{
    public class PTK_1_Material : GH_Component
    {

        int i = 0;

        public PTK_1_Material()
          : base("Material", "Mat","Create a Material",
              CommonProps.category, CommonProps.subcate1)
        {
            Message = CommonProps.initialMessage;
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name","N", "Material Name", GH_ParamAccess.item);
            pManager.AddParameter(new Param_MaterialStructuralProp(), "Structural Material Prop", "SMP", "Add material properties here", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.RegisterParam(new Param_Material(), "Material", "M", "MaterialData to be connected with Materializer Component");
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            #region variables
            string name = null;
            GH_MaterialStructuralProp gProp = null;
            MaterialStructuralProp prop = null;
            #endregion

            #region input
            if (!DA.GetData(0, ref name)) { return; }
            if (!DA.GetData(1, ref gProp)) {
                prop = new MaterialStructuralProp();
            }
            else
            {
                prop = gProp.Value;
            }
            #endregion

            #region solve
            GH_Material material = new GH_Material(new Material(name, prop));
            #endregion

            #region output
            DA.SetData(0, material);
            #endregion
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return PTK.Properties.Resources.ico_material;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("911bef7b-feea-46d8-abe9-f686d11b9c41"); }
        }
    }
}