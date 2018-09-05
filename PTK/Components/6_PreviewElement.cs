using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace PTK.Components
{
    public class PTK_6_PreviewElement : GH_Component
    {
        public PTK_6_PreviewElement()
          : base("Preview Element", "PrevElem",
              "Preview Element",
              CommonProps.category, CommonProps.subcate6)
        {
            Message = CommonProps.initialMessage;
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new Param_Element1D(), "Elements", "E", "Add elements here", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Model", "M", "3d model", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            #region variables
            GH_Element1D gElement = null;
            List<Brep> models = new List<Brep>();
            #endregion

            #region input
            if (!DA.GetData(0, ref gElement)) { return; }
            Element1D element = gElement.Value;
            #endregion

            #region solve
            List<Curve> secs = new List<Curve>();
            //
            List<CrossSection> crossSections = new List<CrossSection>();
            foreach (Sub2DElement subElement in element.Sub2DElements)
            {
                crossSections.Add(subElement.CrossSection);
            }
            //
            foreach (CrossSection crossSection in crossSections)
            {
                if(crossSection is RectangleCroSec recSec)
                {
                    secs.Add(new Rectangle3d(element.CroSecLocalPlane, 
                        new Interval(-crossSection.GetWidth(), crossSection.GetWidth()), 
                        new Interval(-crossSection.GetHeight(), crossSection.GetHeight())).ToNurbsCurve());
                }
            }
            foreach(Curve s in secs)
            {
                Brep[] breps = Brep.CreateFromSweep(element.BaseCurve, s, true, Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance);
                models.AddRange(breps);
            }
            #endregion

            #region output
            DA.SetDataList(0, models);
            #endregion
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return null;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("7da0c2a7-ccb0-4f9e-b383-43b74bf56375"); }
        }
    }
}