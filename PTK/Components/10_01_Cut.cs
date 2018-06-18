using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization;

namespace PTK.Components
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
            pManager.AddGenericParameter("A", "Assembly", "", GH_ParamAccess.item);
            pManager.AddPlaneParameter("P", "CutPlane", "", GH_ParamAccess.list);
            pManager.AddIntegerParameter("ID", "ElemID", "", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("B", "BTL-Cut", "", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Assembly Assembly = new Assembly();
            List<Plane> cutPlanes = new List<Plane>();
            
            List<int> ElemIDs = new List<int>();

            DA.GetData(0, ref Assembly);
            DA.GetDataList(1, cutPlanes);
            DA.GetDataList(2, ElemIDs);

            List<BTLprocess> Processes = new List<BTLprocess>();
            int i = 0;
            foreach (int ElemID in ElemIDs)
            {
                Plane cutPlane = cutPlanes[i];
                PTK_Element elem = Assembly.Elems.Find(t => t.Id == ElemID);
                //Element elem = Assembly.Elems[0];

                if (elem != null)
                {
                    //In the PTK1.0 here will be a loop to go through multiple btl-elements
                    RefSides Refsides = elem.SubElementBTL[0].Refsides;
                    Plane RefSide = Refsides.RefSide1;
                    Line refEdge = Refsides.RefEdge1;
                    

                    Point3d intersectPoint = Rhino.Geometry.Intersect.Intersection.CurvePlane(refEdge.ToNurbsCurve(), cutPlane, 0.01)[0].PointA;
                    Line directionLine = new Line();
                    if (Rhino.Geometry.Intersect.Intersection.PlanePlane(RefSide, cutPlane, out directionLine)) ;


                    OrientationType orientation;

                    cutPlane = SubElementBTL.AlignInputPlane(refEdge, RefSide, cutPlane, out orientation);
                    Plane inclinationplane = new Plane(cutPlane.Origin, cutPlane.YAxis, cutPlane.ZAxis);





                    Vector3d RefVector = refEdge.Direction;
                    Vector3d Cutvector = cutPlane.YAxis;

                    List<Point3d> voidpoints = Refsides.StartPoints; //Adding startpoints to be included in the cuttingbox


                    if (orientation == OrientationType.end)
                    {

                        RefVector.Reverse();
                        Cutvector.Reverse(); //Correct
                        voidpoints = Refsides.EndPoints;  //Adding endpoints to be included in the cuttingbox

                    }

                    //Adding points to ensure the cuttingbox includes the crossesction of the cut
                    voidpoints.Add(Rhino.Geometry.Intersect.Intersection.CurvePlane(Refsides.RefEdge1.ToNurbsCurve(), cutPlane, 0.1)[0].PointA);
                    voidpoints.Add(Rhino.Geometry.Intersect.Intersection.CurvePlane(Refsides.RefEdge2.ToNurbsCurve(), cutPlane, 0.1)[0].PointA);
                    voidpoints.Add(Rhino.Geometry.Intersect.Intersection.CurvePlane(Refsides.RefEdge3.ToNurbsCurve(), cutPlane, 0.1)[0].PointA);
                    voidpoints.Add(Rhino.Geometry.Intersect.Intersection.CurvePlane(Refsides.RefEdge4.ToNurbsCurve(), cutPlane, 0.1)[0].PointA);

                    Box box = new Box(cutPlane, voidpoints);

                    //Creating BTL processing
                    JackRafterCutType JackRafterCut = new JackRafterCutType();

                    JackRafterCut.Orientation = orientation;
                    JackRafterCut.ReferencePlaneID = 1;
                    JackRafterCut.Process = BooleanType.yes;
                    JackRafterCut.StartX = refEdge.From.DistanceTo(intersectPoint);
                    JackRafterCut.StartY = 0.0;
                    JackRafterCut.StartDepth = 0.0;
                    JackRafterCut.Angle = Vector3d.VectorAngle(RefVector, cutPlane.XAxis);
                    JackRafterCut.Angle = Convert.ToDouble(Rhino.RhinoMath.ToDegrees(JackRafterCut.Angle));
                    JackRafterCut.Inclination = Vector3d.VectorAngle(RefVector, Cutvector);
                    JackRafterCut.Inclination = Convert.ToDouble(Rhino.RhinoMath.ToDegrees(JackRafterCut.Inclination));
                    JackRafterCut.StartDepth = 0.0;
                    JackRafterCut.Name = Convert.ToString(ElemID);    //Name is used as container for elemId identifier

                    Processes.Add(new BTLprocess(JackRafterCut, Brep.CreateFromBox(box), ElemID));
                    i++;
                }

                DA.SetDataList(0, Processes);

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
            get { return new Guid("afa494ab-ab34-4eb2-87ca-4003bb90439c"); }
        }
    }
}