using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace PTK
{
    public class PTK_1_1_Material : GH_Component
    {

        public PTK_1_1_Material()
          : base("Material (PTK)", "Mat",
              "creates a Material",
              CommonProps.category, CommonProps.subcat2)
        {
            Message = CommonProps.initialMessage;
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("MaterialName","Name", "Names the material", GH_ParamAccess.item, "UntitledMaterial");      //We should add default values here.
            pManager.AddParameter(new Param_MaterialStructuralProp(), "Structural Properties", "StrctProp", "Add material properties here", GH_ParamAccess.item);    //We should add default values here.
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.RegisterParam(new Param_Material(), "Material", "Mat", "MaterialData to be connected with Materializer Component");
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            #region variables
            string name = null;
            MaterialStructuralProp prop = null;
            #endregion

            #region input
            DA.GetData(0, ref name);
            DA.GetData(1, ref prop);
            #endregion

            #region solve
            #endregion

            #region output
            DA.SetData(0, new GH_Material(new Material(name, prop)));
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