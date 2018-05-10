using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace PTK
{
    public class PTK_UTIL_1 : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the PTK_UTIL_1 class.
        /// </summary>
        public PTK_UTIL_1()
          : base("Generate Geometry", "Geom (PTK)",
              "Generating Mesh or Brep Geometry",
              "PTK", "UTIL")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("PTK Assembly", "PTK A", "Input PTK Assembly", GH_ParamAccess.item);
            pManager.AddBooleanParameter("IsMesh", "IsMesh", "IsMesh", GH_ParamAccess.item, false);
            pManager.AddBooleanParameter("IsBrep", "IsBrep", "IsBrep", GH_ParamAccess.item, false);

            pManager[1].Optional = true;
            pManager[2].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddBrepParameter("BREP /S", "BREP /S", "BREP /S", GH_ParamAccess.list);
            pManager.AddBrepParameter("BREP", "BREP", "BREP", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            #region variables
            GH_ObjectWrapper wrapAssembly = new GH_ObjectWrapper();
            Assembly assemble;
            Boolean isMesh = false;
            Boolean isBrep = false;
            List<Node> nodes = new List<Node>();
            List<Element> elems = new List<Element>();
            List<Section> secs = new List<Section>();
            List<Brep> brepGeom = new List<Brep>();
            List<Brep> slashedBreps = new List<Brep>();
            #endregion

            #region input
            if (!DA.GetData(0, ref wrapAssembly)) { return; };
            wrapAssembly.CastTo<Assembly>(out assemble);
            DA.GetData(1, ref isMesh);
            DA.GetData(2, ref isBrep);
            #endregion

            #region solve
            nodes = assemble.Nodes;
            elems = assemble.Elems;
            secs = assemble.Secs;

            if (isBrep == true)
            {
                foreach (Element e in elems)
                {
                    // from Element.GenerateIntervals()
                    Interval iz;
                    Interval iy;
                    Interval ix;
                    Rectangle3d crossSectionRectangle;
                    Brep elementGeometry;
                    BoundingBox boundingbox;

                    double[] paramter = { 0.0, 2.2 };

                    Section sec = Section.FindSecById(secs, e.SecId);
                    double HalfWidth = 0.5 * sec.Width;
                    double HalfHeight = 0.5 * sec.Height;

                    iz = new Interval(-HalfHeight, HalfHeight);
                    iy = new Interval(-HalfWidth, HalfWidth);
                    ix = new Interval(0, e.Crv.GetLength());
                    crossSectionRectangle = new Rectangle3d(e.localYZPlane, iy, iz);

                    // from Element.GenerateElementGeometry()
                    Brep oneBeamGeom = new Brep();

                    if (e.Crv.IsLinear())
                    {
                        Box boxen = new Box(e.localYZPlane, iy, iz, ix);
                        oneBeamGeom = Brep.CreateFromBox(boxen);
                        boundingbox = boxen.BoundingBox;
                    }
                    else
                    {
                        SweepOneRail tempsweep = new SweepOneRail();
                        var sweep = tempsweep.PerformSweep(e.Crv, crossSectionRectangle.ToNurbsCurve());
                        oneBeamGeom = sweep[0].CapPlanarHoles(ProjectProperties.tolerances);
                        boundingbox = oneBeamGeom.GetBoundingBox(Rhino.Geometry.Plane.WorldXY);
                    }

                    brepGeom.Add(oneBeamGeom);
                    
                }

                List<Brep> tempBrep = Functions_DDL.OperatePriority(nodes, elems, ref brepGeom);
                slashedBreps.AddRange(tempBrep);
            }

            if (isMesh == true)
            {
                // mesh
            }
            
            #endregion

            #region output
            
            DA.SetDataList(0, slashedBreps);
            DA.SetDataList(1, brepGeom);
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
                return PTK.Properties.Resources.icontest14;

            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("38eb4f7f-a3bc-4563-8ebe-dd37784db737"); }
        }
    }
}