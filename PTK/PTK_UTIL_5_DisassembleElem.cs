using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using Grasshopper;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

using Rhino.Geometry;

namespace PTK
{
    public class PTK_UTIL_5 : GH_Component
    {

        /// <summary>
        /// Initializes a new instance of the TestC class.
        /// </summary>
        public PTK_UTIL_5()
          : base("Disassemble Element", "DA_E (PTK)",
              "Disassemble Element (PTK)",
              "PTK", "UTIL") 
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("PTK ELEM", "PTK E", "PTK ELEM", GH_ParamAccess.item);
            pManager.AddTextParameter("tag", "tag", "", GH_ParamAccess.list);

            pManager[1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "Curve", "Curve", GH_ParamAccess.list);
            pManager.AddTextParameter("Tag", "Tag", "Tag", GH_ParamAccess.list);
            pManager.AddIntegerParameter("PTK ELEM ID", "PTK E ID", "PTK ELEM ID", GH_ParamAccess.list);
            // pManager.AddIntegerParameter("PTK NODE ID 0", "PTK N0 ID", "PTK NODE ID 0", GH_ParamAccess.list);
            // pManager.AddIntegerParameter("PTK NODE ID 1", "PKT N1 ID", "PTK NODE ID 1", GH_ParamAccess.list);
            pManager.AddGenericParameter("PTK SECTION", "PTK S", "PTK SECTION", GH_ParamAccess.list);
            pManager.AddPlaneParameter("local yz plane", "yz-plane", "returns local yz plane", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Node ID", "NID", "", GH_ParamAccess.tree);
            pManager.AddNumberParameter("ParameterConnectedNodes", "PCN", "", GH_ParamAccess.tree);
            pManager.AddBoxParameter("BoundingBox", "BB", "", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Mat ID", "MID", "Material ID", GH_ParamAccess.list);
            pManager.AddLineParameter("Structural Lines", "STR LNS", "Structural Lines", GH_ParamAccess.tree);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            #region variables
            GH_ObjectWrapper wrapElem = new GH_ObjectWrapper();
            List<Element> elems = new List<Element>();
            List<Element> outElems = new List<Element>();
            List<string> elemTags = new List<string>();
            List<String> inputTags = new List<string>();
            List<Curve> curves = new List<Curve>();
            List<int> elemids = new List<int>();
            // List<int> n0ids = new List<int>();
            // List<int> n1ids = new List<int>();
            List<Section> secs = new List<Section>();
            List<Plane> plns = new List<Plane>();
            List<int> mids = new List<int>();
            
            List<BoundingBox> bbox = new List<BoundingBox>();

            DataTree<int> nidTr = new DataTree<int>();
            DataTree<double> pcnTr = new DataTree<double>();
            DataTree<Line> lines = new DataTree<Line>();
            #endregion

            #region input
            if (!DA.GetData(0, ref wrapElem)) { return;  }
            wrapElem.CastTo<List<Element>>(out elems);

            DA.GetDataList(1, inputTags);
            #endregion
            // MessageBox.Show(elems[0].MatId.ToString());
            // MessageBox.Show(elems[0].localYZPlane.ToString());

            #region solve
            // Detect Elements 
            if (inputTags.Count == 0)
            {
                outElems = elems;
            }
            else
            {
                for (int i=0; i<inputTags.Count; i++)
                {
                    inputTags[i] = inputTags[i].Trim();
                }

                foreach (Element e in elems)
                {
                    if (!inputTags.Contains(e.Tag)) continue;

                    outElems.Add(e);
                }
            }
            // foreach (Element e in outElems)
            for (int i=0; i<outElems.Count;i++)
            {
                curves.Add(outElems[i].Crv);
                elemTags.Add(outElems[i].Tag);
                elemids.Add(outElems[i].Id);
                // e.SubStructural[0].StrctrLine;
                //n0ids.Add(e.N0id);
                //n1ids.Add(e.N1id);
                secs.Add(outElems[i].Section);
                plns.Add(outElems[i].localYZPlane);

                mids.Add(outElems[i].MatId);

                GH_Path pth = new GH_Path(i);
                for (int j = 0; j < outElems[i].NodeParams.Count; j++)
                {
                    pcnTr.Add(outElems[i].NodeParams[j], pth);
                    nidTr.Add(outElems[i].NodeIds[j], pth);
                }
                
                for (int j=0; j< outElems[i].SubStructural.Count; j++)
                {
                    lines.Add(outElems[i].SubStructural[j].StrctrLine, pth);
                }

                bbox.Add(outElems[i].BoundingBox);
            }
            #endregion

            #region output
            DA.SetDataList(0, curves);
            DA.SetDataList(1, elemTags);
            DA.SetDataList(2, elemids);
            // DA.SetDataList(3, n0ids);
            // DA.SetDataList(4, n1ids);
            DA.SetDataList(3, secs);
            DA.SetDataList(4, plns);
            DA.SetDataTree(5, nidTr);
            DA.SetDataTree(6, pcnTr);
            DA.SetDataList(7, bbox);
            DA.SetDataList(8, mids);
            DA.SetDataTree(9, lines);
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
                return PTK.Properties.Resources.icon_truss;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("891a0366-cf2f-4642-b92b-4a93d0389330"); }
        }

    }

}