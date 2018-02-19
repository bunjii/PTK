using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace PTKTEST11
{

    public class Node
    {
        #region fields
        private int ID;
        private double X;
        private double Y;
        private double Z;
        #endregion

        #region constructors
        public Node(Point3d pt)
        {
            X = pt.X;
            Y = pt.Y;
            Z = pt.Z;
            ID = -999;
        }
        #endregion

        #region properties

        #endregion

        #region methods

        #endregion
    }

    public class Element
    {
        #region fields
        private Node N0;
        private Node N1;
        private Line ElemLine;
        private double Length;
        #endregion

        #region constructors
        public Element(Line line)
        {
            ElemLine = line;

        }
        #endregion

        #region properties
        
        #endregion

        #region methods

        #endregion
    }

    public class TEST11A : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public TEST11A()
          : base("Test11A", "A",
              "TestAcomponent",
              "PTK", "STR")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddLineParameter("lines", "lns", "lines", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            // pManager.AddFieldParameter
            // pManager.AddGenericParameter()
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            
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
                return null;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("0f259d4d-3cf2-4337-9545-c392178e1fe1"); }
        }
    }
}
