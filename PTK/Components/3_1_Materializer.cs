using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PTK
{
    public class PTK_3_Materializer : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        /// 

        public PTK_3_Materializer()
          : base("Materializer (PTK)", "Materializer",
              "creates a beam element.",
              CommonProps.category, CommonProps.subcat2)
        {
            Message = CommonProps.initialMessage;
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("tag", "tag", "Add tags to the structure here.", GH_ParamAccess.item, "Untitled");
            pManager.AddCurveParameter("curve", "crv", "Add curves that shall be materalized", GH_ParamAccess.list);
            pManager.AddGenericParameter("PTK Cross Section", "CS (PTK)", "Add the cross-section componentt here", GH_ParamAccess.item);
            pManager.AddGenericParameter("PTK Material", "M (PTK)", "Add Material-component here", GH_ParamAccess.item);
            pManager.AddGenericParameter("PTK Align", "Aln (PTK)", "Describes the alignment of the member. (Rotation and offset)", GH_ParamAccess.item);
            pManager.AddGenericParameter("PTK Force", "F (PTK)", "Add Forces-component here", GH_ParamAccess.item);

            pManager[0].Optional = true;
            pManager[2].Optional = true;
            pManager[3].Optional = true;
            pManager[4].Optional = true;
            pManager[5].Optional = true;

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PTK Element", "E (PTK)", "PTK Elements", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            #region variables

            List<Curve> curves = new List<Curve>();
            List<Point3d> pts = new List<Point3d>();
            List<PTK_Element> elems = new List<PTK_Element>();
            List<Node> nodes = new List<Node>();
            List<Align> alignList = new List<Align>();
            // Align aligner;

            string elemTag = "N/A";
            List<Vector3d> normalVec = new List<Vector3d>();
            GH_ObjectWrapper wrapSec = new GH_ObjectWrapper();
            GH_ObjectWrapper wrapMat = new GH_ObjectWrapper();
            GH_ObjectWrapper wrapAlign = new GH_ObjectWrapper();
            GH_ObjectWrapper wrapForce = new GH_ObjectWrapper();

            Section section;
            PTK_Material material;
            PTK_Forces forces;
            Align align;
            #endregion

            #region input
            DA.GetData(0, ref elemTag);
            if (!DA.GetDataList(1, curves)) { return; }
            DA.GetData(2, ref wrapSec);
            DA.GetData(3, ref wrapMat);
            DA.GetData(4, ref wrapAlign);
            DA.GetData(5, ref wrapForce);

            #endregion

            #region solve
            //Turning objectwrappers into its respective objects. 
            wrapSec.CastTo<Section>(out section);
            wrapMat.CastTo<PTK_Material>(out material);
            wrapAlign.CastTo<Align>(out align);
            wrapForce.CastTo<PTK_Forces>(out forces);


            //Assigning Default Values if not inputed (correctly)
            if (section == null) section = new Section("Untitled", 100, 100);
            if (forces == null) forces = new PTK_Forces();
            if (material == null)
            {
                // material = new Material("untitled", 10, new MatProps("Untitled", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)); //Marcin: Add something generic here
                material = new PTK_Material(
                    new PTK_MatProps("Untitled", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
            }
            if (align == null) align = new Align("Untitled", new Vector3d(0, 0, 1), new Vector3d(0, 0, 0));

            elemTag = elemTag.Trim();

            /* 
            // trial multi-threading by john, need to understand this.
            if (curves.Count > 20)
            {
                Parallel.For(0, curves.Count, (int i) =>
                {
                    if (curves[i] != null)
                    {
                        if (!curves[i].IsValid) { return; }
                        elems.Add(new Element(curves[i], elemTag, align, section, material));
                    }

                });
            }

            // non multi-threading way. Creating Elements from Curves
            else
            {
                for (int i = 0; i < curves.Count; i++)
                {
                    if (curves[i] == null) continue;
                    if (!curves[i].IsValid) continue;

                    elems.Add(new Element(curves[i], elemTag, align, section, material));
                }
            }*/
            for (int i = 0; i < curves.Count; i++)
            {
                if (curves[i] == null) continue;
                if (!curves[i].IsValid) continue;

                elems.Add(new PTK_Element(curves[i], elemTag, align, section, material ));
            }

            #endregion

            #region output
            // ExpireSolution(false);
            DA.SetData(0, elems);
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
                return PTK.Properties.Resources.ico_materializer;
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
