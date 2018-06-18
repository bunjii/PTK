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
            pManager.AddGenericParameter("A", "Assembly", "", GH_ParamAccess.item);
            pManager.AddPlaneParameter("P", "CutPlane", "", GH_ParamAccess.item);
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
            Plane cutPlane = new Plane();
            int ElemId = 0;
            List<int> ElemIDs = new List<int>();

            DA.GetData(0, ref Assembly);
            DA.GetData(1, ref cutPlane);
            DA.GetDataList(2, ElemIDs);

            List<BTLprocess> Processes = new List<BTLprocess>();

            foreach(int ElemID in ElemIDs)
            {
                Element elem = Assembly.Elems.Find(t => t.ID == ElemId);
                //Element elem = Assembly.Elems[0];

                if (elem != null)
                {
                    Plane RefSide = elem.BTLRef.RefSide1;
                    Line refEdge = elem.BTLRef.RefEdge1;


                    Point3d intersectPoint = Rhino.Geometry.Intersect.Intersection.CurvePlane(refEdge.ToNurbsCurve(), cutPlane, 0.01)[0].PointA;
                    Line directionLine = new Line();
                    if (Rhino.Geometry.Intersect.Intersection.PlanePlane(RefSide, cutPlane, out directionLine)) ;


                    OrientationType orientation;
                    cutPlane = BTLref.AlignInputPlane(refEdge, RefSide, cutPlane, out orientation);
                    Plane inclinationplane = new Plane(cutPlane.Origin, cutPlane.YAxis, cutPlane.ZAxis);





                    Vector3d RefVector = refEdge.Direction;
                    Vector3d Cutvector = cutPlane.YAxis;
                    
                    List<Point3d> voidpoints = elem.BTLRef.StartPoints; //Adding startpoints to be included in the cuttingbox


                    if (orientation == OrientationType.end)
                    {

                        RefVector.Reverse();
                        Cutvector.Reverse(); //Correct
                        voidpoints = elem.BTLRef.Endpoints;  //Adding endpoints to be included in the cuttingbox

                    }

                    //Adding points to ensure the cuttingbox includes the crossesction of the cut
                    voidpoints.Add(Rhino.Geometry.Intersect.Intersection.CurvePlane(elem.BTLRef.RefEdge1.ToNurbsCurve(), cutPlane, 0.1)[0].PointA);
                    voidpoints.Add(Rhino.Geometry.Intersect.Intersection.CurvePlane(elem.BTLRef.RefEdge2.ToNurbsCurve(), cutPlane, 0.1)[0].PointA);
                    voidpoints.Add(Rhino.Geometry.Intersect.Intersection.CurvePlane(elem.BTLRef.RefEdge3.ToNurbsCurve(), cutPlane, 0.1)[0].PointA);
                    voidpoints.Add(Rhino.Geometry.Intersect.Intersection.CurvePlane(elem.BTLRef.RefEdge4.ToNurbsCurve(), cutPlane, 0.1)[0].PointA);

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
                    JackRafterCut.Name = Convert.ToString(ElemId);    //Name is used as container for elemId identifier

                    Processes.Add( new BTLprocess(JackRafterCut, Brep.CreateFromBox(box)));

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
            get { return new Guid("58491a45-aace-430f-bb1c-c01cd1182a26"); }
        }
    }
}