using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PTK
{

    public class PTK_4_Assembly : GH_Component
    {

        public PTK_4_Assembly()
          : base("Assembly", "Assembly",
              "Assembly",
              CommonProps.category, CommonProps.subcate1)
        {
            Message = CommonProps.initialMessage;
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new Param_Element1D(), "Elements", "E", "Add elements here", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.RegisterParam(new Param_Assembly(), "Assembly", "A", "Assembled project data", GH_ParamAccess.item);
            pManager.RegisterParam(new Param_Node(), "Nodes", "N", "Nodes included in the Assembly", GH_ParamAccess.list);
            pManager.AddTextParameter("Tags", "T", "Tag list held by Elements included in Assemble", GH_ParamAccess.list);
            pManager.RegisterParam(new Param_Material(), "Materials", "M", "Material list held by Elements included in Assemble", GH_ParamAccess.list);
            pManager.RegisterParam(new Param_CrossSection(), "CrossSection", "S", "CrossSection list held by Elements included in Assemble", GH_ParamAccess.list);
        }


        protected override void SolveInstance(IGH_DataAccess DA)
        {
            #region variables
            Assembly assembly = new Assembly();
            List<GH_Element1D> gElems = new List<GH_Element1D>();
            List<Element1D> elems = null;
            #endregion

            #region input
            if (!DA.GetDataList(0, gElems))
            {
                elems = new List<Element1D>();
            }
            else
            {
                elems = gElems.ConvertAll(e => e.Value);
            }
            #endregion

            #region solve
            foreach(Element1D elem in elems)
            {
                assembly.AddElement(elem);
            }
            #endregion

            #region output

            List<GH_Node> nodes = new List<GH_Node>();
            foreach(Node n in assembly.Nodes)
            {
                nodes.Add(new GH_Node(n));
            }
            List<string> tags = assembly.Tags;
            List<GH_Material> materials = new List<GH_Material>();
            foreach(Material m in assembly.Materials)
            {
                materials.Add(new GH_Material(m));
            }
            List<GH_CrossSection> sections = new List<GH_CrossSection>();
            foreach(CrossSection sec in assembly.CrossSections)
            {
                sections.Add(new GH_CrossSection(sec));
            }
            
            DA.SetData(0, new GH_Assembly(assembly));
            DA.SetDataList(1, nodes);
            DA.SetDataList(2, tags);
            DA.SetDataList(3, materials);
            DA.SetDataList(4, sections);
            #endregion

        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return PTK.Properties.Resources.ico_assemble;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("d16b2f49-a170-4d47-ae63-f17a4907fed1"); }
        }
    }
}
