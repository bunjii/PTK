using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System.Threading.Tasks;

using System.Windows.Forms;

namespace PTK
{
    public class PTK3_Materializer : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        /// 
        
        public PTK3_Materializer()
          : base("Materializer", "MT",
              "This component materializes curves. This based on cross-sections, material and alignments",
              "PTK", "1_INPUT")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "N", "Add name of the group here", GH_ParamAccess.item, "Untitled");
            pManager.AddCurveParameter("Curves", "Crv", "Add element-curves that shall be materalized", GH_ParamAccess.list);
            pManager.AddGenericParameter("CrossSection", "CS", "Add the cross-section componentt here", GH_ParamAccess.item);
            pManager.AddGenericParameter("Material", "MT", "Add Material-component here",GH_ParamAccess.item);
            pManager.AddGenericParameter("Align", "A", "Describes the alignment of the member. (Rotation and offset)", GH_ParamAccess.item);
            pManager.AddGenericParameter("Forces", "F", "Add Forces-component here", GH_ParamAccess.item);
            pManager.AddTextParameter("Tags", "T", "Add tags to the structure here. Tags are individual to each element", GH_ParamAccess.tree);
            pManager.AddIntegerParameter("Priority", "P", "Add a integer value that defines the priority of the member", GH_ParamAccess.list);
            
            pManager[0].Optional = true;
            pManager[2].Optional = true;
            pManager[3].Optional = true;
            pManager[4].Optional = true;
            pManager[5].Optional = true;
            pManager[6].Optional = true;
            pManager[7].Optional = true;

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Element", "E", "The output from the Materializer are elements", GH_ParamAccess.item);
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
            List<Element> elems = new List<Element>();
            List<Node> nodes = new List<Node>();
            List<Align> alignList = new List<Align>();
            Align aligner;
            
            string elemTag = "N/A";
            List<Vector3d> normalVec = new List<Vector3d>();
            GH_ObjectWrapper wrapSec = new GH_ObjectWrapper();
            GH_ObjectWrapper  wrapMat = new GH_ObjectWrapper();
            GH_ObjectWrapper wrapAlign = new GH_ObjectWrapper();
            GH_ObjectWrapper wrapForc = new GH_ObjectWrapper();

            Section section;
            Material material;
            Forces forces;
            Align align;
            #endregion

            #region input
            DA.GetData(0, ref elemTag);
            if (!DA.GetDataList(1, curves)) { return; }
            DA.GetData(2, ref wrapSec);
            DA.GetData(3, ref wrapMat);
            DA.GetData(4, ref wrapAlign);
            DA.GetData(5, ref wrapForc);
            
            #endregion

            #region solve
            //Turning objectwrappers into its respective objects. 
            wrapSec.CastTo<Section>(out section);   
            wrapMat.CastTo<Material>(out material);
            wrapAlign.CastTo<Align>(out align);
            wrapForc.CastTo<Forces>(out forces);


            //Assigning Default Values if not inputed (correctly)
            if (section == null)
            {
                section = new Section("Untitled", 100, 100);
            }
            if (forces == null)
            {
                forces = new Forces();
            }
            if (material == null)
            {
                // material = new Material("untitled", 10, new MatProps("Untitled", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)); //Marcin: Add something generic here
                material = new Material(new MatProps("Untitled", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
            }
            if (align == null)
            {
                align = new Align("Untitled", new Vector3d(0, 0, 1), new Vector3d(0, 0, 0));
            }


            elemTag = elemTag.Trim();

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

            //Creating Elements from Curves
            else
            {
                for (int i = 0; i < curves.Count; i++)
                {
                    if (curves[i] == null) continue;
                    if (!curves[i].IsValid) continue;

                    elems.Add(new Element(curves[i], elemTag, align, section, material));
                    // MessageBox.Show(elems[elems.Count-1].MatId.ToString());
                }
            }

            #endregion

            #region output

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
                return PTK.Properties.Resources.icontest1;
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
