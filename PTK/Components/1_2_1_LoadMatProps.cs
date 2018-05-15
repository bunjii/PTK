using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Data;

namespace PTK
{
    public class PTK_1_2_1_LoadMatProps : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public PTK_1_2_1_LoadMatProps()
          : base("Load Material Properties (PTK)", "Load MP",
              "loads material properties from Tree.",
              CommonProps.category, "Materialize")
        {
            Message = "PTK";
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Material Name", "MatName", "names Material.", GH_ParamAccess.item, "GL26c");      //We should add default values here.
            pManager.AddTextParameter("Load data", "dataTree", "Load data tree with properties.", GH_ParamAccess.tree);
            
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Material Properties (PTK)", "MP (PTK)", "Material Property (PTK) data to be connected to a Material (PTK) component", GH_ParamAccess.item);
            pManager.AddTextParameter("Material Properties text", "MP txt", "Text output of Material Properties (PTK)", GH_ParamAccess.list);
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

            double fc0gk = new double();
            double fc90gk = new double();

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

            // double Qgk = new double();
            // double Qgmean = new double();

            double rhogk = new double();
            double rhogmean = new double();
            
            GH_Structure<GH_String> Tree = new GH_Structure<GH_String>();

            List<string> nlist = new List<string>();
            #endregion

            #region input
            DA.GetData(0, ref MaterialName) ;
            DA.GetDataTree(1, out Tree ) ;
            #endregion


            #region solve
            // check locale: "comma" or "period"
            string decimalSeparator = CommonProps.FindDecimalSeparator();
            bool comma = false, period = false; // to check if text contains comma or period.
             
            // registering materials
            for (int i = 0; i < Tree.get_Branch(0).Count; i++)
            {
                GH_Path pth = new GH_Path(i);

                // if MaterialName doesn't match, move on to next loop.
                if (MaterialName != Tree.get_Branch(0)[i].ToString())
                    continue;

                // obtain material properties with the matching "MN"
                for (int j = 1; j < Tree.Branches.Count(); j++)
                {
                    string txt = Tree.get_Branch(j)[i].ToString();
                    nlist.Add(txt);
                    if (txt.Contains(",")) comma = true;
                    else if (txt.Contains(".")) period = true;
                }
            }
            
            // comma, period decimal conversion
            for (int i=0;i<nlist.Count;i++)
            {
                bool convert = false;
                string convertedTxt = "";

                if (decimalSeparator == "comma")
                {
                    if (period == false) continue;

                    // if csv includes "period", it needs treatment
                    convertedTxt = Functions_DDL.ConvertCommaToPeriodDecimal(nlist[i], true);
                }
                else if (decimalSeparator == "period")
                {
                    if (comma == false) continue;

                    // if csv includes "comma", it needs treatment
                    convertedTxt = Functions_DDL.ConvertCommaToPeriodDecimal(nlist[i]);
                }
                else
                {
                    // exception
                    convertedTxt = nlist[i];
                }

                nlist[i] = convertedTxt;
            }
            
            fmgk = double.Parse(nlist[0]);
            ft0gk = double.Parse(nlist[1]);
            ft90gk = double.Parse(nlist[2]);

            fc0gk = double.Parse(nlist[3]);
            fc90gk = double.Parse(nlist[4]);

            fvgk = double.Parse(nlist[5]);
            frgk = double.Parse(nlist[6]);

            E0gmean = double.Parse(nlist[7]);
            E0g05 = double.Parse(nlist[8]);
            E90gmean = double.Parse(nlist[9]);
            E90g05 = double.Parse(nlist[10]);

            Ggmean = double.Parse(nlist[11]);
            Gg05 = double.Parse(nlist[12]);
            Gtgmean = double.Parse(nlist[13]);
            Grg05 = double.Parse(nlist[14]);

            // Qgk = double.Parse(nlist[0]);
            // Qgmean = double.Parse(nlist[0]);

            rhogk = double.Parse(nlist[15]);
            rhogmean = double.Parse(nlist[16]);

            MatProps matProp = new MatProps(
             MaterialName,
             fmgk,
             ft0gk,
             ft90gk,

             fc0gk,
             fc90gk,

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

             // Qgk,
             // Qgmean,

             rhogk,
             rhogmean
                );


            #endregion
            
            // MessageBox.Show(matProp.Fmgk.ToString());//0 -> 24

            #region output
            DA.SetData(0, matProp);
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
                return PTK.Properties.Resources.icontest0;
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
