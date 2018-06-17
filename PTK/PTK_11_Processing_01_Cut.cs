using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;


namespace PTK
{
    public class PTK_11_Processing_01_Cut : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the PTK_11_Processing_01_Cut class.
        /// </summary>
        public PTK_11_Processing_01_Cut()
          : base("Cut", "Cut",
              "Description",
              "PTK", "BTL-processings")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Assembly", "", "", GH_ParamAccess.item);
            pManager.AddPlaneParameter("Cut Plane", "", "", GH_ParamAccess.item);
            pManager.AddIntegerParameter("ElemID", "", "", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("BTL-Cut", "", "", GH_ParamAccess.list);
            pManager.AddCurveParameter("Cutline", "", "", GH_ParamAccess.item);
            pManager.AddPlaneParameter("RefSide", "", "", GH_ParamAccess.item);
            pManager.AddCurveParameter("RefEdge", "", "", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Assembly Assembly = new Assembly();
            Plane cutPlane = new Plane();
            int ElemId = 0;

            DA.GetData(0, ref Assembly);
            DA.GetData(1, ref cutPlane);
            DA.GetData(2, ref ElemId);

            


            Element elem = Assembly.Elems.Find(t => t.ID ==ElemId);
            

            if (elem != null)
            {
                Plane RefSide = elem.BTLRef.RefSide1;
                Line refEdge = elem.BTLRef.RefEdge1;

                Point3d intersectPoint = Rhino.Geometry.Intersect.Intersection.CurvePlane(refEdge.ToNurbsCurve(), cutPlane, 0.01)[0].PointA;
                Line directionLine = new Line();
                if (Rhino.Geometry.Intersect.Intersection.PlanePlane(RefSide, cutPlane, out directionLine)) ;
                





                ComponentTypeProcessings Processing = new ComponentTypeProcessings();
                JackRafterCutType JackRafterCut = new JackRafterCutType();

                

                JackRafterCut.ReferencePlaneID = 1;
                JackRafterCut.Process = BooleanType.yes;
                JackRafterCut.StartX = refEdge.From.DistanceTo(intersectPoint);
                JackRafterCut.StartY = 0.0;
                JackRafterCut.StartDepth = 0.0;
                JackRafterCut.Angle = Vector3d.VectorAngle(refEdge.Direction, directionLine.Direction);
                JackRafterCut.Angle = Convert.ToDouble( Rhino.RhinoMath.ToDegrees(JackRafterCut.Angle));
                JackRafterCut.Inclination = 90.0;
                JackRafterCut.Orientation = OrientationType.end;
                JackRafterCut.StartDepth = 0.0;



                JackRafterCut.Name = Convert.ToString(ElemId);
                List<ProcessingBaseType> Base = new List<ProcessingBaseType>();
                

                List<ProcessingType> Process = new List<ProcessingType>();
                Process.Add(JackRafterCut);
                

                DA.SetDataList(0, Process);
                DA.SetData(1, directionLine);
                DA.SetData(2, RefSide);
                DA.SetData(3, refEdge);
            }


            

            


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
            get { return new Guid("58491a45-aace-430f-bb1c-c01cd1182a26"); }
        }
    }
}