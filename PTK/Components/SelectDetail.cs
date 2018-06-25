using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;

namespace PTK.Components
{
    public class SelectDetail : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the SelectDetail class.
        /// </summary>
        public SelectDetail()
          : base("SelectDetail", "Nickname",
              "Description",
              "Category", "Subcategory")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("DetailingGroupName", "", "", GH_ParamAccess.item);
            pManager.AddGenericParameter("PTK Assembly", "PTK A", "PTK Assembly", GH_ParamAccess.item);
            // pManager.AddGenericParameter("PTK LOGIC", "PTK LOGIC", "COLLECTIONS OF DETAIL SELECTIONS", GH_ParamAccess.item);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            //This output would be nice to be variable for PTK1.0
            pManager.AddGenericParameter("N", "Node", "", GH_ParamAccess.list);
            pManager.AddGenericParameter("E0", "Elem0", "", GH_ParamAccess.list);
            pManager.AddGenericParameter("E1", "Elem1", "", GH_ParamAccess.list);
            pManager.AddGenericParameter("E2", "Elem2", "", GH_ParamAccess.list);
            pManager.AddGenericParameter("E3", "Elem3", "", GH_ParamAccess.list);
            pManager.AddGenericParameter("E4", "Elem4", "", GH_ParamAccess.list);



        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            #region variables

            Assembly assemble = null;
            string Name = "";

            DA.GetData(0, ref Name);

            DA.GetData(1, ref assemble);



            List<Detail> Details = assemble.DetailingGroups.Find(t => t.Name == Name).Details;

            List<Node> Nodes = new List<Node>();
            List<Element> Elements0 = new List<Element>();
            List<Element> Elements1 = new List<Element>();
            List<Element> Elements2 = new List<Element>();
            List<Element> Elements3 = new List<Element>();
            List<Element> Elements4 = new List<Element>();

            //The next one is stupid code, but did not find a better solution. The idea is to output each element of the detail in different output. The challenge is that the element-count varies
            foreach (Detail Detail in Details)
            {
                List<Element> elemsInDetail = new List<Element>();
                for (int i = 0; i < Detail.ElemsIds.Count; i++)
                {
                    elemsInDetail.Add(assemble.Elems.Find(t => t.Id == Detail.ElemsIds[i]));
                }

                Nodes.Add(assemble.Nodes.Find(t => t.Id == Detail.NodeIds[0]));

                elemsInDetail = elemsInDetail.OrderBy(t => -t.Priority).ToList();


                if (elemsInDetail.Count > 0)
                {
                    Elements0.Add(elemsInDetail[0]);
                }
                if (elemsInDetail.Count > 1)
                {
                    Elements1.Add(elemsInDetail[1]);
                }
                if (elemsInDetail.Count > 2)
                {
                    Elements2.Add(elemsInDetail[2]);
                }
                if (elemsInDetail.Count > 3)
                {
                    Elements3.Add(elemsInDetail[3]);
                }
                if (elemsInDetail.Count > 4)
                {
                    Elements4.Add(elemsInDetail[4]);
                }
            }

            DA.SetDataList(0, Nodes);
            DA.SetDataList(1, Elements0);
            DA.SetDataList(2, Elements1);
            DA.SetDataList(3, Elements2);
            DA.SetDataList(4, Elements3);
            DA.SetDataList(5, Elements4);


            #endregion



            #region solve


            #endregion

            #region output
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
                return PTK.Properties.Resources.GA_icon;
            }
        }


        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("b05d02e4-ad84-4793-ad59-9397559c0ea5"); }
        }
    }
}