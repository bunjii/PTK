using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PTK
{
    public class PTK_3_Element : GH_Component
    {

        public PTK_3_Element()
          : base("Element", "Element",
              "creates a beam element.",
              CommonProps.category, CommonProps.subcate2)
        {
            Message = CommonProps.initialMessage;
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Tag", "T", "Add tags to the structure here.", GH_ParamAccess.item);
            pManager.AddCurveParameter("Base Curve", "C", "Add curves that shall be materalized", GH_ParamAccess.item);
            pManager.AddParameter(new Param_CrossSection(), "Cross Sections", "S", "Add the cross-section componentt here", GH_ParamAccess.list);
            pManager.AddParameter(new Param_Alignment(), "Alignment", "A", "Describes the alignment of the member. (Rotation and offset)", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Intersect Other", "I", "Whether this element intersects other members at other than the end point", GH_ParamAccess.item, true);

            pManager[0].Optional = true;
            pManager[3].Optional = true;
            pManager[4].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.RegisterParam(new Param_Element1D(), "Element", "E", "PTK Elements", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            #region variables
            string tag = null;
            Curve curve = null;
            List<CrossSection> sections = null;
            Alignment align = null;
            bool intersect = true;
            #endregion

            #region input
            if (!DA.GetData(0, ref tag)) { return; }
            if (!DA.GetData(1, ref curve)) { return; }
            if (!DA.GetDataList(2, sections))
            {
                sections = new List<CrossSection>();
            }
            if (!DA.GetData(3, ref align))
            {
                align = new Alignment();
            }
            if (!DA.GetData(4, ref intersect)) { return; }
            #endregion

            #region solve
            GH_Element1D elem = new GH_Element1D(new Element1D(tag, curve, sections, align, intersect));
            #endregion

            #region output
            DA.SetData(0, elem);
            #endregion
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return PTK.Properties.Resources.ico_materializer;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("0f259d4d-3cf2-4337-9545-c392178e1fe1"); }
        }
    }
}
