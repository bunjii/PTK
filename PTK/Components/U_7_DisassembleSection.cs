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
    public class PTK_U_7_DisassembleSection : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the PTK_UTIL_7_DisassembleSection class.
        /// </summary>
        public PTK_U_7_DisassembleSection()
          : base("Disassemble Section (PTK)", "X Section",
              "Disassemble Section (PTK)",
              "PTK", "Utility")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("PTK SECTION", "S (PTK)", "PTK SECTION", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "Name", "Section Name", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Section Id", "Sec Id", "Section Id", GH_ParamAccess.list);
            pManager.AddNumberParameter("Width", "Width", "Width", GH_ParamAccess.list);
            pManager.AddNumberParameter("Height", "Height", "Height", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Element Ids", "Elem Ids", "", GH_ParamAccess.tree);
            pManager.AddTextParameter("Section Hash", "Sec Hash", "Sec Hash", GH_ParamAccess.list);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            #region variables
            GH_ObjectWrapper wrapSec = new GH_ObjectWrapper();

            // in case the component is connected to somewhere after Assemble.
            List<Section> secs = new List<Section>();
            // in case the component is connected directly to Section component.
            Section sec;

            List<string> secHashes = new List<string>();
            List<string> secNames = new List<string>();
            List<int> secIds = new List<int>();
            List<double> secWidth = new List<double>();
            List<double> secHeight = new List<double>();
            DataTree<int> elemIdsTree = new DataTree<int>();
            #endregion

            #region input
            if (!DA.GetData(0, ref wrapSec)) { return; }
            wrapSec.CastTo<List<Section>>(out secs);
            wrapSec.CastTo<Section>(out sec);
            #endregion

            #region solve
            if (secs.Count == 0)
            {
                secs.Clear();
                secs.Add(sec);
            }

            for (int i = 0; i < secs.Count; i++)
            {
                secNames.Add(secs[i].SectionName);
                secIds.Add(secs[i].Id);
                secHashes.Add(secs[i].TxtHash);
                secWidth.Add(secs[i].Width);
                secHeight.Add(secs[i].Height);

                GH_Path path = new GH_Path(i);

                List<int> _elemIdLst = new List<int>();
                for (int j=0;j<secs[i].ElemIds.Count; j++)
                {
                    _elemIdLst.Add(secs[i].ElemIds[j]);
                }

                elemIdsTree.AddRange(_elemIdLst, path);
                
            }

            #endregion

            #region output
            DA.SetDataList(0, secNames);
            DA.SetDataList(1, secIds);
            DA.SetDataList(2, secWidth);
            DA.SetDataList(3, secHeight);
            DA.SetDataTree(4, elemIdsTree);
            DA.SetDataList(5, secHashes);
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
            get { return new Guid("b0854f53-c7ba-4817-a8a2-b9ca136b5524"); }
        }
    }
}