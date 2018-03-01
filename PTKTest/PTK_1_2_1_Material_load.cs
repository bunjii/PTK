using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Data;

namespace PTK
{
    public class PTK_1_2_1_Material_load : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public PTK_1_2_1_Material_load()
          : base("1_2_1_Material_load", "MTp",
              "Load material properties from tree here",
              "PTK", "Materializer")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("MaterialName", "MN", "Name the material", GH_ParamAccess.item, "GL26c");      //We should add default values here.

            pManager.AddTextParameter("Load data", "dataTree", "Load data tree with properties", GH_ParamAccess.tree);


        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Material", "M", "MaterialData to be connected with MaterializerComponent", GH_ParamAccess.item);

            pManager.AddTextParameter("Properties list", "List", "List of timber properties", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            #region variables
            string MaterialName = "N/A";

            // for glulam according LIMTREBOKA
            double fmgk = new double();
            double ft0gk = new double();
            double ft90gk = new double();
            double fvgk = new double();
            double frgk = new double();

            double E0gmean = new double();
            double E0g05 = new double();
            double E90gmean = new double();
            double E90g05 = new double();

            double Ggmean = new double();
            double Gg05 = new double();
            double Gtgmean = new double();
            double Grg05 = new double();

            double Qgk = new double();
            double Qgmean = new double();
            #endregion

            
           GH_Structure<GH_String> Tree = new GH_Structure<GH_String>();
            List<string> nlist = new List<string>();

            #region input
            DA.GetData(0, ref MaterialName) ;
            DA.GetDataTree(1, out Tree ) ;
            #endregion
           
            
            #region sorting


            for (int k = 0; k < Tree.get_Branch(0).Count; k++)
            {
                GH_Path pth = new GH_Path(k);
                
                
                if (MaterialName == Tree.get_Branch(0)[k].ToString() )
                {
                    //B = Tree.get_Branch(0)[k];
                    for (int kk = 1; kk < Tree.Branches.Count(); kk++)
                    {
                        nlist.Add(Tree.get_Branch(kk)[k].ToString());
                    }

                }
                
            }

            #endregion

            double.Parse(nlist[0]);
             fmgk= double.Parse(nlist[0]);
            ft0gk = double.Parse(nlist[0]);
            ft90gk = double.Parse(nlist[0]);
            fvgk = double.Parse(nlist[0]);
            frgk = double.Parse(nlist[0]);

            E0gmean = double.Parse(nlist[0]);
            E0g05 = double.Parse(nlist[0]);
            E90gmean = double.Parse(nlist[0]);
            E90g05 = double.Parse(nlist[0]);

            Ggmean = double.Parse(nlist[0]);
            Gg05 = double.Parse(nlist[0]);
            Gtgmean = double.Parse(nlist[0]);
            Grg05 = double.Parse(nlist[0]);

            Qgk = double.Parse(nlist[0]);
            Qgmean = double.Parse(nlist[0]);

            #region solve
            Material_properties Material_prop = new Material_properties(
             MaterialName,
             fmgk,
             ft0gk,
             ft90gk,
             fvgk,
             frgk,

             E0gmean,
             E0g05,
             E90gmean,
             E90g05,

             Ggmean,
             Gg05,
             Gtgmean,
             Grg05,

             Qgk,
             Qgmean
                );


            #endregion

            #region output
            DA.SetData(0, Material_prop);
            DA.SetDataList(1, nlist);
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
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("965bef7b-feea-54d8-abe9-f126d21b9c41"); }
        }
    }
}
