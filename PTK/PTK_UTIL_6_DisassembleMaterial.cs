using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

using Rhino.Geometry;

namespace PTK
{
    public class DisassembleMaterial : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the PTK_UTIL6_DisassembleMaterial class.
        /// </summary>
        public DisassembleMaterial()
          : base("Disassemble Material", "DA_M (PTK)",
              "Disassemble Material (PTK)",
              "PTK", "UTIL")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("PTK MATERIAL", "PTK M", "PTK MATERIAL", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Material Name", "Mat Name", "", GH_ParamAccess.list);
            pManager.AddTextParameter("Material Properties", "Mat Prop","", GH_ParamAccess.tree);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            #region variables
            GH_ObjectWrapper wrapMat = new GH_ObjectWrapper();
            // in case the component is connected to somewhere after Assemble.
            List<Material> mats = new List<Material>();
            // in case the component is connected directly to Material.
            Material mat;

            List<string> matNames = new List<string>();
            DataTree<double> matPropTree = new DataTree<double>();
            #endregion

            #region input
            if (!DA.GetData(0, ref wrapMat)) { return; }
            wrapMat.CastTo<List<Material>>(out mats);
            wrapMat.CastTo<Material>(out mat);
            #endregion

            #region solve
            if (mats.Count == 0)
            {
                mats.Clear();
                mats.Add(mat);
            }
            for (int i = 0; i<mats.Count;i++)
            {
                Material_properties mp = mats[i].Properties;

                matNames.Add(mp.MaterialName);
                GH_Path path = new GH_Path(i);
                double[] props = {
                    mp.Fmgk,
                    mp.Ft0gk,
                    mp.Ft90gk,
                    mp.Fc0gk,
                    mp.Fc90gk,

                    mp.Fvgk,
                    mp.Frgk,
                    mp.EE0gmean,
                    mp.EE0g05,
                    mp.EE90gmean,

                    mp.EE90g05,
                    mp.GGgmean,
                    mp.GGg05,
                    mp.GGrgmean,
                    mp.GGrg05,

                    mp.Rhogk,
                    mp.Rhogmean
                };
                matPropTree.AddRange(props, path);
            }
            #endregion

            #region output
            DA.SetDataList(0, matNames);
            DA.SetDataTree(1, matPropTree);
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
            get { return new Guid("0b73c6b4-2875-4e18-9ee1-bfd80c8d75cb"); }
        }
    }
}