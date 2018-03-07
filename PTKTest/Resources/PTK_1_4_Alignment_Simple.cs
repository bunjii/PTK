using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace PTK
{
    public class PTK1_4_Alignment_Simple : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the PTK1_4_Alignment class.
        /// </summary>
        public PTK1_4_Alignment_Simple()
          : base("Alignment", "Nickname",
              "Description",
              "PTK", "Materializer")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("AlignmentName", "a", "Alignmentname is optional. One or similiar amount as curve", GH_ParamAccess.item,"Untitled");
            pManager.AddVectorParameter("Global Z-vector", "z", "Direction of the global z(height)-vector", GH_ParamAccess.list,new Vector3d(0,0,1));
            pManager.AddNumberParameter("Offset Local y", "local y", "Offset length local y", GH_ParamAccess.list,0);
            pManager.AddNumberParameter("Offset Local z", "local y", "Offset length local z", GH_ParamAccess.list,0);

            pManager[0].Optional = true;
            pManager[1].Optional = true;
            pManager[2].Optional = true;
            pManager[3].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Alignment", "A,", "AlignmentData to be added to materializer", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            #region variables
            string alignmentname = "N/A";
            List<Vector3d> globalZvector = new List<Vector3d>();
            List<double> offsetY = new List<double>();
            List<double> offsetZ = new List<double>();

            #endregion

            #region input

            if (!DA.GetData(0, ref alignmentname)) { return; }
            if (!DA.GetDataList(1, globalZvector)) { return; }
            if (!DA.GetDataList(2,  offsetY)) { return; }
            if (!DA.GetDataList(3,  offsetZ)) { return; }

            #endregion

            #region solve

            List<int> listlengths = new List<int>();
            listlengths.Add(globalZvector.Count);
            listlengths.Add(offsetY.Count);
            listlengths.Add(offsetZ.Count);
            listlengths.Sort();

            List<Align> simplAlign = new List<Align>();

            simplAlign.Add(new Align(alignmentname, globalZvector[0], offsetY[0], offsetZ[0]));



            if (Functions.CheckInputValidity(listlengths))
            {
                for (int i = 0; i< listlengths[2]; i++)
                {
                       
                }
            }
            else
            {
                //ADD A ERROR MESSAGE SAYING: Listlength of offset y, offset z and globalzvector must be either 1 or similar to the larges. (And must be similar to the length of the member)
            }

            #endregion

            #region output
            DA.SetData(0, new Align(alignmentname, globalZvector[0], offsetY[0], offsetZ[0]));
            #endregion


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
                return PTK.Properties.Resources.icontest2;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("c0633721-663b-49cf-b124-3e4d39647266"); }
        }
    }
}