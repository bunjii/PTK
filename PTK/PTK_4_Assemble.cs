﻿using System;
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
            pManager.AddGenericParameter("DetailingGroupDescriptions", "DGD", "Add detailinggroupDescriptions here", GH_ParamAccess.list);


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
            pManager.AddCurveParameter("", "", "", GH_ParamAccess.item);
            pManager.AddPointParameter("", "", "", GH_ParamAccess.item);


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


            //Assigning lists off objects
            List<Node> nodes = new List<Node>();
            List<Element> elems = new List<Element>();
            List<Section> rectSecs = new List<Section>();
            List<Element> tempElemList = new List<Element>();
            List<DetailingGroup> DetailingGroup = new List<DetailingGroup>();
            

            List<GH_ObjectWrapper> wrapElemList = new List<GH_ObjectWrapper>();
            #endregion

            #region input
            if (!DA.GetDataList(0, wrapElemList)) { return; }
            DA.GetDataList(3, DetailingGroup);

            #endregion

            #region solve

            // DDL "unwrap wrapped element class" and "merge multiple element class"
            for (int i = 0; i < wrapElemList.Count; i++)
            {
                wrapElemList[i].CastTo<List<Element>>(out tempElemList);
                elems.AddRange(tempElemList);
            }

            // DDL "generate Elem ID"  // John: I think the ID-asignment should be done inside the class


            //Adding a list of points.
            //Adding Endpoints
            //Adding StartPoints
            //Adding Intersections
            //Removing duplicates
            //Asigning to nodes
            List<Point3d> TempPoint = new List<Point3d>();


            Functions.Assemble(elems, out nodes);
            Functions.GenerateStructuralLines(elems);
            
  
            


            List<Brep> BokseTest = new List<Brep>();
            List<Curve> elementCurves = new List<Curve>();
            List<int> elementid = new List<int>();
            List<int> ConnectedNodes = new List<int>();
            List<Line> strLine = new List<Line>();
            List<String> SubID = new List<String>();
        

            //Testing, making breps
            for (int i = 0; i < elems.Count; i++)
            {

                BokseTest.Add(elems[i].ElementGeometry);
                elementCurves.Add(elems[i].Crv);
                ConnectedNodes.Add(elems[i].ConnectedNodes);
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



            for (int i = 0; i < DetailingGroup.Count; i++)
            {
                DetailingGroup[i].assignDetails(nodes, elems);
            }


            #endregion

            #region output




            Assembly Assembly = new Assembly(nodes, elems, DetailingGroup);

            DA.SetData(0, Assembly);
            DA.SetDataList(1, PointNodes);
            DA.SetDataList(2, BokseTest);
            DA.SetDataList(3, IDs);
            DA.SetDataList(4, elementCurves);
            DA.SetDataList(5, elementid);
            DA.SetDataList(6, ConnectedNodes);
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
