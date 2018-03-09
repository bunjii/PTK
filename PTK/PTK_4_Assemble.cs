using System;
// using System.Linq;
using System.Collections.Generic;

// using System.Runtime.InteropServices;
using System.Windows.Forms;

// using System.Diagnostics;
// using System.IO;
// using System.Threading.Tasks;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace PTK
{
   
    public class PTK4_Assemble : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public PTK4_Assemble()
          : base("2", "2",
              "Assemble",
              "PTK", "2_ASSEMBLE")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Element", "E", "Add elements here", GH_ParamAccess.list);
            pManager.AddGenericParameter("Supports", "S", "Add Supports here", GH_ParamAccess.list);
            pManager.AddGenericParameter("Loads", "L", "Add Loads here", GH_ParamAccess.list);

            pManager[1].Optional = true;
            pManager[2].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Assembly", "A", "AssemblyObjectContaining the whole project", GH_ParamAccess.item);
            pManager.AddPointParameter("Point", "", "", GH_ParamAccess.list);
            pManager.AddBrepParameter("Breptest", "", "", GH_ParamAccess.list);
            pManager.AddTextParameter("ID", "", "", GH_ParamAccess.list);
            pManager.AddCurveParameter("CenterCurve", "", "", GH_ParamAccess.list);
            pManager.AddNumberParameter("CenterCurve", "", "", GH_ParamAccess.list);
            pManager.AddTextParameter("Neighbours", "", "", GH_ParamAccess.list);
            pManager.AddLineParameter("Lines", "", "", GH_ParamAccess.list);
            pManager.AddTextParameter("SubID", "", "", GH_ParamAccess.list);


        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Node.ResetIDCount();
            #region variables
            int elemIdNum = 0;

            //Assigning lists off objects
            List<Node> nodes = new List<Node>();
            List<Element> elems = new List<Element>();
            List<Section> rectSecs = new List<Section>();
            List<Element> tempElemList = new List<Element>();
            

            List<GH_ObjectWrapper> wrapElemList = new List<GH_ObjectWrapper>();
            #endregion

            #region input
            if (!DA.GetDataList(0, wrapElemList)) { return; }
            #endregion

            #region solve

            // DDL "unwrap wrapped element class" and "merge multiple element class"
            for (int i = 0; i < wrapElemList.Count; i++)
            {
                wrapElemList[i].CastTo<List<Element>>(out tempElemList);
                elems.AddRange(tempElemList);
            }

            // DDL "generate Elem ID"  // John: I think the ID-asignment should be done inside the class
            for (int i = 0; i < elems.Count; i++)
            {
                elems[i].ID = elemIdNum;
                elemIdNum++;
            }

            //Adding a list of points.
            //Adding Endpoints
            //Adding StartPoints
            //Adding Intersections
            //Removing duplicates
            //Asigning to nodes
            List<Point3d> TempPoint = new List<Point3d>();
            

            //ADDING NODES
            //COMMENT JOHN: I thought it was more safe to first create a unique set of points, then assign it as nodes

            //This loop is finding all points and add it to the TempPointList
            //First by adding end/start point
            //Then it finds the intersections and add text if 
            for (int i = 0; i < elems.Count; i++)
            {
                TempPoint.Add(elems[i].Crv.PointAtEnd);
                TempPoint.Add(elems[i].Crv.PointAtStart);

                
                for (int j = i; j<elems.Count; j++)
                {
                    var events = Rhino.Geometry.Intersect.Intersection.CurveCurve(elems[i].Crv, elems[j].Crv, ProjectProperties.tolerances, ProjectProperties.tolerances);
                    if (events!= null)
                    {
                        for (int k =0; k<events.Count; k++)
                        {
                            TempPoint.Add(events[k].PointA);
                        }
                    }
                }
            }

            //Removing all duplicates. With a tolerance. Tolerance is best to be set as low as possible. May be an input. Or one can use the rhino unit tolerance
            TempPoint = Functions.RemoveDuplicates(TempPoint, 0.001);

            // DDL "generate Node"
            for (int i = 0; i < TempPoint.Count; i++)
            {
                nodes.Add(new Node(TempPoint[i]));
            }
  
            
            List<Brep> BokseTest = new List<Brep>();
            List<Curve> elementCurves = new List<Curve>();
            List<int> elementid = new List<int>();

            Functions.AsignNeighbour(elems, nodes);
            List<Line> strLine = new List<Line>();
            List<String> SubID = new List<String>();
        

            //Testing, making breps
            for (int i = 0; i < elems.Count; i++)
            {

                BokseTest.Add(elems[i].MakeBrep());
                elementCurves.Add(elems[i].Crv);
                
                elementid.Add(elems[i].ID);
                string tempID = Convert.ToString(elems[i].ID);


                for (int j = 0; j < elems[i].SubStructural.Count; j++)
                {
                    strLine.Add(elems[i].SubStructural[j].StrctrLine);
                    SubID.Add(tempID + "_" + Convert.ToString(elems[i].SubStructural[j].StrctrlLineID));
                }

            }

            List<string> IDs = new List<string>();
            List<Point3d> PointNodes = new List<Point3d>();
            List<string> NeighbourList = new List<string>();
            for (int i = 0; i < nodes.Count; i++)
            {

                IDs.Add(Convert.ToString( nodes[i].ID));
                PointNodes.Add(nodes[i].Pt3d);
                string text ="N"+ Convert.ToString(nodes[i].ID)+"_";
                for (int j = 0; j < nodes[i].ElemIds.Count; j++)
                {
                    text += "E:" +Convert.ToString(nodes[i].ElemIds[j])+" ";
                }
                NeighbourList.Add(text);


            }

            


            #endregion

            #region output

            Assembly Assembly = new Assembly(nodes, elems);

            DA.SetData(0, Assembly);
            DA.SetDataList(1, TempPoint);
            DA.SetDataList(2, BokseTest);
            DA.SetDataList(3, IDs);
            DA.SetDataList(4, elementCurves);
            DA.SetDataList(5, elementid);
            DA.SetDataList(6, NeighbourList);
            DA.SetDataList(7, strLine);
            DA.SetDataList(8, SubID);
            #endregion




        }



        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                // return null;
                return PTK.Properties.Resources.icontest11;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("d16b2f49-a170-4d47-ae63-f17a4907fed1"); }
        }
    }
}
